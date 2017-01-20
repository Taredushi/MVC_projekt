using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Quartz;

namespace MVC_projekt.Models.Classes
{
    public class ReturnDelayEmail : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            DateTime time = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            var orders =
                db.Orders.Where(x => (time.Subtract(x.AvailableOn) > TimeSpan.Zero ) && !x.Returned)
                    .GroupBy(x => x.ApplicationUser);

            foreach (var o in orders)
            {
                EmailService es = new EmailService();
                IdentityMessage im = new IdentityMessage();
                im.Destination = o.Key.Email;
                im.Subject = "Return Book Date";
                im.Body = "Dzień zwrotu upłynął dla książek: \n";
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