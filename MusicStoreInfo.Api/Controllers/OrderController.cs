using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.OrderService;
using MusicStoreInfo.Services.Services.ProductService;
using MusicStoreInfo.Services.Services.ShoppingCartService;

namespace MusicStoreInfo.Api.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _service;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly MusicStoreDbContext _dbContext;

        public OrderController(IOrderService orderService, IProductService productService, MusicStoreDbContext dbContext, IShoppingCartService shoppingCartService)
        {
            _service = orderService;
            _productService = productService;
            _dbContext = dbContext;
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Payment(int storeId, int albumId, string userName, int quantity)
        {
            var product = await _dbContext.Products
                .Include(p => p.Album)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(p => p.StoreId == storeId && p.AlbumId == albumId);
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user != null && product != null)
            {
                var order = new Order
                {
                    AlbumId = albumId,
                    Product = product,
                    ProductId = product.Id,
                    StoreId = storeId,
                    UserId = user.Id,
                    OrderDate = DateTime.Now,
                    ExpectedArrivalDate = DateTime.Now.AddDays(7),
                    IsDelivered = false,
                    Quantity = quantity
                };

                return View(order);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Payment(Order order)
        {
            if (ModelState.IsValid)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.StoreId == order.StoreId && p.AlbumId == order.AlbumId);
                int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "ShoppingCartId")?.Value!);
                if (product != null)
                {
                    product.Quantity -= order.Quantity;
                    await _service.AddAsync(order);
                    await _productService.EditAsync(product.Id, product);
                    await _shoppingCartService.DeleteProductAsync(id, product.AlbumId, product.StoreId);
                    _dbContext.SaveChanges();
                }

                return RedirectToAction("Index", "ShoppingCart", new {id = id});
            }

            return View(order);
        }
    }
}
