using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.GenreService;

namespace MusicStoreInfo.Api.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _service;
        private readonly MusicStoreDbContext _dbContext;

        public GenreController(IGenreService genreService, MusicStoreDbContext dbContext)
        {
            _service = genreService;
            _dbContext = dbContext;
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
        public async Task<IActionResult> Create(Genre model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.CreateAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _service.GetByIdAsync(id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Genre model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _service.EditAsync(model.Id, model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var genre = await _service.DetailsAsync(id);

            var genreViewModel = new GenreViewModel
            {
                Genre = genre,
                Groups = _dbContext.Groups.ToList(),
            };

            return View(genreViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(int genreId, int groupId)
        {
            await _service.AddGroupAsync(genreId, groupId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup(int genreId, int groupId)
        {
            await _service.DeleteGroupAsync(genreId, groupId);
            return Json(new { success = true });
        }
    }
}
