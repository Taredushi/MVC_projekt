using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Migrations.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_projekt.Models;

namespace MVC_projekt.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVC_projekt.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVC_projekt.Models.ApplicationDbContext context)
        {
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

            SeedRoles(context);
            SeedUsers(context);
        }

        private void SeedRoles(MVC_projekt.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var adminrole = roleManager.FindByName("Admin");
            if (adminrole == null)
            {
                adminrole = new IdentityRole("Admin");
                roleManager.Create(adminrole);
            }
            var employeerole = roleManager.FindByName("Employee");
            if (employeerole == null)
            {
                employeerole = new IdentityRole("Employee");
                roleManager.Create(employeerole);
            }
            var userrole = roleManager.FindByName("User");
            if (userrole == null)
            {
                userrole = new IdentityRole("User");
                roleManager.Create(userrole);
            }
        }

        private void SeedUsers(MVC_projekt.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var passwordHash = new PasswordHasher();

            if (userManager.FindByEmail("admin@admin.net") == null)
            {
                var user = new ApplicationUser
                {
                    Name = "Jan",
                    Surname = "Kowalski",
                    UserName = "admin@admin.net",
                    Email = "admin@admin.net",
                    PasswordHash = passwordHash.HashPassword("admin"),
                };

                var result = userManager.Create(user);
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }


            if (userManager.FindByEmail("bibliotekarz@bibliotekarz.net") == null)
            {
                var user = new ApplicationUser
                {
                    Name = "Jan",
                    Surname = "Nowak",
                    UserName = "bibliotekarz@bibliotekarz.net",
                    Email = "bibliotekarz@bibliotekarz.net",
                    PasswordHash = passwordHash.HashPassword("bibliotekarz"),
                };

                var result = userManager.Create(user);
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Employee");
                }
            }


            if (userManager.FindByEmail("user@user.net") == null)
            {
                var user = new ApplicationUser
                {
                    Name = "Jan",
                    Surname = "Zegarek",
                    UserName = "user@user.net",
                    Email = "user@user.net",
                    PasswordHash = passwordHash.HashPassword("user"),
                };

                var result = userManager.Create(user);
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "User");
                }
            }
        }
    }
}
