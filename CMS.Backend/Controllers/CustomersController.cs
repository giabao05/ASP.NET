using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;

namespace CMS.Backend.Controllers // Lưu ý: Bỏ thư mục .API để tránh trùng lặp với Controller cũ
{
    public class CustomersController : Controller // Kế thừa từ 'Controller' để dùng View()
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context) => _context = context;

        // 1. Hiển thị danh sách khách hàng
        public IActionResult Index()
        {
            var customers = _context.Customers.ToList();
            return View(customers); // Tìm file Views/Customers/Index.cshtml
        }

        // 2. Hiển thị trang tạo mới
        public IActionResult Create()
        {
            return View(); // Tìm file Views/Customers/Create.cshtml
        }

        // 3. Xử lý thêm mới từ form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer model)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index)); // Sau khi thêm, quay về danh sách
            }
            return View(model); // Nếu lỗi dữ liệu, trả về lại form với thông báo lỗi
        }
    }
}