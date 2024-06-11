using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.OrderService;
using MusicStoreInfo.Services.Services.ProductService;
using MusicStoreInfo.Services.Services.ShoppingCartService;
using System.Text.Json;
using System.Transactions;

namespace MusicStoreInfo.Api.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _service;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly MusicStoreDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public OrderController(IOrderService orderService, IProductService productService, MusicStoreDbContext dbContext, IShoppingCartService shoppingCartService, IServiceProvider serviceProvider)
        {
            _service = orderService;
            _productService = productService;
            _dbContext = dbContext;
            _shoppingCartService = shoppingCartService;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            var ordersAll = await _service.GetAsync();
            return View(ordersAll);
        }


        [HttpGet]
        public IActionResult RevenueReport()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RevenueReport(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.AddDays(-30);
            }
            if (!endDate.HasValue)
            {
                endDate = DateTime.Now;
            }

            var user = await _dbContext.Users.Include(u => u.Store)
                                             .FirstOrDefaultAsync(u => u.UserName.Equals(User.Identity.Name));

            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var orders = await _service.GetAsync();

            var filteredOrders = orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.StoreId == user.StoreId)
                                       .ToList();

            var revenueData = filteredOrders
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Revenue = g.Sum(o => o.Quantity * o.Product.Price)
                })
                .OrderBy(r => r.Date)
                .ToList();

            if (!revenueData.Any())
            {
                return Json(new { success = false, message = "No data found" });
            }

            return View("RevenueReportResult", revenueData);
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
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedDbContext = scope.ServiceProvider.GetRequiredService<MusicStoreDbContext>();
                    var scopedOrderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    var scopedProductService = scope.ServiceProvider.GetRequiredService<IProductService>();
                    var scopedCartService = scope.ServiceProvider.GetRequiredService<IShoppingCartService>();

                    using (var transaction = await scopedDbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var product = await scopedDbContext.Products.FirstOrDefaultAsync(p => p.StoreId == order.StoreId && p.AlbumId == order.AlbumId);
                            int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "ShoppingCartId")?.Value!);
                            if (product != null)
                            {
                                product.Quantity -= order.Quantity;
                                await scopedDbContext.SaveChangesAsync(); // Сохранение изменений в продукте

                                await scopedOrderService.AddAsync(order);
                                await scopedDbContext.SaveChangesAsync(); // Сохранение нового заказа

                                // Редактирование продукта
                                scopedDbContext.Products.Attach(product);
                                await scopedProductService.EditAsync(product.Id, product);
                                await scopedDbContext.SaveChangesAsync(); // Сохранение изменений в продукте через сервис

                                // Удаление продукта из корзины
                                await scopedCartService.DeleteProductAsync(id, product.AlbumId, product.StoreId);
                                await scopedDbContext.SaveChangesAsync(); // Сохранение изменений в корзине
                            }

                            await transaction.CommitAsync();
                            return RedirectToAction("Index", "ShoppingCart", new { id = id });
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }

            return View(order);
        }
    }
}
