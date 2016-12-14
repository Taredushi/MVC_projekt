using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MVC_projekt.Models.View;

namespace MVC_projekt.Models.Helpers
{
    public class BookTools
    {
        public List<BookStateViewModel> GetOrderedBook(ApplicationDbContext db, string userId)
        {
            var orders = db.Orders.Where(x => !x.Returned && x.ApplicationUserID.Equals(userId));
            List<BookStateViewModel> list = new List<BookStateViewModel>();

            foreach (var order in orders)
            {
                list.Add(GetBookStateViewModel(order));
            }

            return list;
        }

        public List<BookStateViewModel> GetBookHistory(ApplicationDbContext db, string userId)
        {
            var orders = db.Orders.Where(x => x.Returned && x.ApplicationUserID.Equals(userId));
            List<BookStateViewModel> list = new List<BookStateViewModel>();

            foreach (var order in orders)
            {
                list.Add(GetBookStateViewModel(order));
            }

            return list;
        }

        private BookStateViewModel GetBookStateViewModel(Order order)
        {
            var bsvm = new BookStateViewModel()
            {
                Title = order.BookItem.Title,
                Authors = order.BookItem.AuthorGroups.Select(x=>x.Author).ToList(),
                Returned = order.Returned,
                AvailableOn = order.AvailableOn,
                OrderDate = order.OrderDate,
                ID = order.BookItemID,
                ReturnDate = order.ReturnDate
            };

            return bsvm;
        }
    }
}