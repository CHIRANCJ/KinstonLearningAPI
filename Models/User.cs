using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KinstonUniAPI.Models
{
    public class User 
    {
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // 'Professor', 'Student'

        [Required]
        public string Name { get; set; } // Added Name property
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }

        // Navigation properties
        public ICollection<Course> Courses { get; set; } // If Professor
        public ICollection<Enrollment> Enrollments { get; set; } // If Student
    }
}
