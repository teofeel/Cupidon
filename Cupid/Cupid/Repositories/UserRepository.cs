using System.Collections.Concurrent;
using Cupid.DataRepositories.Interfaces;
using Cupid.Models;

namespace Cupid.DataRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();

        public bool AddUser(User user)
        {
            if (user == null || string.IsNullOrEmpty(user?.Username)) return false;
            
            return this._users.TryAdd(user.Username, user);
        }

        public bool UpdateUser(User user)
        {
            if (user == null || string.IsNullOrEmpty(user?.Username)) return false;

            while (this._users.TryGetValue(user.Username, out var oldUser))
            {
                if (this._users.TryUpdate(user.Username, user, oldUser))
                    return true;
            }

            return false;
        }

        public bool RemoveUser(string username)
        {
            if(string.IsNullOrEmpty(username)) return false;

            if (!this._users.ContainsKey(username)) return false;

            return this._users.TryRemove(username, out _);
        }

        public bool TrySetBusy(string username, bool value)
        {
            if (string.IsNullOrEmpty(username)) return false;

            while (this._users.TryGetValue(username, out var oldUser))
            {
                if (oldUser.IsBusy == value) return false;
                User updatedUser = new User(oldUser.Username, oldUser.City, oldUser.Age, oldUser.PhoneNumber, value)
                {
                    BlockedUsers = oldUser.BlockedUsers.Clone()
                };

                if (this._users.TryUpdate(username, updatedUser, oldUser))
                    return true;
            }

            return false;
        }

        public bool AddBlockedUser(string currentUser, string userToBlock)
        {
            while (this._users.TryGetValue(currentUser, out var oldUser))
            {
                if (oldUser.BlockedUsers.Contains(userToBlock))
                    return true; 

                var newBlocked = oldUser.BlockedUsers.Clone();
                newBlocked.Add(userToBlock);

                var updated = new User(oldUser.Username, oldUser.City, oldUser.Age, oldUser.PhoneNumber, oldUser.IsBusy)
                {
                    BlockedUsers = newBlocked
                };

                if (this._users.TryUpdate(currentUser, updated, oldUser))
                    return true;
            }
            return false;
        }

        public User? GetUser(string username)
        {
            if(!this._users.ContainsKey(username)) return null;

            this._users.TryGetValue(username, out User user);
            return user;
        }

        public List<User> GetAllUsers()
        {
            return this._users.Values.ToList();
        }
    }
}
