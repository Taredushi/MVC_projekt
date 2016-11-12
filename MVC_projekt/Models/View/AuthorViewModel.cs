using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class AuthorViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }
        public int BooksNumber { get; set; }
    }

}