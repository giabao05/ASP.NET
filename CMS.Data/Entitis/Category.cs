/*
 * Sinh viên: Lê Gia Bảo
 * Mã sinh viên: 2123110193
 * Lớp: CCQ2311F
 * Ngày tạo: 15/5/2026
 * Mô tả: Thực hiện quản lí danh mục
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entitis
{
   public class Category
    {
            public int Id { get; set; }
        public string Name { get; set; }//Tên danh mục
        public string Description { get; set; } 
        
        //Quan hệ: Một danh mục có nhiều bài viết
        public virtual ICollection<Post> Posts { get; set; }


    }
}
