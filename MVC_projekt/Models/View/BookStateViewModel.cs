using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources;

namespace MVC_projekt.Models.View
{
    public class BookStateViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Global))]
        public string Title { get; set; }

        [Display(Name = "Author", ResourceType = typeof(Global))]
        public List<Author> Authors { get; set; }

        [Display(Name = "OrderDate", ResourceType = typeof(Global))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "ReturnDate", ResourceType = typeof(Global))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Expire", ResourceType = typeof(Global))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AvailableOn { get; set; }

        [Display(Name = "Returned", ResourceType = typeof(Global))]
        public bool Returned { get; set; }

    }
}