using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Contracts;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.GroupService;
using MusicStoreInfo.Services.Services.ImageService;


namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IGroupService _service;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly MusicStoreDbContext _dbContext;
        private readonly string _staticFilesPath;


        public GroupController(IGroupService groupService, IImageService imageService, IWebHostEnvironment hostEnvironment, MusicStoreDbContext dbContext)
        {
            _service = groupService;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _dbContext = dbContext; 
            _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), _hostEnvironment.WebRootPath + "\\Image");
        }

        [HttpGet]
        public IActionResult Index(int page = 1, List<int> genreIds = null,
                                   string sortOrder = null, string searchString = null)
        {

            var groups = _service.GetAllAsync().Result;


            groups = Filter(genreIds, groups);

            if (!string.IsNullOrEmpty(searchString))
            {
                groups = groups.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            groups = Sort(sortOrder, groups);

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = groups.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = groups.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;
            ViewBag.GenreIds = genreIds ?? new List<int>();
            ViewBag.SortOrder = sortOrder;

            var groupViewModel = new GroupIndexViewModel
            {
                Groups = data,
                Genres = _dbContext.Genres.ToList()
            };

            return View(groupViewModel);
        }

        private static List<Group>? Sort(string sortOrder, List<Group>? groups)
        {
            switch (sortOrder)
            {
                case "name_asc":
                    groups = groups.OrderBy(p => p.Name).ToList();
                    break;
                default:
                    break;
            }

            return groups;
        }

        //TODO: Перенести в DAL и упростить
        private static List<Group>? Filter(List<int> genreIds, List<Group> groups)
        {
            if (genreIds != null && genreIds.Any())
                groups = groups.Where(p => p.Genres.Any(g => genreIds.Contains(g.Id))).ToList();
            return groups;
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupDto model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var image = await _imageService.CreateImageAsync(model.ImagePath, _staticFilesPath);

            var group = new Group
            {
                Name = model.Name,
                ImagePath = image
            };

            await _service.CreateAsync(group);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public IActionResult Edit(int id)
        {
            var model = _service.GetByIdAsync(id).Result;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupDto model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var image = await _imageService.CreateImageAsync(model.ImagePath, _staticFilesPath);

            var group = new Group
            {
                Id = model.Id,
                Name = model.Name,
                ImagePath = image,
            };

            await _service.EditAsync(group.Id, group);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var group = await _service.DetailsAsync(id);

            var groupViewModel = new GroupViewModel
            {
                Group = group,
                Members = _dbContext.Members.Include(m => m.Specializations)
                                            .Include(m => m.Gender).ToList(),
                Genres = _dbContext.Genres.ToList(),
            };
            return View(groupViewModel);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> AddGenre(int groupId, int genreId)
        {
            await _service.AddGenre(groupId, genreId);
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> DeleteGenre(int groupId, int genreId)
        {
            await _service.DeleteGenreAsync(groupId, genreId);
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> AddMember(int groupId, int memberId)
        {
            await _service.AddMemberAsync(groupId, memberId);
            return Json(new { success = true });
        }


        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> DeleteMember(int groupId, int memberId)
        {
            await _service.DeleteMemberAsync(groupId, memberId);
            return Json(new { success = true });
        }
    }
}
