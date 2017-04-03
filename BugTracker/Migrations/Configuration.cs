namespace BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            if (!context.Roles.Any(r => r.Name == "Unassigned"))
            {
                roleManager.Create(new IdentityRole { Name = "Unassigned" });
            }


            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "jtisdale1977@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jtisdale1977@gmail.com",
                    Email = "jtisdale1977@gmail.com",
                    FirstName = "Justin",
                    LastName = "Tisdale",
                    DisplayName = "Justin Tisdale"
                }, "Abc123!");
            }

            if (!context.Users.Any(u => u.Email == "jtwichell@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jtwichell@mailinator.com",
                    Email = "jtwichell@mailinator.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "Jason Twichell"
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "mjaang@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "mjaang@mailinator.com",
                    Email = "mjaang@mailinator.com",
                    FirstName = "Mark",
                    LastName = "Jaang",
                    DisplayName = "Mark Jaang"
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "araynor@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "araynor@mailinator.com",
                    Email = "araynor@mailinator.com",
                    FirstName = "Antonio",
                    LastName = "Raynor",
                    DisplayName = "Antonio Raynor"
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "unassed@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "unassed@mailinator.com",
                    Email = "unassed@mailinator.com",
                    FirstName = "Unassigned",
                    LastName = "Unassigned",
                    DisplayName = "Unassigned"
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "admindemo@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "admindemo@mailinator.com",
                    Email = "admindemo@mailinator.com",
                    FirstName = "Admin",
                    LastName = "Demo",
                    DisplayName = "Admin Demo"
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "prodmandemo@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "prodmandemo@mailinator.com",
                    Email = "prodmandemo@mailinator.com",
                    FirstName = "Project Manager",
                    LastName = "Demo",
                    DisplayName = "Project Manager Demo"
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "develdemo@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "develdemo@mailinator.com",
                    Email = "develdemo@mailinator.com",
                    FirstName = "Developer",
                    LastName = "Demo",
                    DisplayName = "Developer Demo"
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "submitdemo@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "submitdemo@mailinator.com",
                    Email = "submitdemo@mailinator.com",
                    FirstName = "Submitter",
                    LastName = "Demo",
                    DisplayName = "Submitter Demo"
                }, "Abc&123!");
            }

            
            var Admin = userManager.FindByEmail("jtisdale1977@gmail.com").Id;
            userManager.AddToRole(Admin, "Admin");

            var ProdMan = userManager.FindByEmail("jtwichell@mailinator.com").Id;
            userManager.AddToRole(ProdMan, "Project Manager");

            var Devel = userManager.FindByEmail("mjaang@mailinator.com").Id;
            userManager.AddToRole(Devel, "Developer");

            var Submit = userManager.FindByEmail("araynor@mailinator.com").Id;
            userManager.AddToRole(Submit, "Submitter");

            var Unassigned = userManager.FindByEmail("unassed@mailinator.com").Id;
            userManager.AddToRole(Unassigned, "Unassigned");

            var AdminDemo = userManager.FindByEmail("admindemo@mailinator.com").Id;
            userManager.AddToRole(AdminDemo, "Admin");

            var ProdManDemo = userManager.FindByEmail("prodmandemo@mailinator.com").Id;
            userManager.AddToRole(ProdManDemo, "Project Manager");

            var DevelDemo = userManager.FindByEmail("develdemo@mailinator.com").Id;
            userManager.AddToRole(DevelDemo, "Developer");

            var SubmitDemo = userManager.FindByEmail("submitdemo@mailinator.com").Id;
            userManager.AddToRole(SubmitDemo, "Submitter");


            if (!context.TicketPriority.Any(u => u.Name == "Emergency"))
            { context.TicketPriority.Add(new TicketPriorities { Name = "Emergency" }); }

            if (!context.TicketPriority.Any(u => u.Name == "High"))
            { context.TicketPriority.Add(new TicketPriorities { Name = "High" }); }

            if (!context.TicketPriority.Any(u => u.Name == "Medium"))
            { context.TicketPriority.Add(new TicketPriorities { Name = "Medium" }); }

            if (!context.TicketPriority.Any(u => u.Name == "Low"))
            { context.TicketPriority.Add(new TicketPriorities { Name = "Low" }); }

            if (!context.TicketPriority.Any(u => u.Name == "Urgent"))
            { context.TicketPriority.Add(new TicketPriorities { Name = "Urgent" }); }

            if (!context.TicketType.Any(u => u.Name == "Bug"))
            { context.TicketType.Add(new TicketTypes { Name = "Bug" }); }

            if (!context.TicketType.Any(u => u.Name == "Technical Issue"))
            { context.TicketType.Add(new TicketTypes { Name = "Technical Issue" }); }

            if (!context.TicketType.Any(u => u.Name == "Production Fix"))
            { context.TicketType.Add(new TicketTypes { Name = "Production Fix" }); }

            if (!context.TicketType.Any(u => u.Name == "Project Task"))
            { context.TicketType.Add(new TicketTypes { Name = "Project Task" }); }

            if (!context.TicketType.Any(u => u.Name == "Software Issue"))
            { context.TicketType.Add(new TicketTypes { Name = "Software Issue" }); }

            if (!context.TicketType.Any(u => u.Name == "Software Update"))
            { context.TicketType.Add(new TicketTypes { Name = "Software Update" }); }

            if (!context.TicketType.Any(u => u.Name == "Hardware Issue"))
            { context.TicketType.Add(new TicketTypes { Name = "Harware Issue" }); }

            if (!context.TicketType.Any(u => u.Name == "General Question"))
            { context.TicketType.Add(new TicketTypes { Name = "General Question" }); }

            if (!context.TicketStatus.Any(u => u.Name == "New"))
            { context.TicketStatus.Add(new TicketStatuses { Name = "New" }); }

            if (!context.TicketStatus.Any(u => u.Name == "Assigned"))
            { context.TicketStatus.Add(new TicketStatuses { Name = "Assigned" }); }

            if (!context.TicketStatus.Any(u => u.Name == "Active"))
            { context.TicketStatus.Add(new TicketStatuses { Name = "Active" }); }

            if (!context.TicketStatus.Any(u => u.Name == "On Hold"))
            { context.TicketStatus.Add(new TicketStatuses { Name = "On Hold" }); }

            if (!context.TicketStatus.Any(u => u.Name == "Resolved"))
            { context.TicketStatus.Add(new TicketStatuses { Name = "Resolved" }); }

            if (!context.TicketStatus.Any(u => u.Name == "Closed"))
            { context.TicketStatus.Add(new TicketStatuses { Name = "Closed" }); }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
