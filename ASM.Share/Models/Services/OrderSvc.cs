using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models.Services
{
    public interface IOrderSvc
    {
        //Task<List<Order>> GetAllOrders(int userId); //Lấy tất cả đơn hàng
        //Task<bool> CreateOrder(Order order); //Thêm đơn hàng
        //Task<List<Product>> GetAllProduct(int cartId); //Lấy tất cả sản phẩm trong giỏ hàng
    }
    public class OrderSvc : IOrderSvc
    {
        private readonly ICartSvc _cartSvc;
        private readonly IAccountSvc _accountSvc;
        public OrderSvc(ICartSvc cartSvc, IAccountSvc accountSvc)
        {
            _cartSvc = cartSvc;
            _accountSvc = accountSvc;
        }
    }
}
