using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugBully.Data;
using BugBully.Models;
using BugBully.Services;

namespace BugBully.Controllers
{
    public class BugsController : Controller
    {
        private readonly BugService _dependecyService;

        public BugsController(IRepository context)
        {
            _dependecyService = new BugService(context);
        }

        // GET: Bugs
        public async Task<ActionResult> Index()
        {
            var bugs = await _dependecyService.GetBugsIncludedAsync();
            return View(bugs);
        }

        // GET: Bugs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bug = await _dependecyService.GetBugByIdAsync(id);
            if (bug == null)
            {
                return HttpNotFound();
            }
            return View(bug);
        }

        // GET: Bugs/Create
        public ActionResult Create()
        {
            ViewBag.StatusId = new SelectList(_dependecyService.GetStatusSet(), "Id", "Name");
            ViewBag.UserId = new SelectList(_dependecyService.GetUserSet(), "Id", "Name");
            return View();
        }

        // POST: Bugs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,DateReported,StatusId,UserId")] Bugs bug)
        {
            if (ModelState.IsValid)
            {
                var isAdded = await _dependecyService.AddBugAsync(bug);
                if (isAdded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }

            ViewBag.StatusId = new SelectList(_dependecyService.GetStatusSet(), "Id", "Name", bug.StatusId);
            return View(bug);
        }

        // GET: Bugs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bug = await _dependecyService.GetBugByIdAsync(id);
            if (bug == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(_dependecyService.GetStatusSet(), "Id", "Name", bug.StatusId);
            ViewBag.UserId = new SelectList(_dependecyService.GetUserSet(), "Id", "Name", bug.UserId);
            return View(bug);
        }

        // POST: Bugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,DateReported,StatusId,UserId")] Bugs bug)
        {
            if (ModelState.IsValid)
            {
                var isModified = await _dependecyService.UpdateBugAsync(bug);
                if (isModified)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Edit", bug.Id);
                }
            }
            ViewBag.StatusId = new SelectList(_dependecyService.GetStatusSet(), "Id", "Name", bug.StatusId);
            ViewBag.UserId = new SelectList(_dependecyService.GetUserSet(), "Id", "Name", bug.UserId);
            return View(bug);
        }

        // GET: Bugs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bug = await _dependecyService.GetBugByIdAsync(id);
            if (bug == null)
            {
                return HttpNotFound();
            }
            return View(bug);
        }

        // POST: Bugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var bug = await _dependecyService.GetBugByIdAsync(id);
            var isDeleted = await _dependecyService.DeleteBugAsync(bug);
            if (isDeleted)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Delete", id);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dependecyService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
