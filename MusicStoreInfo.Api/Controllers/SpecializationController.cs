using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.SpecializationService;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class SpecializationController : Controller
    {
        private readonly ISpecializationService _service;
        private readonly MusicStoreDbContext _dbContext;

        public SpecializationController(ISpecializationService specializationService, MusicStoreDbContext dbContext)
        {
            _service = specializationService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1,string searchString = null)
        {

            var specializations = await _service.GetAllAsync();
            const int pageSize = 10;
            if (page < 1)
                page = 1;

            if (!string.IsNullOrEmpty(searchString))
            {
                specializations = specializations.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int recsCount = specializations.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = specializations.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.Pager = pager;
            ViewBag.CurrentFilter = searchString;

            return View(data);
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Specialization model)
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
        public async Task<IActionResult> Edit(Specialization model)
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
            var specialization = await _service.DetailsAsync(id);

            var specializationViewModel = new SpecializationViewModel
            {
                Specialization = specialization,
                Members = _dbContext.Members.Include(m => m.Gender)
                                            .Include(m => m.Groups).ToList()
            };

            return View(specializationViewModel);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> AddMember(int specializationId, int memberId)
        {
            await _service.AddMemberAsync(specializationId, memberId);
            return Json(new { success = true });
        }


        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> DeleteMember(int specializationId, int memberId)
        {
            await _service.DeleteMemberAsync(specializationId, memberId);
            return Json(new { success = true });
        }
    }
}
