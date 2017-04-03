using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper urh = new UserRolesHelper();

        // GET: TicketComments
        [Authorize]
        public ActionResult Index()
        {
            var ticketComment = db.TicketComment.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketComment.ToList());
        }

        // GET: TicketComments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            TicketComments ticketComments = db.TicketComment.Find(id);
            if (ticketComments == null)
            {
                return View("404", "Account");
            }
            return View(ticketComments);
        }

        // GET: TicketComments/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            var ticketComment = new TicketComments();
            ticketComment.TicketId = id;
            return View(ticketComment);
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "TicketId, Comment")] TicketComments ticketComments)
        {
            if (ModelState.IsValid)
            {
                ticketComments.Created = DateTimeOffset.Now;
                ticketComments.UserId = User.Identity.GetUserId();
                db.TicketComment.Add(ticketComments);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketComments.TicketId });
            }

            ViewBag.TicketId = new SelectList(db.Ticket, "Id", "Title", ticketComments.TicketId);
            return View(ticketComments);
        }

        // GET: TicketComments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            TicketComments ticketComments = db.TicketComment.Find(id);
            if (ticketComments == null)
            {
                return View("404", "Account");
            }
            ViewBag.TicketId = new SelectList(db.Ticket, "Id", "Title", ticketComments.TicketId);
            return View(ticketComments);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Comment,Created,TicketId,UserId")] TicketComments ticketComments)
        {
            if (ModelState.IsValid)
            {
                ticketComments.Updated = DateTimeOffset.Now;
                ticketComments.UserId = User.Identity.GetUserId();
                db.Entry(ticketComments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Ticket, "Id", "Title", ticketComments.TicketId);
            return View(ticketComments);
        }

        // GET: TicketComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            TicketComments ticketComments = db.TicketComment.Find(id);
            if (ticketComments == null)
            {
                return View("404", "Account");
            }
            return View(ticketComments);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComments ticketComments = db.TicketComment.Find(id);
            db.TicketComment.Remove(ticketComments);
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
