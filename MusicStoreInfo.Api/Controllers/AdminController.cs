using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly MusicStoreDbContext _dbContext;

        public AdminController(IAccountService accountService, MusicStoreDbContext dbContext)
        {
            _accountService = accountService;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(_accountService.GetAsync().Result);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(int id)
        {
            ViewBag.Roles = new SelectList(_dbContext.Roles, "Id", "Name");
            var user = await _accountService.GetUserByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(User model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new User
            {
                Id = model.Id,
                UserName = model.UserName,
                PasswordHash = model.PasswordHash,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                ImagePath = model.ImagePath,
                RoleId = model.RoleId
            };

            await _accountService.EditAsync(user.Id, user);
            return Redirect("/Admin/Index");
        }
    }
}
