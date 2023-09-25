using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "UserName length must be between 3 and 10 characters.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Password length must be between 4 and 16 characters.")]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public DateOnly? DateOfBirth { get; set; }

    }
}