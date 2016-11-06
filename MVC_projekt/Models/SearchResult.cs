using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class SearchResult
    {
        public int SearchResultID { get; set; }

        public virtual BookItem BookItem { get; set; }
        public virtual ApplicationUser Account { get; set; }
    }
}