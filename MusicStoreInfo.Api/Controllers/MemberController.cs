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
        public IActionResult Index()
        {
            return View(_service.GetAllAsync().Result);
        }

        [HttpGet]
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
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _service.GetByIdAsync(id);
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

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(int memberId, int groupId)
        {
            await _service.AddGroupAsync(memberId, groupId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialization(int memberId, int specializationId)
        {
            await _service.AddSpecializationAsync(memberId, specializationId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup(int memberId, int groupId)
        {
            await _service.DeleteGroupAsync(memberId, groupId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSpecialization(int memberId, int specializationId)
        {
            await _service.DeleteSpecializationAsync(memberId, specializationId);
            return Json(new { success = true });
        }
    }
}
