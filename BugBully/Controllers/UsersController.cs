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
    public class UsersController : Controller
    {
        private readonly UserService _dependecyService;

        public UsersController(IRepository context)
        {
            _dependecyService = new UserService(context);
        }

        // GET: User
        public async Task<ActionResult> Index()
        {
            var users = await _dependecyService.GetUsersAsync();
            return View(users);
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _dependecyService.GetUserByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include = "Id,Name,Username,Password")] Users user)
        {
            if (ModelState.IsValid)
            {
                var isAdded = await _dependecyService.AddUserAsync(user);
                if (isAdded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _dependecyService.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Password = string.Empty;
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Username,Password")] Users user)
        {
            if (ModelState.IsValid)
            {
                var isModified = await _dependecyService.UpdateUserAsync(user);
                if (isModified)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _dependecyService.GetUserByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var user = await _dependecyService.GetUserByIdAsync(id);
            await _dependecyService.DeleteUserAsync(user);
            
            return RedirectToAction("Index");
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
