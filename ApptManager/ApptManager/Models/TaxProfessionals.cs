using System.ComponentModel.DataAnnotations;

namespace ApptManager.Models
{
    public class TaxProfessional
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;       
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage ="the tax professionl learnt spanish")]
        public bool speaks_spanish { get; set; }
        public bool SMB_certified { get; set; }
        public DateTime CreatedOn { get; set; }                  
        public DateTime? UpdatedOn { get; set; }
        
    }
}