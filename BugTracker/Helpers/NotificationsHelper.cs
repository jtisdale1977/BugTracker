using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Data.Entity;
using System.Net.Mail;
using System.Data.SqlClient;

namespace BugTracker.Helpers
{
    public class NotificationsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task HandleNotifications(Tickets OGticket, Tickets newTicket)
        {
            await GenerateNotification(OGticket, newTicket);
            InsertNotificationTable(newTicket);
        }

        public async Task GenerateNotification(Tickets OGticket, Tickets newTicket)
        {
            var unassignedEmail = db.Users.FirstOrDefault(t => t.Id == OGticket.AssignedToUserId).Email;

            if (unassignedEmail == "unassed@mailinator.com" && !string.IsNullOrEmpty(newTicket.AssignedToUserId))
            {
                var myEmail = new PersonalEmail();
                myEmail.FromEmail = WebConfigurationManager.AppSettings["emailFrom"];
                myEmail.ToEmail = db.Users.AsNoTracking().FirstOrDefault(u => u.Id == newTicket.AssignedToUserId).Email;
                myEmail.Subject = string.Format("You have been assigned to a Ticket Id: {0}", OGticket.Id);
                myEmail.Body = ComposeEmailBody(newTicket);

                await SendNotification(myEmail);
            }
        }

        public string ComposeEmailBody(Tickets ticket)
        {
            var emailBody = new StringBuilder();
            emailBody.AppendFormat("Ticket Title: (0)", ticket.Title);
            emailBody.AppendLine("");
            emailBody.AppendFormat("Ticket Description: ");
            emailBody.AppendLine("");
            emailBody.AppendFormat("<p>{0}</p>", ticket.Description);
            return emailBody.ToString();
        }

        public void InsertNotificationTable(Tickets ticket)
        {
            var tn = new TicketNotifications();
            tn.TicketId = ticket.Id;
            tn.Detail = ComposeEmailBody(ticket);
            tn.UserId = ticket.AssignedToUser.Id;
            tn.GeneratedDate = DateTime.Now;
            db.TicketNotification.Add(tn);
            try
            {
                db.SaveChanges();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SendNotification(PersonalEmail model)
        {
            var body = "<p>Email From: <b>{0}</b> <br /> ({1})</p><p>Message:</p><p>{2}</p>";
            var emailFrom = WebConfigurationManager.AppSettings["emailFrom"];
            var email = new MailMessage(model.FromEmail, model.ToEmail)
            {
                Subject = model.Subject,
                Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                IsBodyHtml = true
            };
            var svc = new NotificationSvc();
            await svc.SendAsync(email);
        }

    }
}