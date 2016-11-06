using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class Account
    {

        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }

        public virtual IEnumerable<Fee> Fees { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }
        public virtual IEnumerable<Booking> Bookings { get; set; }
        public virtual IEnumerable<SearchResult> SearchResults { get; set; }
    }
}