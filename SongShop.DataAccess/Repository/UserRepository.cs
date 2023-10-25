using SongShop.DataAccess.Data;
using SongShop.DataAccess.Repository.IRepository;
using SongShop.Models;
using SongShop.Models.ViewModels;

namespace SongShop.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddUser(User user)
        {
			user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
			_db.Users.Add(user);
            _db.SaveChanges();
        }

        public bool CheckEmail(string email)
        {
            if (_db.Users.FirstOrDefault(x => x.Email == email) == null)
                return false;
            else
                return true;
        }

        public User Get(string email)
        {
            return _db.Users.FirstOrDefault(x => x.Email == email);
        }

        public User Login(UserLoginVM user)
        {
            User userFromDb = _db.Users.FirstOrDefault(x => x.Email == user.Email);

            if (userFromDb == null)
                return null;

            if (BCrypt.Net.BCrypt.Verify(user.Password, userFromDb.Password))
                return userFromDb;
            return null;
        }

        public void UpdateUser(User user)
        {
            User userFromDb = _db.Users.FirstOrDefault(x => x.Email == user.Email);

            userFromDb.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            userFromDb.Name = user.Name;
            userFromDb.Surname = user.Surname;
            userFromDb.Address = user.Address;
            userFromDb.Date = user.Date;
            userFromDb.UserType = user.UserType;

            _db.SaveChanges();
        }
    }
}
