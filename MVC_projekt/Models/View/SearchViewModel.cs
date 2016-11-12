using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class SearchViewModel
    {

        public List<int> FoundBooks { get; set; }

        public SearchViewModel()
        {
            FoundBooks = new List<int>();
        }
    }
}