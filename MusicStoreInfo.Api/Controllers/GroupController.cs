using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Contracts;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.GroupService;
using MusicStoreInfo.Services.Services.ImageService;

namespace MusicStoreInfo.Api.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _service;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly MusicStoreDbContext _dbContext;
        private readonly string _staticFilesPath;


        public GroupController(IGroupService groupService, IImageService imageService, IWebHostEnvironment hostEnvironment, MusicStoreDbContext dbContext)
        {
            _service = groupService;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _dbContext = dbContext; 
            _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), _hostEnvironment.WebRootPath + "\\Image");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_service.GetAllAsync().Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupDto model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var image = await _imageService.CreateImageAsync(model.ImagePath, _staticFilesPath);

            var group = new Group
            {
                Name = model.Name,
                ImagePath = image
            };

            await _service.CreateAsync(group);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _service.GetByIdAsync(id).Result;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupDto model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var image = await _imageService.CreateImageAsync(model.ImagePath, _staticFilesPath);

            var group = new Group
            {
                Id = model.Id,
                Name = model.Name,
                ImagePath = image,
            };

            await _service.EditAsync(group.Id, group);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var group = await _service.DetailsAsync(id);

            var groupViewModel = new GroupViewModel
            {
                Group = group,
                Members = _dbContext.Members.Include(m => m.Specializations)
                                            .Include(m => m.Gender).ToList(),
                Genres = _dbContext.Genres.ToList(),
            };
            return View(groupViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre(int groupId, int genreId)
        {
            await _service.AddGenre(groupId, genreId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGenre(int groupId, int genreId)
        {
            await _service.DeleteGenreAsync(groupId, genreId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(int groupId, int memberId)
        {
            await _service.AddMemberAsync(groupId, memberId);
            return Json(new { success = true });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteMember(int groupId, int memberId)
        {
            await _service.DeleteMemberAsync(groupId, memberId);
            return Json(new { success = true });
        }
    }
}
