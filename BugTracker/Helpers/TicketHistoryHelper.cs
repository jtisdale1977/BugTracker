using BugTracker.Models;
using BugTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace BugTracker.Helpers
{
    public class TicketHistoryHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string ManageData(TicketHistories item, int index)
        {
            var data = " ";
            switch(item.Property)
            {
                case "ProjectId":
                    data = index == 0 ? ProjectHelper.GetProjectNameById(Convert.ToInt32(item.OldValue)) : ProjectHelper.GetProjectNameById(Convert.ToInt32(item.NewValue));
                    break;
                case "AssignedToUserId":
                    data = index == 0 ? UserHelper.GetDisplayNameFromId(item.OldValue) : UserHelper.GetDisplayNameFromId(item.NewValue);
                    break;
                case "RoleId":
                    data = index == 0 ? RoleHelper.GetRoleName(item.OldValue) : RoleHelper.GetRoleName(item.NewValue);
                    break;
                case "TicketPriorityId":
                    data = index == 0 ? TicketsHelper.GetTicketPriorityNameById(Convert.ToInt32(item.OldValue)) : TicketsHelper.GetTicketPriorityNameById(Convert.ToInt32(item.NewValue));
                    break;
                case "TicketStatusId":
                    data = index == 0 ? TicketsHelper.GetTicketStatusNameById(Convert.ToInt32(item.OldValue)) : TicketsHelper.GetTicketStatusNameById(Convert.ToInt32(item.NewValue));
                    break;
                case "TicketTypeId":
                    data = index == 0 ? TicketsHelper.GetTicketTypeNameById(Convert.ToInt32(item.OldValue)) : TicketsHelper.GetTicketTypeNameById(Convert.ToInt32(item.NewValue));
                    break;
                case "OwnerUserId":
                    data = index == 0 ? UserHelper.GetDisplayNameFromId(item.OldValue) : UserHelper.GetDisplayNameFromId(item.NewValue);
                    break;
                default:
                    data = index == 0 ? item.OldValue : item.NewValue;
                    break;
            }
            return data;
        }


        //private static ApplicationDbContext db = new ApplicationDbContext();
        //private static UserRolesHelper urh = new UserRolesHelper();
        //private static UserHelper uh = new UserHelper();

        //public void TicketHistory(Tickets ticket)
        //{
        //    TicketHistories th = new TicketHistories();
        //    var user = HttpContext.Current.User.Identity.GetUserId();
        //    var OGticket = db.Ticket.AsNoTracking().First(t => t.Id == ticket.Id);

        //    if (ticket.AssignedToUserId != OGticket.AssignedToUserId)
        //    {
        //        var getUser = db.Users.Where(g => g.Id == ticket.AssignedToUserId);
        //        var getUser2 = getUser.FirstOrDefault();
        //        var assUser = getUser2.FirstName;
        //        th.Property = "Assigned User";
        //        th.OldValue = OGticket.AssignedToUser.FirstName;
        //        th.NewValue = assUser;
        //        th.TicketId = ticket.Id;
        //        th.Changed = DateTimeOffset.Now;
        //        th.UserId = user;
        //        db.TicketHistory.Add(th);
        //        db.SaveChanges();
        //    }

        //    if (ticket.Title != OGticket.Title)
        //    {
        //        th.Property = "Title";
        //        th.OldValue = OGticket.Title;
        //        th.NewValue = ticket.Title;
        //        th.Changed = DateTimeOffset.Now;
        //        th.UserId = user;
        //        db.TicketHistory.Add(th);
        //        db.SaveChanges();
        //    }

        //    if (ticket.Description != OGticket.Description)
        //    {
        //        th.Property = "Description";
        //        th.OldValue = OGticket.Description;
        //        th.NewValue = ticket.Description;
        //        th.Changed = DateTimeOffset.Now;
        //        th.UserId = user;
        //        db.TicketHistory.Add(th);
        //        db.SaveChanges();
        //    }

        //    if (ticket.TicketTypeId != OGticket.TicketTypeId)
        //    {
        //        th.Property = "TicketTypeId";
        //        th.OldValue = OGticket.TicketTypeId.ToString();
        //        th.NewValue = ticket.TicketTypeId.ToString();
        //        th.Changed = DateTimeOffset.Now;
        //        th.UserId = user;
        //        db.TicketHistory.Add(th);
        //        db.SaveChanges();
        //    }

        //    if(ticket.TicketPriorityId != OGticket.TicketPriorityId)
        //    {
        //        th.Property = "TicketPriorityId";
        //        th.OldValue = OGticket.TicketPriorityId.ToString();
        //        th.NewValue = ticket.TicketPriorityId.ToString();
        //        th.Changed = DateTimeOffset.Now;
        //        th.UserId = user;
        //        db.TicketHistory.Add(th);
        //        db.SaveChanges();
        //    }

        //    if(ticket.TicketStatusId != OGticket.TicketStatusId)
        //    {
        //        th.Property = "TicketStatusId";
        //        th.OldValue = OGticket.TicketStatusId.ToString();
        //        th.NewValue = ticket.TicketStatusId.ToString();
        //        th.Changed = DateTimeOffset.Now;
        //        th.UserId = user;
        //        db.TicketHistory.Add(th);
        //        db.SaveChanges();
        //    }

        //}
    }
}