using Cupid.Models;

namespace Cupid.Services.Interfaces
{
    public interface IUserService
    {
        bool RegisterUser(string username, string city, int age, string phoneNumber);
        void BlockUser(string current, string username);
        void SetUserBusy(string username, bool isBusy);

        List<User> GetAllUser();
        User? GetUserByUsername(string username);
    }
}
