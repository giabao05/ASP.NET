using CMS.Data;
using CMS.Data.Entitis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. INDEX: id là CategoryId (kiểu int)
        public IActionResult Index(int id)
        {
            var filteredPosts = _context.Posts
                .Include(p => p.Category)
                .Where(p => p.CategoryId == id)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();
            return View(filteredPosts);
        }

        // 2. DETAILS: Đổi lại thành kiểu int id
        public IActionResult Details(int id)
        {
            var post = _context.Posts.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            return post == null ? NotFound() : View(post);
        }

        // 3. CREATE (GET)
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryList = _context.Categories.ToList();
            return View();
        }

        // 4. CREATE (POST): Đã xóa dòng sinh chuỗi GUID để trả về cơ chế tự tăng của kiểu int
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post model)
        {
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                _context.Posts.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { id = model.CategoryId });
            }

            ViewBag.CategoryList = _context.Categories.ToList();
            return View(model);
        }

        // 5. EDIT (GET): Đổi lại thành kiểu int id
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            ViewBag.CategoryList = _context.Categories.ToList();
            return View(post);
        }

        // 6. EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post model)
        {
            System.Diagnostics.Debug.WriteLine($"ID nhận được: {model.Id}, Title: {model.Title}, CategoryId: {model.CategoryId}");

            if (!ModelState.IsValid)
            {
                return Content("Lỗi ModelState: Dữ liệu gửi lên không đúng định dạng hoặc thiếu trường bắt buộc!");
            }

            var postInDb = _context.Posts.Find(model.Id);
            if (postInDb == null)
            {
                return Content("Lỗi: Không tìm thấy bài viết với ID = " + model.Id + " trong database!");
            }

            postInDb.Title = model.Title;
            postInDb.Content = model.Content;
            postInDb.ImageUrl = model.ImageUrl;
            postInDb.CategoryId = model.CategoryId;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { id = postInDb.CategoryId });
        }

        // 7. DELETE (POST): Đổi lại thành kiểu int id
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
                return RedirectToAction(nameof(Index), new { id = categoryId });
            }
            return NotFound();
        }
    }
}