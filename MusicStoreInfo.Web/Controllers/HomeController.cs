using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Web.Models;
using System.Diagnostics;

namespace MusicStoreInfo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MusicStoreDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, MusicStoreDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
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
