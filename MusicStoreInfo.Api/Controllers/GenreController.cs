using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.GenreService;

namespace MusicStoreInfo.Api.Controllers
{
    public class TopGenreViewModel
    {
        public string GenreName { get; set; } = string.Empty;
        public int AlbumCount { get; set; }
    }

    [Authorize]
    public class GenreController : Controller
    {
        private readonly IGenreService _service;
        private readonly MusicStoreDbContext _dbContext;

        public GenreController(IGenreService genreService, MusicStoreDbContext dbContext)
        {
            _service = genreService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_service.GetAllAsync().Result);
        }

        [HttpGet]
        public IActionResult Top10MostFavouriteGenre()
        {
            using (var context = new MusicStoreDbContext())
            {
                var topGenres = context.Genres
                    .Select(g => new TopGenreViewModel
                    {
                        GenreName = g.Name,
                        AlbumCount = g.Groups.SelectMany(gr => gr.Albums).Count()
                    })
                    .OrderByDescending(g => g.AlbumCount)
                    .Take(10)
                    .ToList();

                return View(topGenres);
            }
        }

        public IActionResult ExportTop10MostFavouriteGenreToExcel()
        {
            var topGenres = _dbContext.Genres
                .Select(g => new TopGenreViewModel
                {
                    GenreName = g.Name,
                    AlbumCount = g.Groups.SelectMany(gr => gr.Albums).Count()
                })
                .OrderByDescending(g => g.AlbumCount)
                .Take(10)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Top 10 Genres");
                worksheet.Cell(1, 1).Value = "Жанр";
                worksheet.Cell(1, 2).Value = "Количество альбомов";

                for (int i = 0; i < topGenres.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = topGenres[i].GenreName;
                    worksheet.Cell(i + 2, 2).Value = topGenres[i].AlbumCount;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Top10MostFavouriteGenres.xlsx");
                }
            }
        }
    

    [HttpGet]
        [Authorize(Policy = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Genre model)
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
            var model = await _service.GetByIdAsync(id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Genre model)
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
            var genre = await _service.DetailsAsync(id);

            var genreViewModel = new GenreViewModel
            {
                Genre = genre,
                Groups = _dbContext.Groups.ToList(),
            };

            return View(genreViewModel);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> AddGroup(int genreId, int groupId)
        {
            await _service.AddGroupAsync(genreId, groupId);
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> DeleteGroup(int genreId, int groupId)
        {
            await _service.DeleteGroupAsync(genreId, groupId);
            return Json(new { success = true });
        }
    }
}
