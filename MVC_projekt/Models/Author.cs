using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Author
    {

        public int AuthorID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<AuthorGroup> AuthorGroups { get; set; }
    }
}