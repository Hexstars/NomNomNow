using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models.Responses
{
    [NotMapped]
    public class OrderProductView
    {
        public string ProductName { get; set; }
        public string Image { get; set; }
        public decimal UnitPrice { get; set; } // Giá lúc đặt
        public int Quantity { get; set; }
        public decimal SubTotal => UnitPrice * Quantity; // Tổng tiền cho sản phẩm này
    }

    [NotMapped]
    public class OrderView
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<OrderProductView> Products { get; set; } = new List<OrderProductView>(); // Danh sách sản phẩm
        public decimal Total => Products.Sum(p => p.SubTotal); // Tổng tiền của cả đơn hàng
        public OrderStatus Status { get; set; }
    }
}
