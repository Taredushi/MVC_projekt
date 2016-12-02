using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Microsoft.AspNet.Identity;

namespace MVC_projekt.Models
{
    public class BookItem
    {

        [Key]
        public int BookItemID { get; set; }
        public string Title { get; set; }
        [Index(IsUnique = true)]
        public long ISBN { get; set; }
        public int Amount { get; set; }
        public string Descryption { get; set; }
        public string Publisher { get; set; }
        [Display(Name = "Released")]
        public int ReleaseDate { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<LabelGroup> LabelGroups { get; set; }
        public virtual ICollection<AuthorGroup> AuthorGroups { get; set; }

    }
}