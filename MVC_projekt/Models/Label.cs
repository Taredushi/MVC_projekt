using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Label
    {

        public int LabelID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LabelGroup> LabelGroups { get; set; }
    }
}