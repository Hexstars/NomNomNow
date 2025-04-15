using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASM.Share.Models
{
    public enum Role
    {
        User,
        Admin
    }
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập email")]
        [EmailAddress]
        //[Remote("")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [StringLength(50)]
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu.")]
        [Column(TypeName = "varchar(50)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [NotMapped]
        [Display(Name = "Nhập lại mật khẩu")]
        [StringLength(50)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Họ và tên")]
        [StringLength(100)]
        [Required(ErrorMessage = "Bạn cần nhập họ và tên.")]
        [Column(TypeName = "nvarchar(100)")]
        public string FullName { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(15)]
        [Required(ErrorMessage = "Bạn cần nhập số điện thoại.")]
        [Column(TypeName = "varchar(15)")]
        public string Phone { get; set; }


        [Display(Name = "Địa chỉ")]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Bạn cần nhập họ địa chỉ.")]
        public string Address { get; set; }


        [Display(Name = "Vai trò")]
        public Role Role { get; set; }  // Foreign Key

        // Thêm thuộc tính Orders để biểu diễn mối quan hệ với đơn hàng
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>(); // Mối quan hệ một-nhiều
        [JsonIgnore]
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    }
}
