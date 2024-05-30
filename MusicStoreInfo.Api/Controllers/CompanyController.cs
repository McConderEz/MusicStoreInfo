using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.CityService;
using MusicStoreInfo.Services.Services.CompanySerivce;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _service;
        private readonly MusicStoreDbContext _dbContext;

        public CompanyController(ICompanyService companyService, MusicStoreDbContext dbContext)
        {
            _service = companyService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index(int page = 1,List<int> cityIds = null, List<int> districtIds = null,
                                   string sortOrder = null, string searchString = null)
        {
            var companies = _service.GetAllAsync().Result;


            companies = Filter(cityIds, districtIds, companies);

            if (!string.IsNullOrEmpty(searchString))
            {
                companies = companies.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                            || p.PhoneNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            companies = Sort(sortOrder, companies);

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = companies.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = companies.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;

            var companyViewModel = new CompanyViewModel
            {
                Companies = data,
                Cities = _dbContext.Cities.Include(c => c.Districts).ToList()
            };

            return View(companyViewModel);
        }

        private static List<Company>? Sort(string sortOrder, List<Company>? companies)
        {
            switch (sortOrder)
            {
                case "name_asc":
                    companies = companies.OrderBy(p => p.Name).ToList();
                    break;
                default:
                    break;
            }

            return companies;
        }

        //TODO: Перенести в DAL и упростить
        private static List<Company>? Filter(List<int> cityIds, List<int> districtIds, List<Company> companies)
        {
            if (cityIds != null && cityIds.Any())
                companies = companies.Where(a => cityIds.Contains(a.District.CityId)).ToList();
            if (districtIds != null && districtIds.Any())
                companies = companies.Where(a => districtIds.Contains(a.DistrictId)).ToList();
            return companies;
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public IActionResult Create()
        {
            ViewBag.Districts = new SelectList(_dbContext.Districts, "Id","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Company model)
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
            ViewBag.Districts = new SelectList(_dbContext.Districts, "Id", "Name");
            var model = await _service.GetByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Company model)
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
            var company = await _service.DetailsAsync(id);

            return View(company);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
