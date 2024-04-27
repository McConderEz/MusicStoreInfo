using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.StoreService;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly IStoreService _service;
        private readonly MusicStoreDbContext _dbContext;

        public StoreController(IStoreService storeService, MusicStoreDbContext dbContext)
        {
            _service = storeService;
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
            ViewBag.OwnershipTypes = new SelectList(_dbContext.OwnershipTypes, "Id", "Name");
            ViewBag.Districts = new SelectList(_dbContext.Districts, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Store model)
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
            ViewBag.OwnershipTypes = new SelectList(_dbContext.OwnershipTypes, "Id", "Name");
            ViewBag.Districts = new SelectList(_dbContext.Districts, "Id", "Name");
            var model = await _service.GetByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Store model)
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
            var store = await _service.DetailsAsync(id);

            return View(store);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
