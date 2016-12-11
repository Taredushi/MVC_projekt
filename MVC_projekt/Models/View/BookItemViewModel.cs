using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_projekt.Models.Classes;
using Resources;

namespace MVC_projekt.Models
{
    public class BookItemViewModel
    {
        public BookItemViewModel()
        {
            Authors = new List<Author>();
            Labels = new List<Label>();
        }

        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Title", ResourceType = typeof(Global))]
        public string Title { get; set; }

        [Required]
        [Display(Name = "ISBN", ResourceType = typeof(Global))]
        public long ISBN { get; set; }

        [Required]
        [Display(Name = "Publisher", ResourceType = typeof(Global))]
        public string Publisher { get; set; }

        [Required]
        [Display(Name = "Released", ResourceType = typeof(Global))]
        public int ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Number", ResourceType = typeof(Global))]
        public int Number { get; set; }

        public int AvailableNumber { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Global))]
        public Category Category { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descryption { get; set; }

        [Required]
        [Display(Name = "Author", ResourceType = typeof(Global))]
        public List<int> SelectedAuthors { get; set; }

        [Required]
        [Display(Name = "Label", ResourceType = typeof(Global))]
        public List<int> SelectedLabels { get; set; }

        [Display(Name = "Author", ResourceType = typeof(Global))]
        public List<Author> Authors { get; set; }

        [Display(Name = "Label", ResourceType = typeof(Global))]
        public List<Label> Labels { get; set; }


        public List<AttachmentFile> FileList { get; set; }

        public string PreviousPage { get; set; }

    }
}