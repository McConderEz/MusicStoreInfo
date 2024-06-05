using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Services.Services.DistrictService;
using MusicStoreInfo.Services.Services.ListenerTypeService;

namespace MusicStoreInfo.Api.Controllers
{
    [Authorize]
    public class ListenerTypeController : Controller
    {
        private readonly IListenerTypeService _service;


        public ListenerTypeController(IListenerTypeService listenerTypeService)
        {
            _service = listenerTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string searchString = null)
        {
            var listenerTypes = await _service.GetAllAsync();
            const int pageSize = 10;
            if (page < 1)
                page = 1;

            if (!string.IsNullOrEmpty(searchString))
            {
                listenerTypes = listenerTypes.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int recsCount = listenerTypes.Count;
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = listenerTypes.Skip(recSkip).Take(pageSize).ToList();
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
        public async Task<IActionResult> Create(ListenerType model)
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
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ListenerType model)
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
            var listenerType = await _service.DetailsAsync(id);

            return View(listenerType);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
