using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Resources;

namespace MVC_projekt.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        [Required(ErrorMessageResourceName = "FirstNameValidationError", ErrorMessageResourceType = typeof(Global))]
        [Display(Name = "FirstName", ResourceType = typeof(Global))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "SurnameValidationError", ErrorMessageResourceType = typeof(Global))]
        [Display(Name = "Surname", ResourceType = typeof(Global))]
        public string Surname { get; set; }

        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<SearchResult> SearchResults { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


}