using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class UserHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string GetDisplayNameFromId(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return db.Users.FirstOrDefault(u => u.Email == "unassed@mailinator.com").DisplayName;
            else
                return db.Users.Find(Id).DisplayName;
        }
    }
}