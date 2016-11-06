using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MVC_projekt.Models
{
    public class Fee
    {
        public int FeeID { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool Paid { get; set; }

        public virtual Account Account { get; set; }

    }
}