using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class SearchResult
    {
        public int SearchResultID { get; set; }

        public string URL { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser Account { get; set; }

        [NotMapped]
        public string SearchCategory
        {
            get
            {
                if (URL != null)
                {
                    var name = URL.Substring(URL.LastIndexOf('/')+1);
                    name = name.Remove(name.LastIndexOf('?'));
                    return name;
                }
                return "";
            }
        }
        [NotMapped]
        public string SearchString
        {
            get
            {
                if (URL != null)
                {
                    var name = URL.Substring(URL.LastIndexOf("SearchString=", StringComparison.Ordinal)+13);
                    name = name.Remove(name.LastIndexOf('&'));
                    return name;
                }
                return "";
            }
        }
    }
}