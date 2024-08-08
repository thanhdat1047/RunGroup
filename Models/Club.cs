using RunGroopWebApp.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroopWebApp.Models
{
    public class Club
    {
        [Key]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public ClubCategory ClubCategory { get; set; }

        [ForeignKey("FK_CLub_Address")]
        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }

        [ForeignKey("FK_CLub_AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
