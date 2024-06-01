using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.SongService;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class SongController : Controller
    {
        private readonly ISongService _service;
        private readonly MusicStoreDbContext _dbContext;

        public SongController(ISongService songService, MusicStoreDbContext dbContext)
        {
            _service = songService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index(int page = 1,
                                   int? minDuration = null, int? maxDuration = null, string sortOrder = null, string searchString = null)
        {
            var songs = _service.GetAllAsync().Result;


            songs = Filter(minDuration, maxDuration, songs);

            if (!string.IsNullOrEmpty(searchString))
            {
                songs = songs.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            songs = Sort(sortOrder, songs);

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = songs.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = songs.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;
            ViewBag.MinDuration = minDuration;
            ViewBag.MaxDuration = maxDuration;
            ViewBag.SortOrder = sortOrder;
            return View(data);
        }

        private static List<Song>? Sort(string sortOrder, List<Song>? songs)
        {
            switch (sortOrder)
            {
                case "duration_asc":
                    songs = songs.OrderBy(p => p.Duration).ToList();
                    break;
                case "duration_desc":
                    songs = songs.OrderByDescending(p => p.Duration).ToList();
                    break;
                default:
                    break;
            }

            return songs;
        }

        //TODO: Перенести в DAL и упростить
        private static List<Song>? Filter(int? minDuration, int? maxDuration, List<Song> songs)
        {
            if (minDuration.HasValue)
                songs = songs.Where(a => a.Duration >= minDuration.Value).ToList();
            if (maxDuration.HasValue)
                songs = songs.Where(a => a.Duration <= maxDuration.Value).ToList();           
            return songs;
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public IActionResult Create()
        {
            ViewBag.Albums = new SelectList(_dbContext.Albums, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Create(Song model)
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
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Albums = new SelectList(_dbContext.Albums, "Id", "Name");
            var model = await _service.GetByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Song model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _service.EditAsync(model.Id, model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var song = await _service.DetailsAsync(id);

            return View(song);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
