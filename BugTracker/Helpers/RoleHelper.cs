using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class RoleHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string GetRoleName(string Id)
        {
            return db.Roles.FirstOrDefault(m => m.Id == Id).Name;
        }
    }
}