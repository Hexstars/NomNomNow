using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Share.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [StringLength(100)]
        [Required(ErrorMessage = "Bạn cần nhập tên.")]
        [Column(TypeName = "nvarchar(100)")]
        public string ProductName { get; set; }


        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "Bạn cần nhập đơn giá.")]
        [Range(0, double.MaxValue, ErrorMessage = "Bạn cần nhập đơn giá hợp lệ.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }


        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Bạn cần nhập số lượng.")]
        [Range(0, int.MaxValue, ErrorMessage = "Bạn cần nhập số lượng hợp lệ.")]
        public int Quantity { get; set; }


        [StringLength(250)]
        [Display(Name = "Hình")]
        public string? Image { get; set; }


        [NotMapped]
        [Display(Name = "Chọn hình")]
        public IFormFile? ImageFile { get; set; }


        [Display(Name = "Phân loại")]
        [Required(ErrorMessage = "Bạn cần chọn loại.")]
        public int CategoryId { get; set; }  // Foreign Key
        public virtual Category? Category { get; set; } //Dùng để đi vào bảng Role để truy vấn các trường khác


        // Tạo mối quan hệ nhiều-nhiều
        public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
