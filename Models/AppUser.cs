using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace RunGroopWebApp.Models
{
    public class AppUser: IdentityUser 
    {
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        [ForeignKey("FK_AppUser_Address")]
        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }

        //collection
        public ICollection<Club> Clubs { get; set; } 
        public ICollection<Race> Races { get; set; }    

    }
}
