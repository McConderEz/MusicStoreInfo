﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.ShoppingCartService;


namespace MusicStoreInfo.Api.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var shoppingCart = await _shoppingCartService.GetByIdAsync(id);

            //shoppingCart.Products = shoppingCart.Products is null ? new List<Product>() : shoppingCart.Products;

            return View(shoppingCart);
        }

        public IActionResult PayForTheProduct(int storeId, int albumId)
        {
            //Сделать когда-нибудь потом
            return View();
        }

        public async Task<IActionResult> DeleteFromShoppingCart(int storeId,int albumId)
        {
            int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "ShoppingCartId")?.Value!);
            await _shoppingCartService.DeleteProductAsync(id, storeId, albumId);
            return RedirectToAction("Index", new { id = id });
        }
    }
}