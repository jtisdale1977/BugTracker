using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper ph = new ProjectHelper();
        private UserRolesHelper urh = new UserRolesHelper();

        public ActionResult IndexLand()
        {
            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var myIndexVM = new IndexVM();
            var myRoles = urh.ListUserRoles(userId).ToList();

            if (myRoles.Any(role => role == "Admin"))
            {
                myIndexVM.Projects = db.Project.ToList();
                myIndexVM.Tickets = db.Ticket.ToList();
                myIndexVM.TicketNotifications = db.Users.Where(u => u.Id == userId).SelectMany(tn => tn.TicketNotification).ToList();
                //myIndexVM.TicketComments = db.Users.Where(u => u.Id == userId).SelectMany(tc => tc.TicketComment).ToList();
            }

            else if (myRoles.Any(role => role == "Project Manager"))
            {
                myIndexVM.Projects = db.Project.ToList();
                myIndexVM.Tickets = db.Users.Where(u => u.Id == userId).SelectMany(t => t.Tickets).ToList();
                myIndexVM.TicketNotifications = db.Users.Where(u => u.Id == userId).SelectMany(tn => tn.TicketNotification).ToList();
                //myIndexVM.TicketComments = db.Users.Where(u => u.Id == userId).SelectMany(tc => tc.TicketComment).ToList();
            }

            else if (myRoles.Any(role => role == "Developer"))
            {
                myIndexVM.Projects = db.Users.Where(u => u.Id == userId).SelectMany(p => p.Projects).ToList();
                myIndexVM.Tickets = db.Users.Where(u => u.Id == userId).SelectMany(t => t.Tickets).ToList();
                myIndexVM.TicketNotifications = db.Users.Where(u => u.Id == userId).SelectMany(tn => tn.TicketNotification).ToList();
                //myIndexVM.TicketComments = db.Users.Where(u => u.Id == userId).SelectMany(tc => tc.TicketComment).ToList();
            }

            else if (myRoles.Any(role => role == "Submitter"))
            {
                myIndexVM.Projects = db.Users.Where(u => u.Id == userId).SelectMany(p => p.Projects).ToList();
                myIndexVM.Tickets = db.Users.Where(u => u.Id == userId).SelectMany(t => t.Tickets).ToList();
                myIndexVM.TicketNotifications = db.Users.Where(u => u.Id == userId).SelectMany(tn => tn.TicketNotification).ToList();
                //myIndexVM.TicketComments = db.Users.Where(u => u.Id == userId).SelectMany(tn => tn.TicketComment).ToList();
            }


            return View(myIndexVM);
        }

        public ActionResult AdminIndex()
        {
            return View();
        }

        public ActionResult UserIndex()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //public FileContentResult UserPhotos()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        //let pass User.Identity into userId
        //        String userId = User.Identity.GetUserId();

        //        if (userId == null)
        //        {
        //            //if there is no photo chosen then use Stock photo- I am using CoderFoundry image
        //            string fileName = HttpContext.Server.MapPath(@"~/images/coderfoundry.png");
        //            //convert import image into byte file that can read by using FileStream and BinaryReader Method
        //            byte[] imageData = null;
        //            FileInfo fileInfo = new FileInfo(fileName);
        //            long imageFileLength = fileInfo.Length;
        //            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //            BinaryReader br = new BinaryReader(fs);
        //            imageData = br.ReadBytes((int)imageFileLength);

        //            return File(imageData, "image/png");

        //        }
        //        // to get the user details to load user Image 
        //        var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        //        var UserImage = bdUsers.Users.Where(photo => photo.Id == userId).FirstOrDefault();

        //        return new FileContentResult(UserImage.UserPhoto, "image/jpeg");
        //    }
        //    else
        //    {
        //        string fileName = HttpContext.Server.MapPath(@"~/images/coderfoundry.png");

        //        byte[] imageData = null;
        //        FileInfo fileInfo = new FileInfo(fileName);
        //        long imageFileLength = fileInfo.Length;
        //        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        BinaryReader br = new BinaryReader(fs);
        //        imageData = br.ReadBytes((int)imageFileLength);
        //        return File(imageData, "image/png");

        //    }
        //}
    }
}