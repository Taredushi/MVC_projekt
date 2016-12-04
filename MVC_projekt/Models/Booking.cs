using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources;

namespace MVC_projekt.Models
{
    public class Booking
    {
        public int BookingID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date", ResourceType = typeof(Global))]
        public DateTime Date { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int BookItemID { get; set; }
        public virtual BookItem BookItem { get; set; }
    }
}