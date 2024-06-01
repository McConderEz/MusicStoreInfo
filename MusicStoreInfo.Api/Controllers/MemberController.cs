using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.MemberService;
using NuGet.Protocol.Core.Types;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberService _service;
        private readonly MusicStoreDbContext _dbContext;


        public MemberController(IMemberService memberService, MusicStoreDbContext dbContext)
        {
            _service = memberService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, List<int> genderIds = null, List<int> specializationIds = null,
                                   List<int> groupIds = null, List<int> genreIds = null,
                                   string sortOrder = null, string searchString = null)
        {
            var members = _service.GetAllAsync().Result;

            // Фильтрация участников
            members = Filter(genderIds, specializationIds, groupIds, genreIds, members);

            // Поиск по строке
            if (!string.IsNullOrEmpty(searchString))
            {
                members = members.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                        || p.SecondName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Сортировка участников
            members = Sort(sortOrder, members);

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            // Пагинация
            int recsCount = members.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = members.Skip(recSkip).Take(pageSize).ToList();

            // Сохранение параметров в ViewBag
            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;
            ViewBag.GenderIds = genderIds ?? new List<int>();
            ViewBag.SpecializationIds = specializationIds ?? new List<int>();
            ViewBag.GroupIds = groupIds ?? new List<int>();
            ViewBag.GenreIds = genreIds ?? new List<int>();
            ViewBag.SortOrder = sortOrder;

            // Формирование ViewModel
            var memberIndexViewModel = new MemberIndexViewModel
            {
                Members = data,
                Genders = _dbContext.Genders.ToList(),
                Groups = _dbContext.Groups.ToList(),
                Genres = _dbContext.Genres.ToList(),
                Specializations = _dbContext.Specializations.ToList()
            };

            return View(memberIndexViewModel);
        }

        private static List<Member>? Sort(string sortOrder, List<Member>? members)
        {
            switch (sortOrder)
            {
                case "age_asc":
                    members = members.OrderBy(p => p.Age).ToList();
                    break;
                case "age_desc":
                    members = members.OrderByDescending(p => p.Age).ToList();
                    break;
                case "name_asc":
                    members = members.OrderBy(p => p.Name).ToList();
                    break;
                case "name_desc":
                    members = members.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    break;
            }

            return members;
        }

        //TODO: Перенести в DAL и упростить
        private static List<Member>? Filter(List<int>? genderIds, List<int>? specializationIds,
                                    List<int>? groupIds, List<int>? genreIds, List<Member> members)
        {
            if (genderIds != null && genderIds.Any())
                members = members.Where(m => genderIds.Contains(m.GenderId)).ToList();

            if (specializationIds != null && specializationIds.Any())
                members = members.Where(m => m.Specializations.Any(s => specializationIds.Contains(s.Id))).ToList();

            if (groupIds != null && groupIds.Any())
                members = members.Where(m => m.Groups.Any(g => groupIds.Contains(g.Id))).ToList();

            if (genreIds != null && genreIds.Any())
                members = members.Where(m => m.Groups.Any(g => g.Genres.Any(gen => genreIds.Contains(gen.Id)))).ToList();

            return members;
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public IActionResult Create()
        {
            ViewBag.Genders = new SelectList(_dbContext.Genders, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Member model)
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
            ViewBag.Genders = new SelectList(_dbContext.Genders, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Member model)
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
            var member = await _service.DetailsAsync(id);

            var memberViewModel = new MemberViewModel
            {
                Member = member,
                Groups = _dbContext.Groups.ToList(),
                Specializations = _dbContext.Specializations.ToList(),
            };

            return View(memberViewModel);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> AddGroup(int memberId, int groupId)
        {
            await _service.AddGroupAsync(memberId, groupId);
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> AddSpecialization(int memberId, int specializationId)
        {
            await _service.AddSpecializationAsync(memberId, specializationId);
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> DeleteGroup(int memberId, int groupId)
        {
            await _service.DeleteGroupAsync(memberId, groupId);
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> DeleteSpecialization(int memberId, int specializationId)
        {
            await _service.DeleteSpecializationAsync(memberId, specializationId);
            return Json(new { success = true });
        }
    }
}
