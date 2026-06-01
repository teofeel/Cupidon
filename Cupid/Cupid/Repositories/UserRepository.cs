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

            if (this._users.TryGetValue(user.Username, out var oldUser))
                return this._users.TryUpdate(user.Username, user, oldUser);

            return false;
        }

        public bool RemoveUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;

            if (!this._users.ContainsKey(username)) return false;

            return this._users.TryRemove(username, out _);
        }

        public User? GetUser(string username)
        {
            if (!this._users.ContainsKey(username)) return null;

            this._users.TryGetValue(username, out User user);
            return user;
        }

        public List<User> GetAllUsers()
        {
            return this._users.Values.ToList();
        }
    }
}