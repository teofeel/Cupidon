using Cupid.DataRepositories;
using Cupid.DataRepositories.Interfaces;
using Cupid.Hubs;
using Cupid.Models;
using Cupid.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Cupid.Services
{
    public class CupidService : ICupidService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<CupidHub> _hubContext;

        private readonly string[] _messages = {
            "Radujem se nasem susretu!",
            "Zelim da se upoznamo.",
            "Nisam zainteresovan/a za upoznavanje"
        };

        public CupidService(IUserRepository userRepository, IHubContext<CupidHub> hubContext)
        {
            _userRepository = userRepository;
            _hubContext = hubContext;
        }

        public async Task SendLetter()
        {
            List<User> users = this._userRepository.GetAllUsers();


            foreach(User receiver in users)
            {
                bool acquiredLock = false;

                try
                {
                    if (!this._userRepository.TrySetBusy(receiver.Username, true)) continue;

                    acquiredLock = true;

                    User? bestMatch = null;
                    int highestScore = -1;

                    foreach (User sender in users)
                    {
                        if (sender.Username == receiver.Username) continue;

                        if (receiver.BlockedUsers.Contains(sender.Username)) continue;

                        int currentScore = this.IncreaseScore(receiver, sender);

                        if (currentScore > highestScore)
                        {
                            highestScore = currentScore;
                            bestMatch = sender;
                        }
                    }

                   

                    if (bestMatch != null && this._userRepository.GetUser(receiver.Username) != null) 
                        await this.PrepareMessage(receiver, bestMatch);
                    else 
                        this._userRepository.TrySetBusy(receiver.Username, false);
                }
                catch(Exception ex)
                {
                    if (acquiredLock) this._userRepository.TrySetBusy(receiver.Username, false);

                    Debug.WriteLine($"SendLetter error {receiver.Username}: {ex.Message}");
                }
            }

            
        }

        private async Task PrepareMessage(User receiver, User bestMatch)
        {

            string randomMessage = this._messages[GetRandomCrypto(0, _messages.Length - 1)];
            string phoneToSend = randomMessage == "Nisam zainteresovan/a za upoznavanje" ? "" : bestMatch.PhoneNumber;
            Letter letter = new Letter(bestMatch.Username, bestMatch.City, bestMatch.Age, randomMessage, phoneToSend);

            //receiver.IsBusy = true;
            //this._userRepository.UpdateUser(receiver);

            await this._hubContext.Clients.Group(receiver.Username).SendAsync("ReceiveLetter", letter);
        }

        private int IncreaseScore(User receiver, User sender)
        {
            int currentScore = 0;

            if (receiver.City.Equals(sender.City, StringComparison.OrdinalIgnoreCase))
                currentScore += 30;

            if (Math.Abs(receiver.Age - sender.Age) <= 2)
                currentScore += 20;


            currentScore += this.GetRandomCrypto(0, 100);

            return currentScore;
        }

        private int GetRandomCrypto(int min, int max)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[4];
                rng.GetBytes(data);

                int value = BitConverter.ToInt32(data, 0);

                return Math.Abs(value % (max-min+1)) + min;
            }
        }
    }
}
