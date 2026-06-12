using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/OrderDetails - Lấy danh sách kèm tên sản phẩm
        [HttpGet]
        public IActionResult GetAll()
        {
            var details = _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .ToList();
            return Ok(details);
        }

        // 2. GET: api/OrderDetails/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var detail = _context.OrderDetails
                .Include(od => od.Product)
                .FirstOrDefault(od => od.Id == id);

            if (detail == null) return NotFound(new { message = "Không tìm thấy chi tiết đơn hàng." });
            return Ok(detail);
        }

        // 3. POST: api/OrderDetails - Thêm sản phẩm vào đơn hàng
        [HttpPost]
        public IActionResult Create(OrderDetail detail)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Kiểm tra xem đơn hàng có tồn tại không
            var order = _context.Orders.Find(detail.OrderId);
            if (order == null) return BadRequest(new { message = "Đơn hàng không tồn tại." });

            _context.OrderDetails.Add(detail);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = detail.Id }, detail);
        }

        // 4. DELETE: api/OrderDetails/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var detail = _context.OrderDetails.Find(id);
            if (detail == null) return NotFound();

            _context.OrderDetails.Remove(detail);
            _context.SaveChanges();
            return Ok(new { message = "Đã xóa sản phẩm khỏi đơn hàng." });
        }
    }
}