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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entitis
{
    public class CategoryProduct
    {
        [Key]

        public int Id { get; set; }


        [Required(ErrorMessage = "Tên danh mục không được để trống")]

        [StringLength(100)]

        public string Name { get; set; }


        public string? Description { get; set; }


        // Quan hệ: Một danh mục có nhiều sản phẩm

        public virtual ICollection<Product>? Products { get; set; }
    }
}
