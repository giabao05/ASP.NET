/*
 * Sinh viên: Lê Gia Bảo
 * Mã sinh viên: 2123110193
 * Lớp: CCQ2311F
 * Ngày tạo: 15/5/2026
 * Mô tả: Thực hiện quản lí bài viết
 * */
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Entitis
{
    public class Post
    {
        // THÊM 2 DÒNG NÀY ĐỂ FIX TRIỆT ĐỂ LỖI DATABASE TỰ ĐỘNG TĂNG ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; } // Tiêu đề bài viết

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Content { get; set; } // Nội dung chi tiết

        // Dùng dấu ? để cho phép không bắt buộc nhập ảnh (tránh lỗi Required)
        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Vui lòng chọn chuyên mục")]
        public int CategoryId { get; set; }

        // THÊM DẤU ? VÀO ĐÂY (Category?) ĐỂ FIX LỖI "The Category field is required" KHI SUBMIT FORM
        public virtual Category? Category { get; set; }
    }
}