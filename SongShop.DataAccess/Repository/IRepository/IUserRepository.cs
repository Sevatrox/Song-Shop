using SongShop.Models;
using SongShop.Models.ViewModels;

namespace SongShop.DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        void AddUser(User user);
        void UpdateUser(User user);
        bool CheckEmail(string email);
        User Login(UserLoginVM user);
        User Get(string email);
    }
}
