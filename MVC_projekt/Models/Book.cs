using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Book
    {
        public Book()
        {
            this.Bookings = new HashSet<Booking>();
            this.Orders = new HashSet<Order>();
        }
        public int BookID { get; set; }
        public bool Returned { get; set; }

        public virtual BookItem BookItem { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}