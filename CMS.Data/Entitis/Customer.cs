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
    // Khách hàng
    public class Customer
    {
        [Key]

        public int Id { get; set; }


        [Required]

        public string FullName { get; set; }


        [Required]

        [EmailAddress]

        public string Email { get; set; }


        public string? Phone { get; set; }


        public string? Address { get; set; }


        [Required]

        public string Password { get; set; } // Lưu mật khẩu thô theo yêu cầu tối giản


        public virtual ICollection<Order>? Orders { get; set; }
    }
}
