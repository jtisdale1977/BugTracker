using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class Tickets
    {
        public Tickets()
        {
            this.TicketComment = new HashSet<TicketComments>();
            this.TicketAttachment = new HashSet<TicketAttachments>();
            this.TicketHistory = new HashSet<TicketHistories>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Display(Name = "Type")]
        public int TicketTypeId { get; set; }

        [Display(Name = "Priority")]
        public int TicketPriorityId { get; set; }

        [Display(Name = "Status")]
        public int TicketStatusId { get; set; }

        [Display(Name = "Owner")]
        public string OwnerUserId { get; set; }

        [Display(Name = "Assignee")]
        public string AssignedToUserId { get; set; }

        public virtual Projects Project { get; set; }

        public virtual TicketStatuses TicketStatus { get; set; }

        public virtual TicketPriorities TicketPriority { get; set; }

        public virtual TicketTypes TicketType { get; set; }

        public virtual ApplicationUser OwnerUser { get; set; }

        public virtual ApplicationUser AssignedToUser { get; set; }

        public virtual ICollection<TicketComments> TicketComment { get; set; }

        public virtual ICollection<TicketAttachments> TicketAttachment { get; set; }

        public virtual ICollection<TicketHistories> TicketHistory { get; set; }

        public virtual ICollection<TicketNotifications> TicketNotification { get; set; }
    }
}