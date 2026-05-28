using Zad2.Models;

namespace Zad2.DataRepositories.Interfaces
{
    public interface IUserRepository
    {
        bool AddUser(User user);
        bool UpdateUser(User user);
        bool RemoveUser(string user);

        User? GetUser(string username);
        List<User> GetAllUsers();
    }
}
