using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LAB1.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Nam")]
        [PersonalData]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [PersonalData]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "City")]
        [PersonalData]
        public string? City { get; set; }
    }
}