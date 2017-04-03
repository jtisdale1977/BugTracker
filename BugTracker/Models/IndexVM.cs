using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class IndexVM
    {
        public List<Projects> Projects { get; set; }
        public List<Tickets> Tickets { get; set; }
        public List<TicketNotifications> TicketNotifications { get; set; }
        public List<TicketComments> TicketComments { get; set; }

        public IndexVM()
        {
            this.Projects = new List<Projects>();
            this.Tickets = new List<Tickets>();
            this.TicketNotifications = new List<TicketNotifications>();
            this.TicketComments = new List<TicketComments>();
        }

        
    }
}