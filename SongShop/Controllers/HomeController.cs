using Microsoft.AspNetCore.Mvc;
using SongShop.Models;
using System.Diagnostics;
using SongShop.DataAccess.Repository.IRepository;
using DemoAudit.Filters;

namespace SongShop.Controllers
{
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISongRepository _songRepository;

        public HomeController(ILogger<HomeController> logger, ISongRepository songRepository)
        {
            _logger = logger;
            _songRepository = songRepository;
        }

        public IActionResult Index()
        {
			_logger.LogInformation("Index method of HomeController started!");
			List<Song> songsFromDb = _songRepository.GetAllSellerSongs();

			_logger.LogInformation("Index method of HomeController ended!");
			return View(songsFromDb);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}