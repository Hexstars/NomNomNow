using System.ComponentModel.DataAnnotations;

namespace ASM.Share.Models.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Bạn cần nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập mật khẩu.")]
        [StringLength(50, ErrorMessage = "Mật khẩu không được dài quá 50 ký tự.")]
        public string Password { get; set; }
    }
}