using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.CompanySerivce;
using MusicStoreInfo.Services.Services.DistrictService;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class DistrictController : Controller
    {
        private readonly IDistrictService _service;
        private readonly MusicStoreDbContext _dbContext;


        public DistrictController(IDistrictService districtService, MusicStoreDbContext dbContext)
        {
            _service = districtService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_service.GetAllAsync().Result);
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public IActionResult Create()
        {
            ViewBag.Cities = new SelectList(_dbContext.Cities, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(District model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.CreateAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Cities = new SelectList(_dbContext.Cities, "Id", "Name");
            var model = await _service.GetByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(District model)
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
            var district = await _service.DetailsAsync(id);

            return View(district);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
