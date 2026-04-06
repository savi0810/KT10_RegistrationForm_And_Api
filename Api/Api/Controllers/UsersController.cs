using Api.Data;
using Api.Data.DTO;
using Api.Data.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"Пользователь с ID {id} не найден" });
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return Ok(userDto);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == createDto.Username || u.Email == createDto.Email);

            if (existing != null)
            {
                if (existing.Username == createDto.Username)
                    ModelState.AddModelError("Username", "Имя пользователя уже занято");
                if (existing.Email == createDto.Email)
                    ModelState.AddModelError("Email", "Email уже используется");
                return BadRequest(ModelState);
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(createDto.Password);

            var user = new User
            {
                Username = createDto.Username,
                Email = createDto.Email,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"Пользователь с ID {id} не найден" });
            }

            var existing = await _context.Users
                .FirstOrDefaultAsync(u => (u.Username == updateDto.Username || u.Email == updateDto.Email) && u.Id != id);

            if (existing != null)
            {
                if (existing.Username == updateDto.Username)
                    ModelState.AddModelError("Username", "Имя пользователя уже занято");
                if (existing.Email == updateDto.Email)
                    ModelState.AddModelError("Email", "Email уже используется");
                return BadRequest(ModelState);
            }

            user.Username = updateDto.Username;
            user.Email = updateDto.Email;

            if (!string.IsNullOrWhiteSpace(updateDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Password);
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"Пользователь с ID {id} не найден" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}