using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public string Title { get; set; }

        [Required]
        public long ISBN { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public int ReleaseDate { get; set; }

        [Required]
        public int Amount { get; set; }

        public int AvailableNumber { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Display(Name = "Category")]
        public Category Category { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descryption { get; set; }

        [Required]
        [Display(Name = "Author")]
        public List<int> SelectedAuthors { get; set; }

        [Required]
        [Display(Name = "Label")]
        public List<int> SelectedLabels { get; set; }

        [Display(Name = "Author")]
        public List<Author> Authors { get; set; }

        [Display(Name = "Label")]
        public List<Label> Labels { get; set; }

    }
}