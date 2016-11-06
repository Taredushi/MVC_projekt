using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Attachment
    {

        public int AttachmentID { get; set; }
        public string Source { get; set; }
        public string Descryption { get; set; }

        public virtual BookItem BookItem { get; set; }
    }
}