using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models.View
{
    public enum AdvancedSearchOption
    {
        And,
        Or
    }

    public class AdvancedSearchViewModel
    {
        public string OptionOne { get; set; }
        public string ValueOne { get; set; }

        public string OptionTwo { get; set; }
        public string ValueTwo { get; set; }

        public string OptionThree { get; set; }
        public string ValueThree { get; set; }

        public string OptionFour { get; set; }
        public string ValueFour { get; set; }

        public AdvancedSearchOption SearchOption { get; set; }
    }
}