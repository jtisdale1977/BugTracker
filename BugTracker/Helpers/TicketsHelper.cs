using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BugTracker.Helpers
{
    public class TicketsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserRolesHelper urh = new UserRolesHelper();
        private static TicketHistoryHelper thh = new TicketHistoryHelper();

        private UserRolesHelper roleHelper;
        private ApplicationDbContext context;

        public TicketsHelper(UserRolesHelper urh, ApplicationDbContext db)
        {
            roleHelper = urh;
            context = db;
        }

        public List<Tickets> GetUserTickets(string userId)
        {
            var tickets = new List<Tickets>();
            var myRoles = urh.ListUserRoles(userId).ToList();

            if (myRoles.Any(role => role == "Admin"))
                tickets = db.Ticket.ToList();

            else if (myRoles.Any(role => role == "Project Manager"))
                tickets = db.Users.Where(u => u.Id == userId).SelectMany(p => p.Projects).SelectMany(t => t.Ticket).ToList();

            else if (myRoles.Any(role => role == "Developer"))
                tickets = db.Ticket.Where(t => t.AssignedToUser.Id == userId).ToList();

            else if (myRoles.Any(role => role == "Submitter"))
                tickets = db.Ticket.Where(t => t.OwnerUser.Id == userId).ToList();

            return tickets;
        }

        public void AddTicketHistory(Tickets OGticket, Tickets newTicket, string userId)
        {

            var unassignedUserEmail = WebConfigurationManager.AppSettings["Unassigned"];
            var unassignedUserId = db.Users.AsNoTracking().First(u => u.Email == unassignedUserEmail).Id;

            foreach (var prop in typeof(Tickets).GetProperties())
            {
                foreach (var ticket in new List<Tickets> { OGticket, newTicket })
                {
                    if (prop.Name.ToLower().Contains("UserId") && prop.GetValue(ticket) == null)
                        prop.SetValue(ticket, unassignedUserId, null);
                }

                if (object.ReferenceEquals(prop.GetValue(OGticket), prop.GetValue(newTicket))) { continue; }
                if (prop.GetValue(OGticket).Equals(prop.GetValue(newTicket))) { continue; }

                var ticketHistory = new TicketHistories()
                {
                    TicketId = OGticket.Id,
                    Property = prop.ToString(),
                    OldValue = prop.GetValue(OGticket).ToString(),
                    NewValue = prop.GetValue(newTicket).ToString(),
                    Changed = DateTimeOffset.Now,
                    UserId = userId

                };
                db.TicketHistory.Add(ticketHistory);
                db.SaveChanges();
            }
        }

        public static string GetTicketPriorityNameById(int Id)
        {
            return db.TicketPriority.AsNoTracking().FirstOrDefault(tp => tp.Id == Id).Name;
        }

        public static string GetTicketStatusNameById(int Id)
        {
            return db.TicketStatus.AsNoTracking().FirstOrDefault(tp => tp.Id == Id).Name;
        }

        public static string GetTicketTypeNameById(int Id)
        {
            return db.TicketType.AsNoTracking().FirstOrDefault(tp => tp.Id == Id).Name;
        }

    }
}