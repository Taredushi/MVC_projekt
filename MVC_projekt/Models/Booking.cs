﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public DateTime Date { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int BookItemID { get; set; }
        public virtual BookItem BookItem { get; set; }
    }
}