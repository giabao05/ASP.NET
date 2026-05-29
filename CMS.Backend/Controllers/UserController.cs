using CMS.Data;
using CMS.Data.Entitis; // ✅ ĐÃ SỬA: Thay 'Entitis' thành 'Entities' và xóa dòng lặp để hết lỗi gạch đỏ
using Microsoft.AspNetCore.Authorization; // Khai báo thư viện bảo mật của Buổi 5
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMS.Backend.Controllers
{
    // 🔒 BẢO MẬT BUỔI 5: Chỉ cho phép tài khoản đã đăng nhập VÀ có vai trò "Admin" truy cập phân hệ này
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Hiển thị danh sách thành viên hệ thống
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // 2. GET: Hiển thị giao diện Thêm thành viên mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Xử lý lưu dữ liệu Thêm thành viên mới vào Database
        [HttpPost]
        [ValidateAntiForgeryToken] // Chống tấn công giả mạo (CSRF)
        public IActionResult Create(User model)
        {
            // Kiểm tra trùng lặp tài khoản dựa trên tên đăng nhập (Username)
            var checkExist = _context.Users.Any(u => u.Username == model.Username);
            if (checkExist)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập này đã có người sử dụng!");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                _context.Users.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Thêm thành viên mới thành công!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // 3. GET: Hiển thị form Sửa kèm thông tin cũ của thành viên
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Lưu thông tin chỉnh sửa cập nhật vào Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User model, string NewPassword)
        {
            var existingUser = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == model.Id);
            if (existingUser == null) return NotFound();

            // Xử lý logic thay đổi Mật khẩu linh hoạt
            if (!string.IsNullOrEmpty(NewPassword))
            {
                // Nếu người quản trị nhập mật khẩu mới vào ô "NewPassword" -> lấy mật khẩu mới
                model.PasswordHash = NewPassword;
            }
            else
            {
                // Nếu ô mật khẩu mới bỏ trống -> giữ nguyên mật khẩu cũ trong database
                model.PasswordHash = existingUser.PasswordHash;
            }

            if (ModelState.IsValid)
            {
                _context.Users.Update(model);
                _context.SaveChanges();
                TempData["Success"] = "Cập nhật thông tin thành viên thành công!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // 4. POST/GET: Xử lý hành động Xóa thành viên khỏi hệ thống
        // Lưu ý: Có thể dùng HttpPost kết hợp ValidateAntiForgeryToken để tăng tính bảo mật cho nút xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["Success"] = "Xóa thành viên thành công!";
            }
            return RedirectToAction("Index");
        }

        // Hàm dự phòng trường hợp thẻ HTML Button ngoài View của bạn đang gọi đường dẫn dạng GET /User/Delete/id
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["Success"] = "Xóa thành viên hệ thống thành công!";
            }
            return RedirectToAction("Index");
        }
    }
}