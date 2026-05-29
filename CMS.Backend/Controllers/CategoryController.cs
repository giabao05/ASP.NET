using CMS.Data;
using CMS.Data.Entitis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. INDEX
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // 2. CREATE (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            // Bỏ qua lỗi ngầm của trường "Posts" (do lỗi Model binding)
            ModelState.Remove("Posts");

            if (ModelState.IsValid)
            {
                _context.Categories.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, trả về view kèm model để hiển thị lỗi đỏ trên form
            return View(model);
        }

        // 4. EDIT (GET)
        [HttpGet]
        public IActionResult Edit(int? id) // Dùng int? để xử lý trường hợp ID bị null
        {
            if (id == null) return NotFound();

            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // 5. EDIT (POST)
        [HttpPost]
        public IActionResult Edit(Category model)
        {
            // DÒNG NÀY RẤT QUAN TRỌNG: Kiểm tra dữ liệu thô nhận về
            System.Diagnostics.Debug.WriteLine($"Debug: ID={model.Id}, Name={model.Name}");

            if (model.Id == 0)
            {
                return Content("Lỗi: ID nhận về bằng 0. Có thể thẻ input hidden bị lỗi!");
            }

            var categoryInDb = _context.Categories.Find(model.Id);
            if (categoryInDb == null)
            {
                return Content("Lỗi: Không tìm thấy category trong DB với ID=" + model.Id);
            }

            categoryInDb.Name = model.Name;
            categoryInDb.Description = model.Description;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }       

        // 6. DELETE (POST) - Nên dùng POST cho hành động xóa để an toàn
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}