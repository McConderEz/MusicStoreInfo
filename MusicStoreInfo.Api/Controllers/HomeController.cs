using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Services.Services;
using MusicStoreInfo.Services.Services.ProductService;
using System.Diagnostics;

namespace MusicStoreInfo.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;
        private readonly string _staticFilesPath;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IWebHostEnvironment hostEnvironment,
            IAccountService accountService)
        {
            _logger = logger;
            _productService = productService;
            _hostEnvironment = hostEnvironment;
            _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), _hostEnvironment.WebRootPath + "\\Image");
            _accountService = accountService;
            //AddAdminIfNotExist();
        }

        public IActionResult Index()
        {           
            var products = _productService.GetAllAsync().Result;
            return View(products);
        }

        //private void AddAdminIfNotExist()
        //{
        //    using (var db = new MusicStoreDbContext())
        //    {
        //        var admin = db.Users.SingleOrDefault(u => u.UserName.Equals("Root") && u.Role.Name.Equals("Root"));
        //        if (admin == null)
        //        {
        //            _accountService.Register("Root", "Root");
        //        }
        //    }
        //}

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
