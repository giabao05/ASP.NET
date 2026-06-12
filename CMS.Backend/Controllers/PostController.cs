using CMS.Data;
using CMS.Data.Entitis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX
        public IActionResult Index(int? id)
        {
            var query = _context.Posts.Include(p => p.Category).AsQueryable();

            if (id.HasValue && id.Value > 0)
            {
                query = query.Where(p => p.CategoryId == id.Value);
            }

            var posts = query.OrderByDescending(p => p.CreatedDate).ToList();
            return View(posts);
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var post = _context.Posts.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            return post == null ? NotFound() : View(post);
        }

        // CREATE GET
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryList = _context.Categories.ToList();
            return View();
        }

        // ==================== CREATE POST - ĐÃ SỬA UPLOAD ẢNH ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post model, IFormFile? ImageFile)
        {
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                // XỬ LÝ UPLOAD ẢNH
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "images", "posts");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    model.ImageUrl = "/images/posts/" + uniqueFileName;
                }

                model.CreatedDate = DateTime.Now;
                _context.Posts.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đăng bài viết mới thành công!";
                return RedirectToAction(nameof(Index), new { id = model.CategoryId });
            }

            ViewBag.CategoryList = _context.Categories.ToList();
            return View(model);
        }

        // EDIT GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            ViewBag.CategoryList = _context.Categories.ToList();
            return View(post);
        }

        // ==================== EDIT POST - ĐÃ SỬA UPLOAD ẢNH ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post model, IFormFile? ImageFile)
        {
            ModelState.Remove("Category");

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = _context.Categories.ToList();
                return View(model);
            }

            var postInDb = await _context.Posts.FindAsync(model.Id);
            if (postInDb == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin
            postInDb.Title = model.Title;
            postInDb.Content = model.Content;
            postInDb.CategoryId = model.CategoryId;

            // Xử lý upload ảnh mới (nếu có)
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "images", "posts");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                postInDb.ImageUrl = "/images/posts/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật bài viết thành công!";

            return RedirectToAction(nameof(Index), new { id = postInDb.CategoryId });
        }

        // DELETE
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