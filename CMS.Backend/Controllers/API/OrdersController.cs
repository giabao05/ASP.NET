using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/Orders - Lấy tất cả đơn hàng kèm thông tin khách hàng
        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return Ok(orders);
        }

        // 2. GET: api/Orders/{id} - Lấy chi tiết đơn hàng kèm chi tiết sản phẩm
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound(new { message = "Không tìm thấy đơn hàng." });
            return Ok(order);
        }

        // 3. POST: api/Orders - Tạo đơn hàng mới
        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            order.OrderDate = System.DateTime.Now;
            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        // 4. PUT: api/Orders/{id} - Cập nhật trạng thái đơn hàng
        [HttpPut("{id}")]
        public IActionResult UpdateStatus(int id, [FromBody] int status)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            order.Status = status;
            _context.SaveChanges();
            return Ok(new { message = "Đã cập nhật trạng thái đơn hàng thành công." });
        }

        // 5. DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return Ok(new { message = "Đã xóa đơn hàng." });
        }
    }
}