using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.AlbumService;

namespace MusicStoreInfo.Api.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService _service;


        public AlbumController(IAlbumService albumService)
        {
            _service = albumService;
        }

        [HttpGet]
        public IActionResult Index()
        {            
            return View(_service.GetAllAsync().Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Album model, [FromForm] IFormFile imageFile)
        {
            model.Image = _service.GetImageBytesAsync(model.ImageName).Result; //Переделать
            if (!ModelState.IsValid && model.Image.Length == 0)
            {
                return View();
            }

            model.Image = _service.GetImageBytesAsync(imageFile).Result;

            await _service.CreateAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Album model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            await _service.EditAsync(id, model);
            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Details(int id)
        {
            var album = await _service.DetailsAsync(id);
            
            return View(album);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
