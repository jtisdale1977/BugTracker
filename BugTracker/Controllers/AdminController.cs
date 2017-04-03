using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper ph = new ProjectHelper();
        private UserRolesHelper urh = new UserRolesHelper();

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult ListRoles()
        {
            return View(db.Users.ToList());
        }

        //Get
        [Authorize(Roles = "Admin")]
        public ActionResult RolesAssigned(string userId)
        {
            var roleHelp = new UserRolesHelper();
            var userAssigned = roleHelp.ListUserRoles(userId);
            ViewBag.Roles = new MultiSelectList(db.Roles, "Name", "Name", userAssigned);

            return View(db.Users.Find(userId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult RolesAssigned(string userId, List<string> Roles)
        {
            var roleHelp = new UserRolesHelper();

            foreach (var role in db.Roles)
            {
                var name = role.Name;
                roleHelp.RemoveUserFromRole(userId, name);
            }
            foreach (var role in Roles)
            {
                var list = roleHelp.AddUserToRole(userId, role);
            }
            return RedirectToAction("ListRoles", "Admin");
        }

        #region
        //Get
        //[Authorize(Roles = "Admin")]
        //public ActionResult RolesUnAssigned(string userId)
        //{
        //    var roleHelp = new UserRolesHelper();
        //    var userUnassigned = roleHelp.ListUserRoles(userId);
        //    ViewBag.Roles = new MultiSelectList(db.Roles, "Name", "Name", userUnassigned);

        //    return View(db.Users.Find(userId));
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public ActionResult RolesUnAssigned(string userId, List<string> Roles)
        //{
        //    var roleHelp = new UserRolesHelper();

        //    foreach (var role in db.Roles)
        //    {
        //        var name = role.Name;
        //        roleHelp.RemoveUserFromRole(userId, name);
        //    }
        //    return View("ListRoles", "Admin");
        //}
        #endregion

        //Get
        public ActionResult UserManagement(string userId)
        {
            ApplicationUser appUser = db.Users.Find(userId);
            return View(appUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult UserMangement([Bind(Include = "Id,FirstName,LastName,DislpayName,Email,UserName,PhoneNumber")] ApplicationUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserManagement", "Admin");
            }
            return View(appUser);
        }
    }
}