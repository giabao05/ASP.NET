using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;

namespace CMS.Backend.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/Users - Lấy danh sách người dùng (Không trả về Password)
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.Select(u => new {
                u.Id,
                u.Username,
                u.FullName,
                u.Role
            }).ToList();
            return Ok(users);
        }

        // 2. GET: api/Users/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.Select(u => new {
                u.Id,
                u.Username,
                u.FullName,
                u.Role
            }).FirstOrDefault(u => u.Id == id);

            if (user == null) return NotFound();
            return Ok(user);
        }

        // 3. POST: api/Users - Thêm người dùng mới
        [HttpPost]
        public IActionResult Create(User user)
        {
            // Lưu ý: Password nên được hash trước khi lưu vào DB
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(new { message = "Thêm người dùng thành công" });
        }

        // 4. DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok(new { message = "Đã xóa người dùng." });
        }
    }
}