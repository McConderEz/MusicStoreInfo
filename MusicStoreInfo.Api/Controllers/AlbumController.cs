﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Contracts;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.AlbumService;
using MusicStoreInfo.Services.Services.ImageService;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {

        //TODO: Добавить пагинацию по возможности
        //TODO: Добавить красивый список вкладок(выполнено)
        //TODO: Добавить аутентификацию/авторизацию и регистрацию(Выполнено)

        //TODO: Добавить роли и политику(выполнено)
        //TODO: Добавить админ-панель(выполнено)
        //TODO: Добавить корзину товаров*

        //TODO: Добавить возможность устанавливать фото в профиль(Изменить модель User)(выполнено)

        //TODO: Сделать отдельным модулем генерацию записей для таблиц и справочников 

        //TODO: Сделать 5 и 6 лабу*(Запросы)

        //TODO: Сделать фильтрацию и поиск записей

        private readonly IAlbumService _service;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly MusicStoreDbContext _dbContext;
        private readonly string _staticFilesPath;

        public AlbumController(IAlbumService albumService, IImageService imageService,
            IWebHostEnvironment hostEnvironment, MusicStoreDbContext dbContext)
        {
            _service = albumService;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _dbContext = dbContext;
            _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), _hostEnvironment.WebRootPath + "\\Image");
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
            ViewBag.ListenerTypes = new SelectList(_dbContext.ListenerTypes, "Id","Name");
            ViewBag.Companies = new SelectList(_dbContext.Companies, "Id","Name");
            ViewBag.Groups = new SelectList(_dbContext.Groups, "Id","Name");
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
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
        [Authorize(Policy = "Manager")]
        public IActionResult Edit(int id)
        {
            ViewBag.ListenerTypes = new SelectList(_dbContext.ListenerTypes, "Id", "Name");
            ViewBag.Companies = new SelectList(_dbContext.Companies, "Id", "Name");
            ViewBag.Groups = new SelectList(_dbContext.Groups, "Id", "Name");
            var model = _service.GetByIdAsync(id).Result;
            return View(model);
        }


        //TODO:Сделать ограничения на картинки

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Edit(AlbumDto model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var image = await _imageService.CreateImageAsync(model.ImagePath, _staticFilesPath);

            var album = new Album
            {
                Id = model.Id,
                Name = model.Name,
                ListenerTypeId = model.ListenerTypeId,
                CompanyId = model.CompanyId,
                GroupId = model.GroupId,
                Duration = model.Duration,
                ReleaseDate = model.ReleaseDate,
                SongsCount = model.SongsCount,
                ImagePath = image
            };

            await _service.EditAsync(album.Id, album);
            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Details(int id)
        {
            var album = await _service.DetailsAsync(id);

            var albumViewModel = new AlbumViewModel
            {
                Album = album,
                Songs = album.Songs.ToList(),
                Stores = _dbContext.Stores.Include(s => s.District)
                                          .ThenInclude(d => d.City).ToList(),
                Products = album.Products.ToList(),
            };

            return View(albumViewModel);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> AddStore(int albumId, int storeId)
        {
            var store = await _dbContext.Stores.FindAsync(storeId);
            await _service.AddStore(albumId, store);
            return Json(new {success = true});
        }
    }
}
