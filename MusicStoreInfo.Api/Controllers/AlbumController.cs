using Microsoft.AspNetCore.Authorization;
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




       //TODO: Исправить кнопки 

        //?
        //Партиции

        //*
        //TODO: Сделать все запросы в виде хранимых процедур, вызывать и мапить. Отчёт в эксель сделать иначе
        //TODO: Вывод списка заказов у Менеджера и Клиента
        //TODO: Статистика доходов магазина у менеджера за определённый срок



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
        public IActionResult Index(int page = 1,int? minSongsCount = null, int? maxSongsCount = null,
                                   int? minDuration = null, int? maxDuration = null,DateTime? minDate = null, DateTime? maxDate = null, List<int> listenerTypeIds = null,
                                   List<int> groupIds = null, List<int> genreIds = null, string sortOrder = null, string searchString = null)
        {
            //TODO: Сделать частичное представление, чтобы увеличить производительность
            var albums = _service.GetAllAsync().Result;


            albums = Filter(minSongsCount, maxSongsCount, minDuration, maxDuration, minDate, maxDate, listenerTypeIds, groupIds, genreIds, albums);

            if (!string.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                        || p.Group.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                        || p.Songs.Any(s => s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            albums = Sort(sortOrder, albums);

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = albums.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = albums.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;

            ViewBag.MinSongsCount = minSongsCount;
            ViewBag.MaxSongsCount = maxSongsCount;
            ViewBag.MinDuration = minDuration;
            ViewBag.MaxDuration = maxDuration;
            ViewBag.MinDate = minDate;
            ViewBag.MaxDate = maxDate;
            ViewBag.ListenerTypeIds = listenerTypeIds ?? new List<int>();
            ViewBag.GroupIds = groupIds ?? new List<int>();
            ViewBag.GenreIds = genreIds ?? new List<int>();
            ViewBag.SortOrder = sortOrder;
            var albumIndexViewModel = new AlbumIndexViewModel
            {
                Albums = data,
                ListenerTypes = _dbContext.ListenerTypes.ToList(),
                Groups = _dbContext.Groups.ToList(),
                Genres = _dbContext.Genres.ToList()
            };

            return View(albumIndexViewModel);
        }

        private static List<Album>? Sort(string sortOrder, List<Album>? albums)
        {
            switch (sortOrder)
            {
                case "duration_asc":
                    albums = albums.OrderBy(p => p.Duration).ToList();
                    break;
                case "duration_desc":
                    albums = albums.OrderByDescending(p => p.Duration).ToList();
                    break;
                case "songCount":
                    albums = albums.OrderBy(p => p.SongsCount).ToList();
                    break;
                case "date_asc":
                    albums = albums.OrderBy(p => p.ReleaseDate.Date).ToList();
                    break;
                case "date_desc":
                    albums = albums.OrderByDescending(p => p.ReleaseDate.Date).ToList();
                    break;
                default:
                    break;
            }

            return albums;
        }

        //TODO: Перенести в DAL и упростить
        private static List<Album>? Filter(int? minSongsCount, int? maxSongsCount, int? minDuration, int? maxDuration, DateTime? minDate, DateTime? maxDate,List<int> listenerTypeIds , List<int> groupIds, List<int> genreIds, List<Album> albums)
        {
            if (minSongsCount.HasValue)
                albums = albums.Where(a => a.SongsCount >= minSongsCount.Value).ToList();
            if (maxSongsCount.HasValue)
                albums = albums.Where(a => a.SongsCount <= maxSongsCount.Value).ToList();
            if (minDuration.HasValue)
                albums = albums.Where(a => a.Duration >= minDuration.Value).ToList();
            if (maxDuration.HasValue)
                albums = albums.Where(a => a.Duration <= maxDuration.Value).ToList();
            if (minDate.HasValue)
                albums = albums.Where(a => a.ReleaseDate.Date >= minDate.Value).ToList();
            if (maxDate.HasValue)
                albums = albums.Where(a => a.ReleaseDate.Date <= maxDate.Value).ToList();
            if (listenerTypeIds != null && listenerTypeIds.Any())
                albums = albums.Where(a => listenerTypeIds.Contains(a.ListenerTypeId)).ToList();
            if (groupIds != null && groupIds.Any())
                albums = albums.Where(a => groupIds.Contains(a.GroupId)).ToList();
            if (genreIds != null && genreIds.Any())
                albums = albums.Where(a => a.Group.Genres.Any(a => genreIds.Contains(a.Id))).ToList();
            return albums;
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
