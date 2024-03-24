using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.SongService;

namespace MusicStoreInfo.Api.Controllers
{
    public class SongController : Controller
    {
        private readonly ISongService _service;


        public SongController(ISongService songService)
        {
            _service = songService;
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
        public async Task<IActionResult> Create(Song model)
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
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Song model)
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
            var song = await _service.DetailsAsync(id);

            return View(song);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
