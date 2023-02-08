namespace BugBully.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.BugBullyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.BugBullyContext context)
        {
            if (!context.Statuses.Any(b => b.Name == "Backlog"))
            {
                context.Statuses.AddOrUpdate(
                    b => b.Name,
                    new Models.Statuses { Id = 1, Name = "Backlog" },
                    new Models.Statuses { Id = 2, Name = "Ready for Review" },
                    new Models.Statuses { Id = 3, Name = "In Review" },
                    new Models.Statuses { Id = 4, Name = "Ready for Development" },
                    new Models.Statuses { Id = 5, Name = "In Development" },
                    new Models.Statuses { Id = 6, Name = "Ready for Code Review" },
                    new Models.Statuses { Id = 7, Name = "In Code Review" },
                    new Models.Statuses { Id = 8, Name = "Ready for QA" },
                    new Models.Statuses { Id = 9, Name = "In QA" },
                    new Models.Statuses { Id = 10, Name = "Ready for Acceptance" },
                    new Models.Statuses { Id = 11, Name = "In Acceptance" }
                );
            }

            if (!context.Users.Any(b => b.Username == "johnsmithdev"))
            {
                context.Users.AddOrUpdate(
                    u => u.Username,
                    new Models.Users { Id = 1, Username = "johnsmithdev", Password = "password1", Name = "John Smith" },
                    new Models.Users { Id = 2, Username = "janedoedev", Password = "password2", Name = "Jane Doe" }
                );
            }

            if (!context.Bugs.Any(b => b.Title == "Error when logging in"))
            {
                context.Bugs.AddOrUpdate(
                    b => b.Title,
                    new Models.Bugs
                    {
                        Id = 1,
                        Title = "Error when logging in",
                        Description = "When I try to log in, I get an error message.",
                        DateReported = DateTime.Now,
                        UserId = 1,
                        StatusId = 3
                    },
                    new Models.Bugs
                    {
                        Id = 2,
                        Title = "Broken link on homepage",
                        Description = "The link to the about page on the homepage is broken.",
                        DateReported = DateTime.Now,
                        UserId = 2,
                        StatusId = 1
                    }
                );
            }
        }
    }
}