using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

        [HttpGet]
        public async Task<IActionResult> EditUserStore(int id)
        {
            ViewBag.Stores = new SelectList(_dbContext.Stores, "Id", "Name");
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

            var oldUser = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == model.Id);
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == model.RoleId);

            string removeFromRoleSql = $@"
                         ALTER ROLE [{oldUser.Role.Name}] DROP MEMBER [{model.UserName}];
                                                                            ";

            string assignToRoleSql = $@"
                        ALTER ROLE [{role.Name}] ADD MEMBER [{model.UserName}];
                                                                           ";


            using (SqlConnection connection = new SqlConnection(MusicStoreDbContext.CONNECTION_STRING))
            {
                try
                {

                    connection.Open();
                    using (SqlCommand command = new SqlCommand(removeFromRoleSql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"User '{model.UserName}' removed from role '{role.Name}' successfully.");
                    }

                    using (SqlCommand command = new SqlCommand(assignToRoleSql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"User '{model.UserName}' assigned to role '{role.Name}' successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            var user = new User
            {
                Id = model.Id,
                UserName = model.UserName,
                PasswordHash = model.PasswordHash,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                ImagePath = model.ImagePath,
                RoleId = model.RoleId,
                StoreId = model.StoreId
            };

            await _accountService.EditAsync(user.Id, user);
            return Redirect("/Admin/Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditUserStore(User model)
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
                RoleId = model.RoleId,
                StoreId = model.StoreId
            };

            await _accountService.EditAsync(user.Id, user);
            return Redirect("/Admin/Index");
        }

        public async Task<IActionResult> BlockUser(int id)
        {
            var user = await _accountService.GetUserByIdAsync(id);

            if (user != null)
            {
                user.IsBlocked = !user.IsBlocked;
                await _accountService.EditAsync(id, user);
                return Json(new { success = true, isBlocked = user.IsBlocked });
            }

            return Json(new { success = false, message = "Пользователь не найден." });
        }
    }
}
