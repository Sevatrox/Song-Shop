using SongShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongShop.DataAccess.Repository.IRepository
{
    public interface ISongRepository
    {
        void AddSong(Song song);
        void UpdateSong(Song song);
        string DeleteSong(int? id);
        Song GetSong(int? id);
        List<Song> GetAll(int userId);
        List<Song> GetAllSellerSongs();
        void BuySong(int songId, int userId);


	}
}
