using System.ComponentModel.DataAnnotations;

namespace LAB1.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Company Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, 200)]
        [Display(Name = "Years in Business")]
        public int YearsInBusiness { get; set; }

        [Required]
        [StringLength(250)]
        [Url]
        public string Website { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Province { get; set; }
    }
}