using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public DateTime Date { get; set; }

        public virtual ApplicationUser Account { get; set; }
        public virtual Book Book { get; set; }
    }
}