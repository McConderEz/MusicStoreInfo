using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.Api.Contracts;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.AlbumService;
using MusicStoreInfo.Services.Services.ImageService;

namespace MusicStoreInfo.Api.Controllers
{
    public class AlbumController : Controller
    {
        //TODO: Сделать добавление данных:Песни, Продукты, Магазины
        //TODO: Сделать ввод справочников по названиям, а не Id.(Выбираем по названию, за которым закреплен Id)
        //TODO: Добавить стиль

        private readonly IAlbumService _service;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _staticFilesPath;

        public AlbumController(IAlbumService albumService, IImageService imageService,
            IWebHostEnvironment hostEnvironment)
        {
            _service = albumService;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), _hostEnvironment.WebRootPath + "\\Image");
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
        public async Task<IActionResult> Create(AlbumDto model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var image = await _imageService.CreateImageAsync(model.ImagePath, _staticFilesPath);

            var album = new Album
            {
                Name = model.Name,
                ListenerTypeId = model.ListenerTypeId,
                CompanyId = model.CompanyId,
                GroupId = model.GroupId,
                Duration = model.Duration,
                ReleaseDate = model.ReleaseDate,
                SongsCount = model.SongsCount,
                ImagePath = image
            };

            await _service.CreateAsync(album);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _service.GetByIdAsync(id).Result;
            return View(model);
        }

        //TODO: Изменить на DTO 
        [HttpPost]
        public async Task<IActionResult> Edit(Album model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            await _service.EditAsync(model.Id, model);
            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Details(int id)
        {
            var album = await _service.DetailsAsync(id);

            var albumViewModel = new AlbumViewModel
            {
                Album = album,
                Songs = album.Songs.ToList(),
                Stores = album.Stores.ToList(),
                Products = album.Products.ToList(),
            };

            return View(albumViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
