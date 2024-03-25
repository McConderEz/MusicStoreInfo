using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.Api.Contracts;
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
        private readonly string _staticFilesPath;


        public GroupController(IGroupService groupService, IImageService imageService, IWebHostEnvironment hostEnvironment)
        {
            _service = groupService;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _service.GetByIdAsync(id);
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

            return View(group);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
