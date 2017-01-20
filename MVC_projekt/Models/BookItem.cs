using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Microsoft.AspNet.Identity;
using Resources;

namespace MVC_projekt.Models
{
    public class BookItem
    {

        [Key]
        public int BookItemID { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Global))]
        public string Title { get; set; }

        [Index(IsUnique = true)]
        [Display(Name = "ISBN", ResourceType = typeof(Global))]
        public long ISBN { get; set; }

        [Display(Name = "Number", ResourceType = typeof(Global))]
        public int Number { get; set; }

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

        [Display(Name = "Publisher", ResourceType = typeof(Global))]
        public string Publisher { get; set; }

        [Display(Name = "Released", ResourceType = typeof(Global))]
        public int ReleaseDate { get; set; }

        [Display(Name = "AddDate", ResourceType = typeof(Global))]
        public DateTime AddDate { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Global))]
        public int CategoryID { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Global))]
        public virtual Category Category { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<LabelGroup> LabelGroups { get; set; }
        public virtual ICollection<AuthorGroup> AuthorGroups { get; set; }

    }
}