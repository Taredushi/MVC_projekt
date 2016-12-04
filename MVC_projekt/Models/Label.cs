using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources;

namespace MVC_projekt.Models
{
    public class Label
    {

        public int LabelID { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Global))]
        public string Name { get; set; }

        public virtual ICollection<LabelGroup> LabelGroups { get; set; }
    }
}