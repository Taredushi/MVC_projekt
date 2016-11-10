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
            SeedFees(context);
            SeedAuthor(context);
            SeedCategory(context);
            SeedBooks(context);
            SeedAuthorGroups(context);
            SeedLabels(context);
        }

        private void SeedRoles(MVC_projekt.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }
            if (!roleManager.RoleExists("Employee"))
            {
                roleManager.Create(new IdentityRole("Employee"));
            }
            if (!roleManager.RoleExists("User"))
            {
                roleManager.Create(new IdentityRole("User"));
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
                    UserName = "admin",
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
                    UserName = "bibliotekarz",
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
                    UserName = "user",
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

        private void SeedFees(MVC_projekt.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var fees = new List<Fee>()
            {
                new Fee()
                {
                    Amount = 10.50,
                    Date = new DateTime(2016, 10, 10),
                    Paid = false,
                    ApplicationUser = userManager.FindByName("user")
                },
                new Fee()
                {
                    Amount = 5,
                    Date = new DateTime(2016, 08, 10),
                    Paid = true,
                    ApplicationUser = userManager.FindByName("user")
                }
            };

            foreach (var fee in fees)
            {
                context.Set<Fee>().AddOrUpdate(fee);
            }
 
            context.SaveChanges();

        }

        private void SeedAuthor(MVC_projekt.Models.ApplicationDbContext context)
        {
            //dodanie do Author
            var authors = new List<Author>()
            {
                new Author()
                {
                    AuthorID = 1,
                    Name = "Andrzej",
                    Surname = "Pilipiuk"
                },
                new Author()
                {
                    AuthorID = 2,
                    Name = "Andrzej",
                    Surname = "Sapkowski",
                },
                new Author()
                {
                    AuthorID = 3,
                    Name = "Jeff",
                    Surname = "Grubb",
                },

            };

            foreach (var aut in authors)
            {
                context.Set<Author>().AddOrUpdate(aut);
            }

            context.SaveChanges();
        }

        private void SeedCategory(ApplicationDbContext context)
        {
            var parent = new Category(){ Name = "Fantastyka" };

            context.Set<Category>().AddOrUpdate(parent);
            context.SaveChanges();

            var categories = new List<Category>()
                {
                    new Category()
                    {
                        Name = "Zagraniczna",
                        Parent = context.Categories.Single(c=>c.Name == "Fantastyka")
                    },
                    new Category()
                    {
                        Name = "Polska",
                        Parent = context.Categories.Single(c=>c.Name == "Fantastyka")
                    }
                };

            foreach (var cat in categories)
            {
                context.Set<Category>().AddOrUpdate(cat);
            }

            context.SaveChanges();
        }

        private void SeedBooks(MVC_projekt.Models.ApplicationDbContext context)
        {
            var categoryPl = context.Set<Category>().FirstOrDefault(c => c.Name == "Polska").CategoryID;
            var categoryZ = context.Set<Category>().FirstOrDefault(c => c.Name == "Zagraniczna").CategoryID;

            //dodanie do BookItem
            var books = new List<BookItem>()
            {
                new BookItem()
                {
                    BookItemID = 1,
                    Title = "Wieszac kazdy moze",
                    Descryption = "\bWieszac kazdy moze" + " – piata z cyklu ksiazek Andrzeja Pilipiuka opowiadajacych o egzorcyscie amatorze, bimbrowniku, zamieszkujacym Stary Majdan zapadla wies na scianie wschodniej – Jakubie Wedrowyczu, wydana w 2006 roku.",
                    ISBN = 9788360505113,
                    Publisher = "Fabryka Slow",
                    ReleaseDate = 2016,
                    Category = context.Categories.FirstOrDefault(c=>c.CategoryID == categoryPl),
                },
                new BookItem()
                {
                    BookItemID = 2,
                    Title = "Pani jeziora",
                    Descryption = "\bPani Jeziora" + " – powiesc z gatunku fantasy, napisana przez Andrzeja Sapkowskiego, wydana w 1999. Jest ostatnia z pieciu czesci sagi o wiedzminie.",
                    ISBN = 8370541291,
                    Publisher = "SuperNowa",
                    ReleaseDate = 1999,
                    Category = context.Categories.FirstOrDefault(c=>c.CategoryID == categoryPl)
                },
                new BookItem()
                {
                    BookItemID = 3,
                    Title = "Starcraft. Krucjata Liberty'ego",
                    Descryption = "W odleglej przyszlosci, 60 000 lat swietlnych od Ziemi, luzna konfederacja ziemskich wygnancow zmaga sie z zagadkowymi Protossami i bezwzglednym Rojem Zergow. Kazdy gatunek walczy o przetrwanie posrod gwiazd, w wojnie, ktora wiesci poczatek najlepszego rozdzialu w historii ludzkosci - albo przepowiada jej gwaltowny, krwawy koniec.\n" 
                    + "Danny Liberty byl dobrym reporterem... zbyt dobrym. Kiedy jego sledztwo zaprowadzilo go zbyt blisko serca skorumpowanej Konfederacji Czlowieka, zmuszony byl dokonac prostego wyboru: kontynuowac publikowanie dotychczasowych reportazy, badz tez przyjac nowe, ryzykowne zadanie towarzyszenia Marines na pierwszej linii w Sektorze Koprulu. Podjecie decyzji trwalo krotko...\n" 
                    + "Za atakami Zergow i Protossow kryje sie dluga historia, ale kazdy kawalek informacji jeszcze bardziej gmatwa tajemnice. Wrzucony w sam srodek wojny, od ktorej wyniku bedzie zalezal los ludzkosci, Danny Liberty pewien jest jedynie tego, ze chcac przezyc, ufac moze tylko sobie...",
                    ISBN = 83705412911,
                    Publisher = "Wydawnictwo Isa",
                    ReleaseDate = 2001,
                    Category = context.Categories.FirstOrDefault(c=>c.CategoryID == categoryZ)
                }
            };

            foreach (var book in books)
            {
                context.Set<BookItem>().AddOrUpdate(book);
            }

            context.SaveChanges();
        }

        private void SeedAuthorGroups(ApplicationDbContext context)
        {
            //dodanie do AuthorGroup
            for (int i = 1; i < 4; i++)
            {
                var ag = new AuthorGroup()
                {
                    AuthorGroupID = i,
                    Author = context.Authors.Single(a => a.AuthorID == i),
                    BookItem = context.BookItems.Single(a => a.BookItemID == i)
                };
                context.Set<AuthorGroup>().AddOrUpdate(ag);
            }
            context.SaveChanges();
        }

        private void SeedLabels(ApplicationDbContext context)
        {
            for (int i = 1; i < 4; i++)
            {
                var ag = new Label()
                {
                    LabelID = i,
                    Name = "Label" + i
                };
                context.Set<Label>().AddOrUpdate(ag);
            }
            context.SaveChanges();
        }


    }
}

