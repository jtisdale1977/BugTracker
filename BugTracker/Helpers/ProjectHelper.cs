using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string userId, int projectId)
        {
            if(db.Project.Find(projectId).User.Contains(db.Users.Find(userId)))
            {
                return true;
            }
            return false;
            //var project = db.Project.Find(projectId);
            //var flag = project.User.Any(u => u.Id == userId);
            //return (projectId);
        }

        public ICollection<Projects> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var projects = user.Projects.ToList();
            return (projects);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if(!IsUserOnProject(userId, projectId))
            {
                Projects proj = db.Project.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.User.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserToProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Projects proj = db.Project.Find(projectId);
                var oldUser = db.Users.Find(userId);

                proj.User.Remove(oldUser);
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> UserOnProject(int projectId)
        {
            return db.Project.Find(projectId).User;
        }

        public ICollection<ApplicationUser> UserNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id == projectId)).ToList();
        }

        public static string GetProjectNameById(int Id)
        {
            return db.Project.AsNoTracking().FirstOrDefault(p => p.Id == Id).Name;
        }

        public static string GetDisplayNameFromUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return db.Users.FirstOrDefault(u => u.Email == "unassed@mailinator.com").DisplayName;
            else
                return db.Users.Find(userId).DisplayName;
        }
    }
}