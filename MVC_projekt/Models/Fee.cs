using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MVC_projekt.Models
{
    public class Fee
    {
        public int FeeID { get; set; }

        [Display(Name = "Data wystawienia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [Display(Name = "Kwota")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public double Amount { get; set; }

        [Display(Name = "Stan")]
        public bool Paid { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}