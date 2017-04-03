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
using System.IO;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AttachmentUpload au = new AttachmentUpload();
        //private TicketsHelper th = new TicketsHelper();

        // GET: TicketAttachments
        [Authorize]
        public ActionResult Index()
        {
            var ticketAttachment = db.TicketAttachment.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachment.ToList());
        }

        // GET: TicketAttachments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            TicketAttachments ticketAttachments = db.TicketAttachment.Find(id);
            if (ticketAttachments == null)
            {
                return View("404", "Account");
            }
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            var ticketAttachment = new TicketAttachments();
            ticketAttachment.TicketId = id;
            //ViewBag.TicketId = new SelectList(db.Ticket, "Id", "Title");
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "TicketId,FilePath,Description,Created,UserId,FileUrl")] TicketAttachments ticketAttachments, HttpPostedFileBase attachment)
        {
            if (ModelState.IsValid)
            {
                if (AttachmentUpload.WebFriendlyImage(attachment))
                {
                    var fileName = Path.GetFileName(attachment.FileName);
                    attachment.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    ticketAttachments.FilePath = "~/Uploads/" + fileName;
                }

                ticketAttachments.Created = DateTimeOffset.Now;
                ticketAttachments.UserId = User.Identity.GetUserId();
                db.TicketAttachment.Add(ticketAttachments);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketAttachments.TicketId });
            }

            ViewBag.TicketId = new SelectList(db.Ticket, "Id", "Title", ticketAttachments.TicketId);
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            TicketAttachments ticketAttachments = db.TicketAttachment.Find(id);
            if (ticketAttachments == null)
            {
                return View("404", "Account");
            }
            ViewBag.TicketId = new SelectList(db.Ticket, "Id", "Title", ticketAttachments.TicketId);
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,TicketId,FilePath,Description,Created,UserId,FileUrl")] TicketAttachments ticketAttachments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Ticket, "Id", "Title", ticketAttachments.TicketId);
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            TicketAttachments ticketAttachments = db.TicketAttachment.Find(id);
            if (ticketAttachments == null)
            {
                return View("404", "Account");
            }
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachments ticketAttachments = db.TicketAttachment.Find(id);
            db.TicketAttachment.Remove(ticketAttachments);
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
