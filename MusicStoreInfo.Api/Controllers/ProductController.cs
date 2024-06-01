using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.ProductService;
using MusicStoreInfo.Services.Services.ShoppingCartService;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly MusicStoreDbContext _dbContext;

        public ProductController(IProductService productService, MusicStoreDbContext dbContext, IShoppingCartService shoppingCartService)
        {
            _service = productService;
            _dbContext = dbContext;
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, int? minPrice = null, int? maxPrice = null,
                                   int? minQuantity = null, int? maxQuantity = null,
                                   List<int> storeIds = null, List<int> groupIds = null, List<int> genreIds = null, string sortOrder = null, string searchString = null)
        {
            var products = _service.GetAllAsync().Result;

            products = Filter(minPrice, maxPrice, minQuantity, maxQuantity, storeIds, groupIds, genreIds, products);

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Album.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                                        || p.Album.Group.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                        || p.Store.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            products = Sort(sortOrder, products);

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = products.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = products.Skip(recSkip).Take(pageSize).ToList();

            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;

            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.MinQuantity = minQuantity;
            ViewBag.MaxQuantity = maxQuantity;
            ViewBag.StoreIds = storeIds ?? new List<int>();
            ViewBag.GroupIds = groupIds ?? new List<int>();
            ViewBag.GenreIds = genreIds ?? new List<int>();
            ViewBag.SortOrder = sortOrder;

            var productViewModel = new ProductViewModel
            {
                Products = data,
                Stores = _dbContext.Stores.ToList(),
                Groups = _dbContext.Groups.ToList(),
                Genres = _dbContext.Genres.ToList()
            };

            return View(productViewModel);
        }

        //TODO: Перенести в DAL
        private static List<Product>? Sort(string sortOrder, List<Product>? products)
        {
            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "quantity":
                    products = products.OrderBy(p => p.Quantity).ToList();
                    break;
                default:
                    break;
            }

            return products;
        }

        //TODO: Перенести в DAL и упростить
        private static List<Product>? Filter(int? minPrice, int? maxPrice, int? minQuantity, int? maxQuantity, List<int> storeIds, List<int> groupIds, List<int> genreIds, List<Product>? products)
        {
            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();
            if (minQuantity.HasValue)
                products = products.Where(p => p.Quantity >= minQuantity.Value).ToList();
            if (maxQuantity.HasValue)
                products = products.Where(p => p.Quantity <= maxQuantity.Value).ToList();
            if (storeIds != null && storeIds.Any())
                products = products.Where(p => storeIds.Contains(p.StoreId)).ToList();
            if (groupIds != null && groupIds.Any())
                products = products.Where(p => groupIds.Contains(p.Album.GroupId)).ToList();
            if (genreIds != null && genreIds.Any())
                products = products.Where(p => p.Album.Group.Genres.Any(g => genreIds.Contains(g.Id))).ToList();
            return products;
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

        [Authorize(Policy = "Client")]
        public async Task<IActionResult> AddInShoppingCart(int storeId, int albumId, int quantity)
        {
            await _shoppingCartService.AddProductAsync(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "ShoppingCartId")?.Value!), storeId,albumId, quantity);

            return Json(new { success = true });
        }
    }
}
