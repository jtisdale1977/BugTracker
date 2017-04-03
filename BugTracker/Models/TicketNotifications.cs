using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketNotifications
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public string UserId { get; set; }

        public string Detail { get; set; }

        public int Count { get; set; }

        public DateTime? GeneratedDate { get; set; }

        public virtual Tickets Ticket { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}