using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models
{
    public class OrderDetail
    {
        [Display(Name = "Mã đơn hàng")]
        [Required(ErrorMessage = "Bạn cần chọn mã đơn hàng.")]

        public int OrderId { get; set; }  // Foreign Key đến Order
        public virtual Order Order { get; set; }


        [Display(Name = "Mã sản phẩm")]
        [Required(ErrorMessage = "Bạn cần chọn mã sản phẩm.")]

        public int ProductId { get; set; }  // Foreign Key đến Product
        public virtual Product Product { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Bạn cần nhập số lượng.")]
        [Range(0, 20, ErrorMessage = "Bạn cần nhập số lượng hợp lệ.")]
        public int Quantity { get; set; }


        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "Bạn cần nhập đơn giá.")]
        [Range(0, double.MaxValue, ErrorMessage = "Bạn cần nhập đơn giá hợp lệ.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }  // Đơn giá sản phẩm tại thời điểm đặt hàng
    }
}
