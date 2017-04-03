using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class Projects
    {
        public Projects ()
        {
            this.Ticket = new HashSet<Tickets>();
            this.User = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        //public virtual ApplicationUser AssignedToUser { get; set; }

        //public int Count { get; set; }

        //public bool AssignedTo { get; set; }
        
        public virtual ICollection<Tickets> Ticket { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }
    }
}