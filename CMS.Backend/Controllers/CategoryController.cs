using CMS.Data;
using CMS.Data.Entitis; // ✅ ĐÃ SỬA: Thay 'Entitis' thành 'Entities' để hết lỗi gạch đỏ
using Microsoft.AspNetCore.Authorization; // Khai báo thư viện để sử dụng ổ khóa bảo mật
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Authorize] // 🔒 BẢO MẬT BUỔI 5: Bắt buộc phải đăng nhập thành công mới được truy cập các hàm bên dưới
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. INDEX: Hiển thị danh sách danh mục sản phẩm/bài viết
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // 2. CREATE (GET): Hiển thị form thêm mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. CREATE (POST): Xử lý lưu danh mục mới vào Database
        [HttpPost]
        [ValidateAntiForgeryToken] // Chống tấn công giả mạo yêu cầu từ bên ngoài (CSRF)
        public IActionResult Create(Category model)
        {
            // Bỏ qua lỗi ngầm của các thuộc tính liên kết (đảm bảo ModelState.IsValid hoạt động tốt)
            ModelState.Remove("Posts");
            ModelState.Remove("Products");

            if (ModelState.IsValid)
            {
                _context.Categories.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Thêm danh mục mới thành công!"; // Thêm thông báo xanh lá
                return RedirectToAction(nameof(Index));
            }

            // Nếu dữ liệu nhập lỗi, trả về form kèm dữ liệu cũ để hiện lỗi đỏ
            return View(model);
        }

        // 4. EDIT (GET): Đọc dữ liệu cũ lên Form để chỉnh sửa
        [HttpGet]
        public IActionResult Edit(int? id) // Dùng int? đề phòng trường hợp truyền ID rỗng
        {
            if (id == null) return NotFound();

            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // 5. EDIT (POST): Cập nhật thay đổi vào Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            // Kiểm tra loại bỏ ràng buộc không mong muốn của đối tượng liên kết liên quan
            ModelState.Remove("Posts");
            ModelState.Remove("Products");

            // Kiểm tra dữ liệu thô nhận về trong cửa sổ Output debug
            System.Diagnostics.Debug.WriteLine($"Debug: ID={model.Id}, Name={model.Name}");

            if (model.Id == 0)
            {
                return Content("Lỗi: ID nhận về bằng 0. Hãy kiểm tra lại thẻ <input type='hidden' asp-for='Id' /> tại giao diện Edit.cshtml!");
            }

            if (ModelState.IsValid)
            {
                var categoryInDb = _context.Categories.Find(model.Id);
                if (categoryInDb == null)
                {
                    return Content("Lỗi: Không tìm thấy danh mục trong DB với ID=" + model.Id);
                }

                // Tiến hành gán giá trị mới người dùng vừa cập nhật
                categoryInDb.Name = model.Name;
                categoryInDb.Description = model.Description;

                _context.SaveChanges();
                TempData["Success"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // 6. DELETE (POST): Xử lý xóa danh mục an toàn qua phương thức POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                TempData["Success"] = "Xóa danh mục thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}