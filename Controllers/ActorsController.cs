using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorsService _service;
        public ActorsController(IActorsService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var allActors = await _service.GetAllAsync();
            return View(allActors);
        }

        //Get : Actors/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureUrl, FullName, Bio")] Actor actor)
        {
            if (!ModelState.IsValid) return View(actor);

            await _service.AddAsync(actor);
            return RedirectToAction(nameof(Index));

        }

        //Get: Actors/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var actorDetail = await _service.GetByIdAsync(id);
            if (actorDetail == null) return View("NotFound");
            return View(actorDetail);
        }

        //Get : Actors/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var actorDetail = await _service.GetByIdAsync(id);
            if (actorDetail == null) return View("NotFound");
            return View(actorDetail);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, ProfilePictureUrl, FullName, Bio")] Actor actor)
        {
            if (!ModelState.IsValid) return View(actor);

            await _service.UpdateAsync(id, actor);
            return RedirectToAction(nameof(Index));

        }

        //Get : Actors/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var actorDetail = await _service.GetByIdAsync(id);
            if (actorDetail == null) return View("NotFound");
            return View(actorDetail);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetail = await _service.GetByIdAsync(id);
            if (actorDetail == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));

        }
    }
}
