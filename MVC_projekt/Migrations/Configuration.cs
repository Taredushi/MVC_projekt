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
                    Bookings = new List<Booking>(),
                    Orders = new List<Order>(),
                    Fees = new List<Fee>()
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
                    Bookings = new List<Booking>(),
                    Orders = new List<Order>(),
                    Fees = new List<Fee>()
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
                    Bookings = new List<Booking>(),
                    Orders = new List<Order>(),
                    Fees = new List<Fee>()
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
                    ApplicationUser = userManager.FindByEmail("user@user.net")
                },
                new Fee()
                {
                    Amount = 5,
                    Date = new DateTime(2016, 08, 10),
                    Paid = true,
                    ApplicationUser = userManager.FindByEmail("user@user.net")
                }
            };
            if (!context.Fees.Any())
            {
                context.Fees.AddRange(fees);
                context.SaveChanges();
                var user = userManager.FindByEmail("user@user.net");
                user.Fees.Add(fees[0]);
                user.Fees.Add(fees[1]);
            }
 
            context.SaveChanges();

        }

        private void SeedBooks(MVC_projekt.Models.ApplicationDbContext context)
        {
            //dodanie do BookItem
            var books = new List<BookItem>()
            {
                new BookItem()
                {
                    Title = "Wieszaæ ka¿dy mo¿e",
                    Descryption = "\bWieszaæ ka¿dy mo¿e" + " – pi¹ta z cyklu ksi¹¿ek Andrzeja Pilipiuka opowiadaj¹cych o egzorcyœcie amatorze, bimbrowniku, zamieszkuj¹cym Stary Majdan zapad³¹ wieœ na œcianie wschodniej – Jakubie Wêdrowyczu, wydana w 2006 roku.",
                    ISBN = 9788360505113,
                    Publisher = "Fabryka S³ów",
                    ReleaseDate = 2016,
                    Category = context.Categories.ToList().Find(x=>x.Name.Equals("Polska")),
                    Attachments = new List<Attachment>(),
                    AuthorGroups = new List<AuthorGroup>(),
                    Books = new List<Book>(),
                    LabelGroups = new List<LabelGroup>()
                },
                new BookItem()
                {
                    Title = "Pani jeziora",
                    Descryption = "\bPani Jeziora" + " – powieœæ z gatunku fantasy, napisana przez Andrzeja Sapkowskiego, wydana w 1999. Jest ostatni¹ z piêciu czêœci sagi o wiedŸminie.",
                    ISBN = 8370541291,
                    Publisher = "SuperNowa",
                    ReleaseDate = 1999,
                    Category = context.Categories.ToList().Find(x=>x.Name.Equals("Polska")),
                    Attachments = new List<Attachment>(),
                    AuthorGroups = new List<AuthorGroup>(),
                    Books = new List<Book>(),
                    LabelGroups = new List<LabelGroup>()
                },
                new BookItem()
                {
                    Title = "Starcraft. Krucjata Liberty'ego",
                    Descryption = "W odleg³ej przysz³oœci, 60 000 lat œwietlnych od Ziemi, luŸna konfederacja ziemskich wygnañców zmaga siê z zagadkowymi Protossami i bezwzglêdnym Rojem Zergów. Ka¿dy gatunek walczy o przetrwanie poœród gwiazd, w wojnie, która wieœci pocz¹tek najlepszego rozdzia³u w historii ludzkoœci - albo przepowiada jej gwa³towny, krwawy koniec.\n" 
                    + "Danny Liberty by³ dobrym reporterem... zbyt dobrym. Kiedy jego œledztwo zaprowadzi³o go zbyt blisko serca skorumpowanej Konfederacji Cz³owieka, zmuszony by³ dokonaæ prostego wyboru: kontynuowaæ publikowanie dotychczasowych reporta¿y, b¹dŸ te¿ przyj¹æ nowe, ryzykowne zadanie towarzyszenia Marines na pierwszej linii w Sektorze Koprulu. Podjêcie decyzji trwa³o krótko...\n" 
                    + "Za atakami Zergów i Protossów kryje siê d³uga historia, ale ka¿dy kawa³ek informacji jeszcze bardziej gmatwa tajemnicê. Wrzucony w sam œrodek wojny, od której wyniku bêdzie zale¿a³ los ludzkoœci, Danny Liberty pewien jest jedynie tego, ¿e chc¹c prze¿yæ, ufaæ mo¿e tylko sobie...",
                    ISBN = 83705412911,
                    Publisher = "Wydawnictwo Isa",
                    ReleaseDate = 2001,
                    Category = context.Categories.ToList().Find(x=>x.Name.Equals("Zagraniczna")),
                    Attachments = new List<Attachment>(),
                    AuthorGroups = new List<AuthorGroup>(),
                    Books = new List<Book>(),
                    LabelGroups = new List<LabelGroup>()
                }
            };
            if (!context.BookItems.Any())
            {
                context.BookItems.AddRange(books);
                context.SaveChanges();

                context.Categories.ToList().Find(x => x.Name.Equals("Polska")).BookItem.Add(books[0]);
                context.Categories.ToList().Find(x => x.Name.Equals("Polska")).BookItem.Add(books[1]);
                context.Categories.ToList().Find(x => x.Name.Equals("Zagraniczna")).BookItem.Add(books[2]);
                context.SaveChanges();
            }
        }

        private void SeedAuthorGroups(ApplicationDbContext context)
        {
            //dodanie do AuthorGroup
            var authorGroup = new List<AuthorGroup>()
            {
                new AuthorGroup()
                {
                    Author = context.Authors.ToList().Find(x => x.Surname.Equals("Pilipiuk")),
                    BookItem =
                        context.BookItems.ToList().Find(x => x.ISBN == 9788360505113)
                },
                new AuthorGroup()
                {
                    Author = context.Authors.Find(context.Authors.ToList().FindIndex(x => x.Surname.Equals("Sapkowski"))),
                    BookItem = context.BookItems.ToList().Find(x => x.ISBN == 8370541291)
                },
                new AuthorGroup()
                {
                    Author = context.Authors.Find(context.Authors.ToList().FindIndex(x => x.Surname.Equals("Grubb"))),
                    BookItem =
                        context.BookItems.ToList().Find(x => x.ISBN == 83705412911)
                }
            };

            if (!context.AuthorGroups.Any())
            {
                context.AuthorGroups.AddRange(authorGroup);
                context.SaveChanges();

                //dodanie listy grup do Author
                foreach (var author in context.Authors)
                {
                    var groups = authorGroup.FindAll(x => x.Author.AuthorID == author.AuthorID);
                    foreach (var arg in groups)
                    {
                        if (!author.AuthorGroups.Contains(arg))
                        {
                            author.AuthorGroups.Add(arg);
                        }
                    }
                }
                context.SaveChanges();

                //dodanie listy grup do BookItem
                foreach (var book in context.BookItems)
                {
                    var groups = authorGroup.FindAll(x => x.BookItem.BookItemID == book.BookItemID);
                    foreach (var arg in groups)
                    {
                        if (!book.AuthorGroups.Contains(arg))
                        {
                            book.AuthorGroups.Add(arg);
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        private void SeedCategory(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                //dodanie do Category
                context.Categories.Add(new Category()
                {
                    Name = "Fantastyka",
                    BookItem = new List<BookItem>()
                });
                context.SaveChanges();

                
            }
            if (context.Categories.Count() == 1)
            {
                var categories = new List<Category>()
                {
                    new Category()
                    {
                        Name = "Zagraniczna",
                        Parent = context.Categories.Find(1),
                        BookItem = new List<BookItem>()
                    },
                    new Category()
                    {
                        Name = "Polska",
                        Parent = context.Categories.Find(1),
                        BookItem = new List<BookItem>()
                    }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }

        private void SeedAuthor(MVC_projekt.Models.ApplicationDbContext context)
        {
            //dodanie do Author
            var authors = new List<Author>()
            {
                new Author()
                {
                    Name = "Andrzej",
                    Surname = "Pilipiuk"
                },
                new Author()
                {
                    Name = "Andrzej",
                    Surname = "Sapkowski"
                },
                new Author()
                {
                    Name = "Jeff",
                    Surname = "Grubb"
                },

            };
            if (!context.Authors.Any())
            {
                context.Authors.AddRange(authors);
                context.SaveChanges();
            }
        }
    }
}

