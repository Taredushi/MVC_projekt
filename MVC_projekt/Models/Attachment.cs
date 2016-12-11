using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources;

namespace MVC_projekt.Models
{
    public class Attachment
    {

        public int AttachmentID { get; set; }

        public string FileName { get; set; }

        public string Source { get; set; }

        [Display(Name = "Descryption", ResourceType = typeof(Global))]
        public string Descryption { get; set; }

        public FileType FileType { get; set; }

        public int BookItemID { get; set; }
        public virtual BookItem BookItem { get; set; }
    }

    public enum FileType
    {
        Cover = 1,
        TableOfContents,
        Attachment
    }
}