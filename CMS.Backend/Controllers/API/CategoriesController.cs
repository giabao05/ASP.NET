using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Lấy tất cả danh mục
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }

        // 2. Thêm mới danh mục (CREATE)
        [HttpPost]
        public IActionResult Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok(category);
        }

        // 3. Sửa danh mục (UPDATE)
        [HttpPut("{id}")]
        public IActionResult Update(int id, Category category)
        {
            var existingCategory = _context.Categories.Find(id);
            if (existingCategory == null) return NotFound();

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            _context.SaveChanges();
            return Ok(existingCategory);
        }

        // 4. Xóa danh mục (DELETE)
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok(new { message = "Đã xóa thành công danh mục có ID: " + id });
        }
    }
}