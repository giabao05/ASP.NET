using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    public class CategoryProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Danh sách
        public IActionResult Index()
        {
            return View(_context.CategoryProducts.ToList());
        }

        // 2. GET: Form tạo mới
        public IActionResult Create() => View();

        // 3. POST: Xử lý lưu dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description")] CategoryProduct category)
        {
            if (!ModelState.IsValid) return View(category);

            _context.CategoryProducts.Add(category);
            _context.SaveChanges();

            TempData["Success"] = "Thêm danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }

        // 4. GET: Form sửa
        public IActionResult Edit(int id)
        {
            var category = _context.CategoryProducts.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // 5. POST: Lưu thay đổi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description")] CategoryProduct category)
        {
            if (id != category.Id) return NotFound();

            if (!ModelState.IsValid) return View(category);

            var existingCategory = _context.CategoryProducts.Find(id);
            if (existingCategory == null) return NotFound();

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            _context.SaveChanges();
            TempData["Success"] = "Cập nhật danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}