using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.AlbumService;
using MusicStoreInfo.Services.Services.CityService;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class CityController : Controller
    {
        private readonly ICityService _service;
        private readonly MusicStoreDbContext _dbContext;


        public CityController(ICityService cityService, MusicStoreDbContext dbContext)
        {
            _service = cityService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string searchString = null)
        {
            var cities = await _service.GetAllAsync();
            const int pageSize = 10;
            if (page < 1)
                page = 1;

            if (!string.IsNullOrEmpty(searchString))
            {
                cities = cities.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int recsCount = cities.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = cities.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;

            return View(data);
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(City model)
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
            
            var model = await _service.GetByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Edit(City model)
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
            var city = await _service.DetailsAsync(id);

            return View(city);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
