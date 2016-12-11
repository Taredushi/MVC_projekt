using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models.Classes
{
    public class AttachmentFile
    {
        public HttpPostedFileBase File { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descryption { get; set; }
    }
}