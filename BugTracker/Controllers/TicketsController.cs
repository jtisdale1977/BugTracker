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
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper urh = new UserRolesHelper();

        // GET: Tickets
        [Authorize(Roles = "Admin, Project Manager, Developer, Submitter")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var th = new TicketsHelper(urh, db);
            var myRoles = urh.ListUserRoles(userId).ToList();
            var ticket = th.GetUserTickets(User.Identity.GetUserId());

            if (myRoles.Any(role => role == "Admin,Project Manager"))
            {
                var tickets = db.Ticket.ToList();
                return View(tickets);
            }
            else
            {
                return View(ticket);
            }
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin, Project Manager, Developer, Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            Tickets tickets = db.Ticket.Find(id);
            if (tickets == null)
            {
                return View("404", "Account");
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int id)
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name");
            ViewBag.Developer = new MultiSelectList(urh.UsersInRole("Submitter"), "Id", "DisplayName");
            return View(new Tickets { ProjectId = id } );
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                //tickets.Id = Id;
                tickets.Created = DateTimeOffset.Now;
                tickets.Updated = DateTimeOffset.Now;
                tickets.OwnerUserId = User.Identity.GetUserId();
                var unassignedUser = WebConfigurationManager.AppSettings["Unassigned"];
                tickets.AssignedToUserId = db.Users.FirstOrDefault(e => e.Email == unassignedUser).Id;

                db.Ticket.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "DisplayName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "DisplayName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, Project Manager, Developer, Submitter")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            Tickets tickets = db.Ticket.Find(id);
            if (tickets == null)
            {
                return View("404", "Account");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "DisplayName", tickets.AssignedToUserId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "DisplayName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name", tickets.TicketTypeId);

            var developers = urh.UsersInRole("Developer");
            ViewBag.Developer = new MultiSelectList(urh.UsersInRole("Developer"), "Id", "DisplayName", tickets.AssignedToUserId);

            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            var th = new TicketsHelper(urh, db);
            var OGticket = db.Ticket.AsNoTracking().First(t => t.Id == tickets.Id);

            if (ModelState.IsValid)
            {
                //tickets.AssignedToUserId = Developer;
                db.Entry(tickets).State = EntityState.Modified;

                db.SaveChanges();

                var newTicket = db.Ticket.AsNoTracking().First(t => t.Id == tickets.Id);

                th.AddTicketHistory(OGticket, newTicket, User.Identity.GetUserId());

                var nh = new NotificationsHelper();
                await nh.HandleNotifications(OGticket, newTicket);

                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "DisplayName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "DisplayName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name", tickets.TicketTypeId);

            var developers = urh.UsersInRole("Developer");
            ViewBag.Developer = new MultiSelectList(urh.UsersInRole("Developer"), "Id", "DisplayName");

            return View(tickets);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("404", "Account");
            }
            Tickets tickets = db.Ticket.Find(id);
            if (tickets == null)
            {
                return View("404", "Account");
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.Ticket.Find(id);
            db.Ticket.Remove(tickets);
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
