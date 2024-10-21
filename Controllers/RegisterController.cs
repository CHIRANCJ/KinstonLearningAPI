using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KinstonUniAPI.Data;
using KinstonUniAPI.Models;

namespace KinstonUniAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /*[Authorize(Roles = "Registrar")] */// Authorization restricted to Registrars
    public class RegistrarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegistrarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/registrar/users
        [HttpGet("users")]
        public async Task<IActionResult> GetPendingUsers()
        {
            var users = await _context.Users
                .Where(u => !u.IsApproved)
                .Select(u => new
                {
                    u.UserId,
                    u.Name,
                    u.Email,
                    u.Role,  // Include role in the response
                    u.IsApproved
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/registrar/all-users
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync(); // Assuming you want all users
            return Ok(users);
        }

        // PUT: api/registrar/approve-user/{id}
        [HttpPut("approve-user/{id}")]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            user.IsApproved = true;
            await _context.SaveChangesAsync();

            return Ok("User approved successfully.");
        }

        // DELETE: api/registrar/delete-user/{id}
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully.");
        }

        // GET: api/registrar/courses
        [HttpGet("courses")]
        public async Task<IActionResult> GetPendingCourses()
        {
            var courses = await _context.Courses
                .Where(c => !c.IsApproved) // Fetch courses that are not approved yet
                .ToListAsync();

            return Ok(courses);
        }

        // PUT: api/registrar/approve-course/{id}
        [HttpPut("approve-course/{id}")]
        public async Task<IActionResult> ApproveCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound("Course not found");

            course.IsApproved = true;
            await _context.SaveChangesAsync();

            return Ok("Course approved successfully.");
        }
    }
}
