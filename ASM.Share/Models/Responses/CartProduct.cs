using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASM.Share.Models.Responses
{
    [NotMapped]
    public class CartProduct
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Tổng cộng")]
        public decimal Total => UnitPrice * Quantity; // Tổng giá
    }
}
