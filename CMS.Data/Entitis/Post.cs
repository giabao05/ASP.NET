using CMS.Data.Entitis;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Entitis // Đảm bảo thư mục này tên là Entities
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Content { get; set; }

        public string? ImageUrl { get; set; }

        // Trường thời gian đăng bài
        [Display(Name = "Ngày đăng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Vui lòng chọn chuyên mục")]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
    }
}