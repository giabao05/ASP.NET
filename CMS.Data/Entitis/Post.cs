/*
 * Sinh viên: Lê Gia Bảo
 * Mã sinh viên: 2123110193
 * Lớp: CCQ2311F
 * Ngày tạo: 15/5/2026
 * Mô tả: Thực hiện quản lí danh mục
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entitis
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }//Tiêu đề bài viết
        public string Content { get; set; }//Nội dung chi tiết
        public string ImagaUrl { get ; set; }//Hình ảnh đại diện
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        //Khóa ngoại liên kết với Category
        public int CategoryId {  get; set; }
        public virtual Category Category { get; set; }
    }
}
