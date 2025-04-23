using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models
{
    public class OrderConfirmationEmail
    {
        public string OrderNumber { get; set; }
        public List<OrderItemEmail> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderItemEmail
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
