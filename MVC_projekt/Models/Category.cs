using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }

        public virtual BookItem BookItem { get; set; }
    }
}