using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index()
        {
            return View(_service.GetAllAsync().Result);
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
