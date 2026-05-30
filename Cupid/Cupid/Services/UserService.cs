using System.Collections.Concurrent;
using Cupid.DataRepositories;
using Cupid.DataRepositories.Interfaces;
using Cupid.Models;
using Cupid.Services.Interfaces;
using Cupid.Utils;

namespace Cupid.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository repo)
        {
            this._userRepository = repo;

        }

        public bool RegisterUser(string username, string city, int age, string phoneNumber)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(phoneNumber)) 
            { 
                throw new ArgumentNullException("User didn't enter any value"); 
            }

            if (age <= 0) throw new ArgumentOutOfRangeException("User entered negative number");

            User newUser = new User(username, city, age, phoneNumber, false);


            return this._userRepository.AddUser(newUser);
        }

        public void BlockUser(string current, string username)
        {
            if(string.IsNullOrEmpty(current) || string.IsNullOrEmpty(username))
                throw new ArgumentNullException("Usernames want sent");

            User? currentUser = this._userRepository.GetUser(current);
            User? toBlock = this._userRepository.GetUser(username);

            if (currentUser == null) throw new KeyNotFoundException("User not found");
            if(toBlock == null) throw new KeyNotFoundException("User to be blocked doesnt exist");

            if (!this._userRepository.AddBlockedUser(current, username))
                throw new KeyNotFoundException("User not found");
            //currentUser.BlockedUsers.Add(username);

            //this._userRepository.UpdateUser(currentUser);
        }

        //public void SetUserBusy(string username, bool isBusy)
        //{
        //    if (string.IsNullOrEmpty(username))
        //        throw new ArgumentNullException("Username want sent");

        //    User? user = this._userRepository.GetUser(username);
        //    if (user == null) throw new KeyNotFoundException("User not found");

        //    user.IsBusy = isBusy;
        //    this._userRepository.UpdateUser(user);

        //}

        public void SetUserBusy(string username, bool isBusy)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            bool success = this._userRepository.TrySetBusy(username, isBusy);

            if (!success)
            {
                if (this._userRepository.GetUser(username) == null)
                    throw new KeyNotFoundException("User not found");
            }
        }


        public List<User> GetAllUser()
        {
            return this._userRepository.GetAllUsers();
        }

        public User? GetUserByUsername(string username)
        {
            return this._userRepository.GetUser(username);
        }

        public bool RemoveUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;

            return this._userRepository.RemoveUser(username);

        }
    }
}
