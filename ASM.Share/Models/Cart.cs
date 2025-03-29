using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required(ErrorMessage = "Bạn cần chọn mã người dùng.")]
        [Display(Name = "Mã người dùng")]
        public int AccountId { get; set; }  // Foreign Key
        public virtual Account Account { get; set; } //Dùng để đi vào bảng Account để truy vấn các trường khác

        public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>(); // Danh sách các CartDetail
    }
}
