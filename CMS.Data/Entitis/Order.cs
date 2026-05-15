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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entitis
{
    public class Order
    {
        [Key]

        public int Id { get; set; }


        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }


        public int Status { get; set; } // 0: Chờ duyệt, 1: Đang giao, 2: Đã xong


        public string? Notes { get; set; }


        [ForeignKey("CustomerId")]

        public virtual Customer? Customer { get; set; }


        public virtual ICollection<OrderDetail>? OrderDetails
        {
            get; set;
        }
    }
}
