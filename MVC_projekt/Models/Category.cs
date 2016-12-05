using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources;

namespace MVC_projekt.Models
{
    public class Category
    {

        public int CategoryID { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Global))]
        public string Name { get; set; }

        public virtual ICollection<BookItem> BookItem { get; set; }

        public int ParentID { get; set; }
        public virtual Category Parent { get; set; }
    }
}