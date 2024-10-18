using System.ComponentModel.DataAnnotations;

namespace KinstonUniAPI.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        // Foreign Keys
        public int CourseId { get; set; }
        public Course Course { get; set; } // Navigation to the Course

        [Required]
        public DateTime EnrollmentDate { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; } // Navigation to the Student
    }
}
