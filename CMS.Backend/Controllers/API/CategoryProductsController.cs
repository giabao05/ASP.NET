using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers.API
{
    // 1. SỬA ĐƯỜNG DẪN: Viết cố định là "api/CategoryProducts" để giữ nguyên URL gọi API của bạn
    [Route("api/CategoryProducts")]
    [ApiController]
    // 2. ĐỔI TÊN CLASS: Đổi từ CategoryProductsController thành CategoryProductsApiController
    // Điều này giúp hệ thống phân biệt được đâu là API, đâu là Giao diện Web
    public class CategoryProductsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryProductsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/CategoryProducts - Lấy toàn bộ danh mục sản phẩm
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.CategoryProducts.ToList();
            return Ok(categories);
        }

        // 2. GET: api/CategoryProducts/{id} - Lấy chi tiết một danh mục
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.CategoryProducts.Find(id);
            if (category == null) return NotFound(new { message = "Không tìm thấy danh mục" });

            return Ok(category);
        }

        // 3. POST: api/CategoryProducts - Thêm danh mục mới
        [HttpPost]
        public IActionResult Create(CategoryProduct category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.CategoryProducts.Add(category);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // 4. PUT: api/CategoryProducts/{id} - Cập nhật danh mục
        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryProduct category)
        {
            var existingCategory = _context.CategoryProducts.Find(id);
            if (existingCategory == null) return NotFound();

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            _context.SaveChanges();
            return Ok(existingCategory);
        }

        // 5. DELETE: api/CategoryProducts/{id} - Xóa danh mục
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.CategoryProducts.Find(id);
            if (category == null) return NotFound();

            _context.CategoryProducts.Remove(category);
            _context.SaveChanges();
            return Ok(new { message = "Đã xóa thành công danh mục ID: " + id });
        }
    }
}