using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Quartz;

namespace MVC_projekt.Models.Classes
{
    public class ReturnRemainderEmail : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            DateTime time = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            var date = time.AddDays(1);

            var orders =
                db.Orders.Where(x => x.AvailableOn.ToShortDateString().Equals(date.ToShortDateString()) && !x.Returned)
                    .GroupBy(x => x.ApplicationUser);

            foreach (var o in orders)
            {
                EmailService es = new EmailService();
                IdentityMessage im = new IdentityMessage();
                im.Destination = o.Key.Email;
                im.Subject = "Return Book Date";
                im.Body = "Dzień zwrotu upływa " + date.ToShortDateString() + " dla książek: \n";
                foreach (var arg in o)
                {
                    foreach (var author in arg.BookItem.AuthorGroups)
                    {
                        im.Body += author.Author.FullName + " ";
                    }

                    im.Body += arg.BookItem.Title + "\n";
                }
                es.SendAsync(im);
            }
        }
    }
}