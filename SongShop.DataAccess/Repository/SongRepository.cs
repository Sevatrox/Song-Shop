using Microsoft.EntityFrameworkCore;
using SongShop.DataAccess.Data;
using SongShop.DataAccess.Repository.IRepository;
using SongShop.Models;

namespace SongShop.DataAccess.Repository
{
    public class SongRepository : ISongRepository
    {

        private readonly ApplicationDbContext _db;

        public SongRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void AddSong(Song song)
        {
            _db.Songs.Add(song);
            _db.SaveChanges();
        }

		public void BuySong(int songId, int userId)
		{
            Song songFromDb = GetSong(songId);
            songFromDb.UserId = userId;
            _db.SaveChanges();
		}

		public string DeleteSong(int? id)
        {
            Song songFromDb = _db.Songs.FirstOrDefault(x => x.Id == id);
            string imageUrl = "";

            if (songFromDb != null)
            {
                imageUrl = songFromDb.ImageUrl;
                _db.Songs.Remove(songFromDb);
                _db.SaveChanges();
                return imageUrl;
            }

            return imageUrl;
        }

        public List<Song> GetAll(int userId)
        {
            return _db.Songs.Where(x=> x.UserId == userId).ToList();
        }

        public List<Song> GetAllSellerSongs()
        {
            return _db.Songs.Include(x => x.User).Where(x => x.User.UserType == "Seller").ToList();
        }

        public Song GetSong(int? id)
        {
            return _db.Songs.FirstOrDefault(x => x.Id == id);
        }


        public void UpdateSong(Song song)
        {
            Song songFromDb = GetSong(song.Id);
            songFromDb.Author = song.Author;
            songFromDb.Title = song.Title;
            songFromDb.Description = song.Description;
            songFromDb.Price = song.Price;
            songFromDb.Type = song.Type;
            if (song.ImageUrl != null)
                songFromDb.ImageUrl = song.ImageUrl;

            _db.SaveChanges();
        }
    }
}
