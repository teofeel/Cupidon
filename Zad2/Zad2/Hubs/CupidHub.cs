using Microsoft.AspNetCore.SignalR;
using Zad2.Services.Interfaces;

namespace Zad2.Hubs
{
    public class CupidHub : Hub
    {
        private readonly IUserService _userService;

        public CupidHub(IUserService userService)
        {
            this._userService = userService;
        }

        public async Task InitSinglePerson(string username, string city, int age, string phoneNumber)
        {
            try
            {
                bool isRegistered = this._userService.RegisterUser(username, city, age, phoneNumber);

                if (!isRegistered) await Clients.Caller.SendAsync("RegistrationResult", false, $"Couldnt register user");

                await Groups.AddToGroupAsync(Context.ConnectionId, username);
                await Clients.Caller.SendAsync("RegistrationResult", true, $"User registered");
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("RegistrationResult", false, $"Couldnt validate: {ex.Message}");
            }

        }

        public async Task ConfirmReceipt(string username)
        {
            try
            {
                this._userService.SetUserBusy(username, false);
                await Clients.Caller.SendAsync("ConfirmationResult", $"You can now receive new letters");
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ConfirmationResult", $"Error: {ex.Message}");
            }
        }

        public async Task BlockUser(string current, string username)
        {
            try
            {
                this._userService.BlockUser(current, username);
                await Clients.Caller.SendAsync("BlockedUser", true, $"BLocked user: {username}");
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("BlockedUser", false, $"Couldnt block: {ex.Message}");
            }

        }
    }
}
