using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Resources;

namespace MVC_projekt.Models
{
    public class Author
    {

        public int AuthorID { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Global))]
        public string Name { get; set; }

        [Display(Name = "Surname", ResourceType = typeof(Global))]
        public string Surname { get; set; }

        [NotMapped]
        [Display(Name = "Author", ResourceType = typeof(Global))]
        public string FullName {
            get { return this.Surname + " " + this.Name; } }

        public virtual ICollection<AuthorGroup> AuthorGroups { get; set; }
    }
}