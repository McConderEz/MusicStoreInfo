using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.ProductService;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly MusicStoreDbContext _dbContext;

        public ProductController(IProductService productService, MusicStoreDbContext dbContext)
        {
            _service = productService;
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
            ViewBag.Stores = new SelectList(_dbContext.Stores, "Id", "Name");
            ViewBag.Albums = new SelectList(_dbContext.Albums, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Create(Product model)
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
        public IActionResult Edit()
        {
            ViewBag.Albums = new SelectList(_dbContext.Albums, "Id", "Name");
            ViewBag.Stores = new SelectList(_dbContext.Stores, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _service.EditAsync(id, model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _service.DetailsAsync(id);

            return View(product);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
