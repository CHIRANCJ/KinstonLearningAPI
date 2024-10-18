using System.ComponentModel.DataAnnotations;

namespace KinstonUniAPI.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; } // Can be "Professor", "Student", or "Registrar"
    }
}
