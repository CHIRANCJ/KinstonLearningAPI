using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KinstonUniAPI.Data;
using KinstonUniAPI.Models;
using System.Security.Claims;

namespace KinstonUniAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")] // Authorization restricted to Students
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/student/enroll
        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollInCourse(int courseId)
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Check if student is already enrolled in another course
            if (await _context.Enrollments.AnyAsync(e => e.StudentId == studentId))
                return BadRequest("Already enrolled in a course.");

            // Enroll the student in the course
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok("Enrolled successfully.");
        }

        // GET: api/student/certificates
        [HttpGet("certificates")]
        public async Task<IActionResult> GetCertificates()
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var certificates = await _context.Certificates
                .Where(c => c.StudentId == studentId)
                .ToListAsync();

            return Ok(certificates);
        }
    }
}
