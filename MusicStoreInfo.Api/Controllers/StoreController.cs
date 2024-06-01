using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.StoreService;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Index(int page = 1, DateTime? minDate = null, DateTime? maxDate = null, List<int> ownershipTypeIds = null,
                           List<int> cityIds = null, List<int> districtIds = null, string sortOrder = null, string searchString = null)
        {
            var stores = _service.GetAllAsync().Result;

            stores = Filter(minDate, maxDate, ownershipTypeIds, cityIds, districtIds, stores);

            if (!string.IsNullOrEmpty(searchString))
            {
                stores = stores.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            stores = Sort(sortOrder, stores);

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = stores.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = stores.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;

            ViewBag.MinDate = minDate;
            ViewBag.MaxDate = maxDate;
            ViewBag.OwnershipTypeIds = ownershipTypeIds ?? new List<int>();
            ViewBag.CityIds = cityIds ?? new List<int>();
            ViewBag.DistrictIds = districtIds ?? new List<int>();
            ViewBag.SortOrder = sortOrder;

            var storeViewModel = new StoreViewModel
            {
                Stores = data,
                OwnershipTypes = _dbContext.OwnershipTypes.ToList(),
                Cities = _dbContext.Cities.Include(c => c.Districts).ToList()
            };

            return View(storeViewModel);
        }

        private static List<Store>? Sort(string sortOrder, List<Store>? stores)
        {
            switch (sortOrder)
            {
                case "date_asc":
                    stores = stores.OrderBy(p => p.YearOpened.Date).ToList();
                    break;
                case "date_desc":
                    stores = stores.OrderByDescending(p => p.YearOpened.Date).ToList();
                    break;
                default:
                    break;
            }

            return stores;
        }

        //TODO: Перенести в DAL и упростить
        private static List<Store>? Filter(DateTime? minDate, DateTime? maxDate, List<int> ownershipTypeIds, List<int> cityIds, List<int> districtIds, List<Store> stores)
        {
            if (minDate.HasValue)
                stores = stores.Where(a => a.YearOpened.Date >= minDate.Value).ToList();
            if (maxDate.HasValue)
                stores = stores.Where(a => a.YearOpened.Date <= maxDate.Value).ToList();
            if (ownershipTypeIds != null && ownershipTypeIds.Any())
                stores = stores.Where(a => ownershipTypeIds.Contains(a.OwnershipTypeId)).ToList();
            if (cityIds != null && cityIds.Any())
                stores = stores.Where(a => cityIds.Contains(a.District.CityId)).ToList();
            if (districtIds != null && districtIds.Any())
                stores = stores.Where(a => districtIds.Contains(a.DistrictId)).ToList();
            return stores;
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
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
        [Authorize(Policy = "Manager")]
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

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
