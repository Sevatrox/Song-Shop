using DemoAudit.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SongShop.DataAccess.Repository.IRepository;
using SongShop.Models;
using SongShop.Models.ViewModels;

namespace SongShop.Controllers
{
	[ServiceFilter(typeof(AuditFilterAttribute))]
	public class SongController : Controller
    {
        private readonly ISongRepository _songRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly ILogger<SongController> _logger;
		public readonly List<string> listGenres = new List<string>() { "Pop", "Hip hop", "Rock", "Funk", "Country", "Jazz", "Disco", "Classical", "Metal" };

		public SongController(ISongRepository songRepository, IUserRepository userRepository, IWebHostEnvironment webHostEnvironment, ILogger<SongController> logger)
        {
            _songRepository = songRepository;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }


        [Authorize(Roles = "Seller")]
        public IActionResult Add()
        {
			_logger.LogInformation("Add method of SongController started!");

			ViewBag.Genres = listGenres;
			_logger.LogInformation("Add method of SongController ended!");
			return View();
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        public IActionResult Add(SongVM songObj, IFormFile? file)
        {
			_logger.LogInformation("Add POST method of SongController started!");

			if (file != null)
            {
                songObj.Song.ImageUrl = @"\images\" + GetFileName(songObj.Song, file);
            }

            string userEmail = User.Claims.First().Value;
            User userFromDb = _userRepository.Get(userEmail);
            songObj.Song.UserId = userFromDb.Id;

            songObj.Song.Type = string.Join(", ", songObj.Genres);

            _songRepository.AddSong(songObj.Song);

            TempData["success"] = "Song added!";
			_logger.LogInformation("Add POST method of SongController ended!");
			return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Seller,Buyer")]
        public IActionResult List()
        {
			_logger.LogInformation("List method of SongController started!");

			string userEmail = User.Claims.First().Value;
            User userFromDb = _userRepository.Get(userEmail);

            List<Song> songsFromDb = _songRepository.GetAll(userFromDb.Id);
			_logger.LogInformation("List method of SongController ended!");
			return View(songsFromDb);
        }

        [Authorize(Roles = "Seller")]
        public IActionResult Delete(int? id)
        {
			_logger.LogInformation("Delete method of SongController started!");

			if (id == 0 || id == null)
            {
                return NotFound();
            }

            string imageUrl = "";
            imageUrl = _songRepository.DeleteSong(id);
            if (imageUrl == "")
            {
                TempData["error"] = "Song with id " + id + " does not exist!";
                return RedirectToAction("List", "Song");
            }

            DeleteImage(imageUrl);

            TempData["success"] = "Song deleted!";
			_logger.LogInformation("Delete method of SongController ended!");
			return RedirectToAction("List", "Song");
        }

        [Authorize(Roles = "Seller")]
        public IActionResult Edit(int id)
        {
			_logger.LogInformation("Edit method of SongController started!");

			SongVM songFromDb = new SongVM();
            songFromDb.Song = _songRepository.GetSong(id);

            if(songFromDb == null)
            {
				TempData["error"] = "Song with id " + id + " does not exist!";
				return RedirectToAction("List", "Song");
			}

            ViewBag.Genres = listGenres;
			_logger.LogInformation("Edit method of SongController ended!");
			return View(songFromDb);
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        public IActionResult Edit(SongVM songObj, IFormFile? file)
        {
			_logger.LogInformation("Edit POST method of SongController started!");

			if (file != null)
            {
                songObj.Song.ImageUrl = @"\images\" + GetFileName(songObj.Song, file);
            }

            songObj.Song.Type = string.Join(", ", songObj.Genres);

            _songRepository.UpdateSong(songObj.Song);

            TempData["success"] = "Song updated!";
			_logger.LogInformation("Edit POST method of SongController ended!");
			return RedirectToAction("List", "Song");
        }

		[Authorize(Roles = "Buyer")]
		public IActionResult BuySong(int id)
		{
			_logger.LogInformation("BuySong method of SongController started!");

			string userEmail = User.Claims.First().Value;
			User userFromDb = _userRepository.Get(userEmail);

			_songRepository.BuySong(id, userFromDb.Id);

			TempData["success"] = "Song bought!";
			_logger.LogInformation("BuySong method of SongController ended!");
			return RedirectToAction("List", "Song");
		}

		public IActionResult Details(int id)
		{
			_logger.LogInformation("Details method of SongController started!");

            Song songFromDb = _songRepository.GetSong(id);

			_logger.LogInformation("Details method of SongController ended!");
			return View(songFromDb);
		}

        public PartialViewResult SearchSongs(string searchText)
        {
            _logger.LogInformation("SearchSongs method of SongController started!");

            List<Song> songsFromDb = _songRepository.GetAllSellerSongs();
            var result = songsFromDb;

            if (searchText != null)
            {
                if (int.TryParse(searchText, out int searchNumber))
                {
                    result = songsFromDb.Where(x => x.Price.Equals(searchNumber)).ToList();
                }
                else
                {
                    result = songsFromDb.Where(x => x.Title.ToLower().Contains(searchText.ToLower()) ||
                                x.Author.ToLower().Contains(searchText.ToLower()) ||
                                x.Description.ToLower().Contains(searchText.ToLower()) ||
                                x.Type.ToLower().Contains(searchText.ToLower())).ToList();
                }
            }

            _logger.LogInformation("SearchSongs method of SongController ended!");
            return PartialView("_SongsView", result);
        }

        public void DeleteImage(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

            }
        }

        public string GetFileName(Song songObj, IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"images");

            if (!string.IsNullOrEmpty(songObj.ImageUrl))
            {
                var oldImagePath = Path.Combine(wwwRootPath, songObj.ImageUrl.TrimStart('\\'));

                if(System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }
    }
}
