using System.ComponentModel.DataAnnotations;

namespace KinstonUniAPI.Models
{
    public class Module
    {
        public int ModuleId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

        // Foreign Key
        public int CourseId { get; set; }
        public Course Course { get; set; } // Navigation to the Course
    }
}
