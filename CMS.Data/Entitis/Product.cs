///*
// * Sinh viên: Lê Gia Bảo
// * Mã sinh viên: 2123110193
// * Lớp: CCQ2311F
// * Ngày tạo: 15/5/2026
// * Mô tả: Thực hiện quản lí danh mục
// * 
// */
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CMS.Data.Entitis
//{
//    public class Product
//    {
//        [Key]

//        public int Id { get; set; }


//        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]

//        public string Name { get; set; }


//        public string? Description { get; set; }


//        [Range(0, double.MaxValue)]

//        [Column(TypeName = "decimal(18,2)")]

//        public decimal Price { get; set; }


//        public int StockQuantity { get; set; }


//        public string? ImageUrl { get; set; }


//        // Khóa ngoại nối tới CategoryProduct

//        public int CategoryProductId { get; set; }


//        [ForeignKey("CategoryProductId")]

//        public virtual CategoryProduct? CategoryProduct { get; set; }
//    }
//} 
/*
 * Sinh viên: Lê Gia Bảo
 * Mã sinh viên: 2123110193
 * Lớp: CCQ2311F
 * Ngày tạo: 15/5/2026
 * Mô tả: Thực hiện quản lí sản phẩm
 */
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Entitis
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        // Khóa ngoại với Danh mục
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryProductId { get; set; }

        [ForeignKey("CategoryProductId")]
        public virtual CategoryProduct? CategoryProduct { get; set; }
    }
}