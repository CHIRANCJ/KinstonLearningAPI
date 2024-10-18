using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KinstonUniAPI.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CurrentBatchStartDate { get; set; }
        public DateTime CurrentBatchEndDate { get; set; }
        public bool IsApproved { get; set; } // Indicates if the course is approved

        // Foreign Key
        public int ProfessorId { get; set; }
        public User Professor { get; set; } // Navigation to the Professor

        // Navigation properties
        public ICollection<Module> Modules { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } // Students enrolled
    }
}
