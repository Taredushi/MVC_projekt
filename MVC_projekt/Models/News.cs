using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class News
    {
        [Key]
        public int NewsID { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Global))]
        public string Title { get; set; }

        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descryption", ResourceType = typeof(Global))]
        public string Descryption { get; set; }

        [NotMapped]
        [Display(Name = "Descryption", ResourceType = typeof(Global))]
        public string ShortDescryption
        {
            get
            {
                var text = Descryption.Length > 150 ? Descryption.Substring(0, 150) + " [...]" : Descryption;
                return text;
            }
        }

        [Display(Name = "AddDate", ResourceType = typeof(Global))]
        public DateTime AddDate { get; set; }
    }
}