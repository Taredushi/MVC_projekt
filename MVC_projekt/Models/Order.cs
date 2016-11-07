using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Book Book { get; set; }
    }
}