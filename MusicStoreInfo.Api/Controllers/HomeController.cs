using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.Services.Services.ProductService;
using System.Diagnostics;

namespace MusicStoreInfo.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _staticFilesPath;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _productService = productService;
            _hostEnvironment = hostEnvironment;
            _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), _hostEnvironment.WebRootPath + "\\Image");
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllAsync().Result;
            return View(products);
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

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
