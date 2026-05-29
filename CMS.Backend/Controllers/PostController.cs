using CMS.Data;
using CMS.Data.Entitis; // ✅ ĐÃ SỬA: Thay 'Entitis' thành 'Entities' để hết lỗi gạch đỏ
using Microsoft.AspNetCore.Authorization; // Khai báo thư viện bảo mật của Buổi 5
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Authorize] // 🔒 BẢO MẬT BUỔI 5: Khóa toàn bộ Controller, bắt buộc phải đăng nhập mới được vào
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. INDEX: id đại diện cho CategoryId (Dùng int? để linh hoạt hơn)
        public IActionResult Index(int? id)
        {
            // Tạo câu truy vấn ban đầu lấy danh sách bài viết kèm thông tin danh mục
            var query = _context.Posts.Include(p => p.Category).AsQueryable();

            // Nếu người dùng chọn lọc theo CategoryId cụ thể
            if (id.HasValue && id.Value > 0)
            {
                query = query.Where(p => p.CategoryId == id.Value);
                ViewBag.SelectedCategoryId = id.Value; // Lưu lại ID để phục vụ giao diện (nếu cần)
            }

            // Sắp xếp bài viết mới nhất lên trên đầu
            var filteredPosts = query.OrderByDescending(p => p.CreatedDate).ToList();

            return View(filteredPosts);
        }

        // 2. DETAILS: Xem chi tiết một bài viết cụ thể
        public IActionResult Details(int id)
        {
            var post = _context.Posts.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            return post == null ? NotFound() : View(post);
        }

        // 3. CREATE (GET): Hiển thị form thêm bài viết mới
        [HttpGet]
        public IActionResult Create()
        {
            // Nạp danh sách danh mục đổ vào dropdown select ngoài giao diện
            ViewBag.CategoryList = _context.Categories.ToList();
            return View();
        }

        // 4. CREATE (POST): Xử lý lưu bài viết mới vào Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post model)
        {
            // Loại bỏ kiểm tra ràng buộc ngầm định của đối tượng Category liên kết để tránh lỗi model binding
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now; // Tự động gán thời gian tạo hiện tại
                _context.Posts.Add(model);
                _context.SaveChanges();

                TempData["Success"] = "Đăng bài viết mới thành công!";
                return RedirectToAction(nameof(Index), new { id = model.CategoryId });
            }

            // Nếu lỗi, nạp lại danh sách danh mục trước khi trả về View lỗi
            ViewBag.CategoryList = _context.Categories.ToList();
            return View(model);
        }

        // 5. EDIT (GET): Đọc bài viết cũ lên form để chỉnh sửa
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            ViewBag.CategoryList = _context.Categories.ToList();
            return View(post);
        }

        // 6. EDIT (POST): Lưu thông tin chỉnh sửa vào Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post model)
        {
            // Loại bỏ kiểm tra thuộc tính ảo điều hướng liên kết
            ModelState.Remove("Category");

            System.Diagnostics.Debug.WriteLine($"ID nhận được: {model.Id}, Title: {model.Title}, CategoryId: {model.CategoryId}");

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = _context.Categories.ToList();
                return View(model);
            }

            var postInDb = _context.Posts.Find(model.Id);
            if (postInDb == null)
            {
                return Content("Lỗi: Không tìm thấy bài viết với ID = " + model.Id + " trong database!");
            }

            // Gán dữ liệu thay đổi
            postInDb.Title = model.Title;
            postInDb.Content = model.Content;
            postInDb.ImageUrl = model.ImageUrl;
            postInDb.CategoryId = model.CategoryId;

            _context.SaveChanges();
            TempData["Success"] = "Cập nhật bài viết thành công!";

            return RedirectToAction(nameof(Index), new { id = postInDb.CategoryId });
        }

        // 7. DELETE (POST): Xóa bài viết một cách an toàn qua phương thức POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                int categoryId = post.CategoryId;
                _context.Posts.Remove(post);
                _context.SaveChanges();

                TempData["Success"] = "Xóa bài viết thành công!";
                return RedirectToAction(nameof(Index), new { id = categoryId });
            }
            return NotFound();
        }
    }
}
