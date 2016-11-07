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
        public DateTime Date { get; set; }

        [Display(Name = "Kwota")]
        public double Amount { get; set; }

        [Display(Name = "Stan")]
        public bool Paid { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}