using Azure;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Models;
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
        public async Task<IActionResult> Index(int page = 1, List<int> cityIds = null, string searchString = null)
        {
            var districts = await _service.GetAllAsync();

            districts = Filter(cityIds, districts);

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            if (!string.IsNullOrEmpty(searchString))
            {
                districts = districts.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int recsCount = districts.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = districts.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;
            ViewBag.CityIds = cityIds ?? new List<int>();

            var cityDistrictViewModel = new CityDistrictViewModel
            {
                Cities = _dbContext.Cities.Include(c => c.Districts).ToList(),
                Districts = data
            };

            return View(cityDistrictViewModel);
        }

        private static List<District>? Filter(List<int> cityIds, List<District> districts)
        {
            if (cityIds != null && cityIds.Any())
                districts = districts.Where(a => cityIds.Contains(a.CityId)).ToList();
            return districts;
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
