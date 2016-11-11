﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Author
    {
        public Author()
        {
            this.AuthorGroups = new HashSet<AuthorGroup>();
        }

        public int AuthorID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [NotMapped]
        public string FullName {
            get { return this.Surname + " " + this.Name; } }

        public virtual ICollection<AuthorGroup> AuthorGroups { get; set; }
    }
}