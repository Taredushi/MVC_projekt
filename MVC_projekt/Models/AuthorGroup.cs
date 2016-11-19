using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class AuthorGroup
    {
        public int AuthorGroupID { get; set; }
        
        public int AuthorID { get; set; }
        public virtual Author Author { get; set; }
        

        public int BookItemID { get; set; }
        public virtual BookItem BookItem { get; set; }
    }
}