using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Return Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReturnDate { get; set; }

        [Display(Name="Available On")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AvailableOn { get; set; }

        public bool Returned { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int BookItemID { get; set; }
        public virtual BookItem BookItem { get; set; }
    }
}