using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services;
using MusicStoreInfo.Services.Services.ProductService;
using MusicStoreInfo.Services.Services.ReviewService;

namespace MusicStoreInfo.Api.Controllers
{
    public class ReviewController: Controller
    {
        private readonly IReviewService _service;
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;

        public ReviewController(IReviewService reviewService, IProductService productService, IAccountService accountService)
        {
            _service = reviewService;
            _productService = productService;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(int productId, string userName, int rating, string comment, DateTime timeCreated, int albumId, int storeId)
        {
            var user = await _accountService.GetUserByNameAsync(userName);
            var review = new Review
            {
                ProductId = productId,
                AlbumId = albumId,
                StoreId = storeId,
                UserId =  user.Id,
                Rating = rating,
                Comment = comment,
                TimeCreated = timeCreated
            };

            await _service.Add(review);
            await _productService.AddReviewAsync(review, productId);
            return RedirectToAction("Details", "Product", new { id = productId });
        }
    }
}
