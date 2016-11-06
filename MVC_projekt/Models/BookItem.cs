using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class BookItem
    {
        public int BookItemID { get; set; }
        public string Title { get; set; }
        [Index(IsUnique = true)]
        public int ISBN { get; set; }
        public string Descryption { get; set; }

        public virtual Category Category { get; set; }
        public virtual IEnumerable<Book> Books { get; set; }
        public virtual IEnumerable<Attachment> Attachments { get; set; }
        public virtual IEnumerable<LabelGroup> LabelGroups { get; set; }
        public virtual IEnumerable<AuthorGroup> AuthorGroups { get; set; }

    }
}