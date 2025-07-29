using System;
using System.ComponentModel.DataAnnotations;

namespace ApptManager.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Phone]
        [MaxLength(15)]
        public string MobileNumber { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public UserType UserType { get; set; } = UserType.NONE;
    }

    public enum UserType
    {
        NONE,ADMIN,USER
    }
}
