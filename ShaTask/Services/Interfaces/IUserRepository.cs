using ShaTask.Core.Models;

namespace ShaTask.Services.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        User GetUserByUsername(string username);
        void AddUser(User user);
    }
}
