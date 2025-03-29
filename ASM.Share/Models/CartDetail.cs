using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models
{
    public class CartDetail
    {
        [Display(Name = "Mã giỏ hàng")]
        [Required(ErrorMessage = "Bạn cần chọn mã giỏ hàng.")]
        public int CartId { get; set; }
        public virtual Cart? Cart { get; set; }  // Khóa ngoại tham chiếu đến bảng Cart


        [Display(Name = "Mã sản phẩm")]
        [StringLength(20)]
        [Required(ErrorMessage = "Bạn cần chọn mã sản phẩm.")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }  // Khóa ngoại tham chiếu đến bảng Product


        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Bạn cần nhập số lượng.")]
        [Range(0, 20, ErrorMessage = "Bạn cần nhập số lượng hợp lệ.")]
        public int Quantity { get; set; }
    }
}
