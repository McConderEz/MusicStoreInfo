using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.Services.Services;
using System.Security.Claims;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        public readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
        
        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var token = await _accountService.Login(model.UserName, model.Password);

            var claims = new List<Claim>
            {
                new Claim("Demo","Value"),
                new Claim(ClaimTypes.Name,model.UserName),

            };
            var claimIdentity = new ClaimsIdentity(claims, "Cookie");
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("Cookie", claimPrincipal);
            HttpContext.Response.Cookies.Append("$data-cookies", token);

            return Redirect(model.ReturnUrl);
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
