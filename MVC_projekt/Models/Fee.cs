using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Resources;

namespace MVC_projekt.Models
{
    public class Fee
    {
        public int FeeID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date", ResourceType = typeof(Global))]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Amount", ResourceType = typeof(Global))]
        public double Amount { get; set; }

        public bool Paid { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}