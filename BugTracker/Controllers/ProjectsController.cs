using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Helpers;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper ph = new ProjectHelper();
        private UserRolesHelper urh = new UserRolesHelper();
        private UserHelper uh = new UserHelper();

        // GET: Projects
        [Authorize(Roles = "Admin, Project Manager, Developer, Submitter")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var ph = new ProjectHelper();
            var myRoles = urh.ListUserRoles(userId).ToList();
            var project = ph.ListUserProjects(User.Identity.GetUserId());

            if (myRoles.Any(role => role == "Admin,Project Manager"))
            {
                var projects = db.Project.ToList();
                return View(projects);
            }
            else
            {
                return View(project);
            }
        }

        // GET: ProjectsAssign
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult ProjectAssign(int? id, List<string> PA)
        {
            ViewBag.PA = new MultiSelectList(db.Users, "Id", "FirstName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Project.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAssign(int Id, List<string> PA)
        {
            if (PA == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projectHelper = new ProjectHelper();
            foreach(var user in db.Users)
            {
                projectHelper.RemoveUserToProject(user.Id, Id);
            }
            foreach(var userId in PA)
            {
                projectHelper.AddUserToProject(userId, Id);
            }
            return RedirectToAction("Index","Projects");
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Admin, Project Manager, Developer, Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            Projects projects = db.Project.Find(id);
            if (projects == null)
            {
                return View("404", "Account");
            }
            return View(projects);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                projects.Created = DateTimeOffset.Now;
                projects.Updated = DateTimeOffset.Now;
                db.Project.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Project.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Project.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Project.Find(id);
            db.Project.Remove(projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
