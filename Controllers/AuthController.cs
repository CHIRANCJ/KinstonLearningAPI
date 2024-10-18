using KinstonUniAPI.DTOs;
using KinstonUniAPI.Models;
using KinstonUniAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace KinstonUniAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDTO.Email))
                return BadRequest("Email is already registered");

            // Create a new User instance
            var user = new User
            {
                Email = registerDTO.Email,
                Name = registerDTO.Name,
                Role = registerDTO.Role,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, registerDTO.Password),
                IsActive = true,  // Set to active
                IsApproved = registerDTO.Role == "Registrar" // Registrars are approved automatically
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // If Professor or Student, they need approval
            if (user.Role == "Professor" || user.Role == "Student")
                return Ok("Registration successful, awaiting approval from Registrar.");

            return Ok("Registration successful.");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");

            var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
                return Unauthorized("Invalid email or password");

            if (!user.IsApproved)
                return BadRequest("Your account has not been approved by a registrar.");

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                UserId = user.UserId,
                Name = user.Name,
                Role = user.Role
            });
        }

        // Helper function to generate JWT token
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
