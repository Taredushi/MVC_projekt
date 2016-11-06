using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class LabelGroup
    {
        public int LabelGroupID { get; set; }

        public virtual BookItem BookItem { get; set; }
        public virtual Label Label { get; set; }
    }
}