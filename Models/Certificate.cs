using System.ComponentModel.DataAnnotations;

namespace KinstonUniAPI.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }

        public string Title { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; } // Navigation to the Student

        public int CourseId { get; set; }
        public Course Course { get; set; } // Navigation to the Course

        public DateTime IssueDate { get; set; }
    }
}
