using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;

namespace CMS.Backend.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/Customers
        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }

        // 2. GET: api/Customers/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound(new { message = "Không tìm thấy khách hàng." });
            return Ok(customer);
        }

        // 3. POST: api/Customers
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Customers.Add(customer);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        // 4. PUT: api/Customers/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, Customer customer)
        {
            var existingCustomer = _context.Customers.Find(id);
            if (existingCustomer == null) return NotFound();

            existingCustomer.FullName = customer.FullName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Address = customer.Address;
            // Lưu ý: Thường không cập nhật Password trực tiếp qua API như thế này trong thực tế

            _context.SaveChanges();
            return Ok(existingCustomer);
        }

        // 5. DELETE: api/Customers/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();

            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return Ok(new { message = "Đã xóa khách hàng thành công." });
        }
    }
}