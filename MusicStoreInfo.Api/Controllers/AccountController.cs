using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Contracts;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services;
using MusicStoreInfo.Services.Services.ImageService;
using System.Security.Claims;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly IAccountService _accountService;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _staticFilesPath;

        public AccountController(IAccountService accountService, IImageService imageService,
            IWebHostEnvironment hostEnvironment)
        {
            _accountService = accountService;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), _hostEnvironment.WebRootPath + "\\Image");
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _accountService.GetUserByIdAsync(id);
            return View(model);
        }


        //TODO:Сделать ограничения на картинки

        [HttpPost]
        public async Task<IActionResult> Edit(UserDto model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var image = await _imageService.CreateImageAsync(model.ImagePath, _staticFilesPath);

            var user = new User
            {
                Id = model.Id,
                UserName = model.UserName,
                PasswordHash = model.PasswordHash,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                ImagePath = image,
                RoleId = model.RoleId
            };

            await _accountService.EditAsync(user.Id, user);
            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> Profile()
        {

            var user = await _accountService.GetUserByNameAsync(User.Identity!.Name!);
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            (string token, User user) = await _accountService.Login(model.UserName, model.Password);
            

            var claims = new List<Claim>
            {
                new Claim("Demo","Value"),
                new Claim(ClaimTypes.Name,model.UserName),
                new Claim(ClaimTypes.Role, user.Role!.Name!),
                new Claim("ShoppingCartId", user.ShoppingCart.Id.ToString())
            };

            if (user.ImagePath != null)
                claims.Add(new Claim("ImagePath", user.ImagePath));

            var claimIdentity = new ClaimsIdentity(claims, "Cookie");
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("Cookie", claimPrincipal);
            HttpContext.Response.Cookies.Append("$data-cookies", token);

            return Redirect("/home/index");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _accountService.Register(model.UserName, model.Password);
            return Redirect(model.ReturnUrl);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("Cookie");
            return Redirect("/home/index");
        }
    }
}
