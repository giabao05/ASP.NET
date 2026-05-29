//using System.Diagnostics;
//using CMS.Backend.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace CMS.Backend.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}
using System.Diagnostics;
using CMS.Data; // Bổ sung để nhận diện ApplicationDbContext
using CMS.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // 1. BỔ SUNG KHAI BÁO _context

        // 2. BỔ SUNG ApplicationDbContext context vào hàm khởi tạo để hệ thống tiêm vào
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Gán kết nối từ hệ thống vào biến cục bộ
        }

        public IActionResult Index()
        {
            // Bây giờ hệ thống đã hiểu _context là gì, lệnh này sẽ hết lỗi gạch đỏ 100%
            var categories = _context.Categories.ToList();

            // Truyền danh sách này vào View
            return View(categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}