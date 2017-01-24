using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models.View
{
    public class CategoryViewModel
    {
        public int ID { get; set; }
        public string text { get; set; }
        public int[] tags = new int[1];
        public string href { get; set; }
        public List<CategoryViewModel> nodes { get; set; }
    }

    public class CategoryViewModelCreate
    {
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }

        public int? ParentID { get; set; }
    }
}