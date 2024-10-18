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
    [Authorize(Roles = "Professor")] // Authorization restricted to Professors
    public class ProfessorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/professor/courses
        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            var professorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var courses = await _context.Courses
                .Where(c => c.ProfessorId == professorId)
                .ToListAsync();

            return Ok(courses);
        }

        // POST: api/professor/courses
        [HttpPost("courses")]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            var professorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            course.ProfessorId = professorId;
            course.IsApproved = false; // Needs approval from Registrar

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok("Course created, awaiting approval.");
        }

        // PUT: api/professor/courses/{id}
        [HttpPut("courses/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, Course updatedCourse)
        {
            var professorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var course = await _context.Courses.FindAsync(id);
            if (course == null || course.ProfessorId != professorId)
                return NotFound("Course not found or not owned by professor.");

            course.Title = updatedCourse.Title;
            course.Description = updatedCourse.Description;
            course.CurrentBatchStartDate = updatedCourse.CurrentBatchStartDate;
            course.CurrentBatchEndDate = updatedCourse.CurrentBatchEndDate;
            course.IsApproved = false; // Needs approval after update

            await _context.SaveChangesAsync();

            return Ok("Course updated, awaiting approval.");
        }
    }
}
