using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Book
    {
        public int BookID { get; set; }

        public virtual BookItem BookItem { get; set; }
        public virtual IEnumerable<Booking> Bookings { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }
    }
}