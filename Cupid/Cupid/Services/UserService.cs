using System.Collections.Concurrent;
using Cupid.DataRepositories;
using Cupid.DataRepositories.Interfaces;
using Cupid.Models;
using Cupid.Services.Interfaces;

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
            if (currentUser == null) throw new KeyNotFoundException("User not found");


            currentUser.BlockedUsers.Add(username);

            this._userRepository.UpdateUser(currentUser);
        }

        public void SetUserBusy(string username, bool isBusy)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("Username want sent");

            User? user = this._userRepository.GetUser(username);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.IsBusy = isBusy;
            this._userRepository.UpdateUser(user);
            
        }

        public List<User> GetAllUser()
        {
            return this._userRepository.GetAllUsers();
        }

        public User? GetUserByUsername(string username)
        {
            return this._userRepository.GetUser(username);
        }
    }
}
