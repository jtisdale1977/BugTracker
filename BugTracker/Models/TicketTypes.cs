﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketTypes
    {
        public TicketTypes()
        {
            this.Ticket = new HashSet<Tickets>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Tickets> Ticket { get; set; }
    }
}