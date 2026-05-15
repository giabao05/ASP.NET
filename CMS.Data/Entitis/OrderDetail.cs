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
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entitis
{
    public class OrderDetail
    {
        [Key]

        public int Id { get; set; }


        public int OrderId { get; set; }


        public int ProductId { get; set; }


        public int Quantity { get; set; }


        [Column(TypeName = "decimal(18,2)")]

        public decimal UnitPrice { get; set; } // Giá tại thời điểm mua


        [ForeignKey("OrderId")]

        public virtual Order? Order { get; set; }


        [ForeignKey("ProductId")]

        public virtual Product? Product { get; set; }
    }
}
