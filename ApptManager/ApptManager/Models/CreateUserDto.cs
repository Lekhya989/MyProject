using System.ComponentModel.DataAnnotations;

namespace ApptManager.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Phone]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        [Required]
        public string UserType { get; set; }
    }

}
