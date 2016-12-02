using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace MVC_projekt.Models.Helpers
{
    public class CartTools
    {
        private List<BookItem> _booksList;
        private ApplicationDbContext db = new ApplicationDbContext();

        public void AddToCart(HttpContextBase context, int id)
        {
            if (context.Session?["cart"] != null)
            {
                _booksList = (List<BookItem>)context.Session["cart"];
                int index = _booksList.FindIndex(x => x.BookItemID == id);

                if (index != -1)
                {
                    var book = db.BookItems.Single(x => x.BookItemID == id);

                    _booksList.Add(book);
                    context.Session["cart"] = _booksList;
                }
            }
            else
            {
                var book = db.BookItems.Single(x => x.BookItemID == id);
                _booksList = new List<BookItem> {book};
                context.Session.Add("cart", _booksList);
            }
        }

        public void RemoveFromCart(HttpContextBase context, int id)
        {
            if (context.Session?["cart"] != null)
            {
                _booksList = (List<BookItem>)context.Session["cart"];
                var index = _booksList.FindIndex(x => x.BookItemID == id);

                if (index != -1)
                {
                    _booksList.RemoveAt(index);
                }
                context.Session["cart"] = _booksList;
            }
        }

        public void Order(HttpContextBase context)
        {
            if (context.Session?["cart"] != null)
            {
                _booksList = (List<BookItem>)context.Session["cart"];

                foreach (var book in _booksList)
                {
                    var order = new Order();
                    var userID = context.User.Identity.GetUserId();
                    order.ApplicationUserID = userID;
                    order.OrderDate = DateTime.Today;
                    order.ReturnDate = DateTime.Today - new TimeSpan(1);
                    order.Returned = false;
                    order.BookItemID = book.BookItemID;

                    db.Orders.Add(order);
                    db.SaveChanges();
                    context.Session["cart"] = null;
                }
            }
        }
    }
}