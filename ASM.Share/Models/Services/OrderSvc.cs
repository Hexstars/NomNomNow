using ASM.Share.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models.Services
{
    public interface IOrderSvc
    {
        Task<List<OrderView>> GetAllOrders(int userId); //Lấy tất cả đơn hàng
        Task<bool> CreateOrder(Order order); //Thêm đơn hàng
        Task<bool> AddIntoDetail(List<CartProduct> products, int orderId); //Thêm vào bảng chi tiết đơn hàng
    }
    public class OrderSvc : IOrderSvc
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartSvc _cartSvc;
        private readonly IAccountSvc _accountSvc;
        public OrderSvc(ICartSvc cartSvc, IAccountSvc accountSvc)
        {
            _cartSvc = cartSvc;
            _accountSvc = accountSvc;
        }

        public OrderSvc(ApplicationDbContext context, ICartSvc cartSvc, IAccountSvc accountSvc)
        {
            _context = context;
            _cartSvc = cartSvc;
            _accountSvc = accountSvc;
        }

        public async Task<List<OrderView>> GetAllOrders(int userId)
        {
            // Lấy danh sách tất cả các đơn hàng của user
            var orders = (from o in _context.Orders
                          where o.AccountId == userId
                          select new OrderView
                          {
                              OrderId = o.OrderId,
                              OrderDate = o.OrderDate,
                              Phone = o.Phone,
                              Address = o.Address,
                              Status = o.Status,
                              Products = (from od in _context.OrderDetails //Query tất cả sản phẩm ở chi tiết đơn hàng và gán vào list OrderProductView
                                          join p in _context.Products on od.ProductId equals p.ProductId
                                          where od.OrderId == o.OrderId
                                          select new OrderProductView
                                          {
                                              ProductName = p.ProductName,
                                              Image = p.Image,
                                              UnitPrice = od.UnitPrice,
                                              Quantity = od.Quantity
                                          }).ToList()
                          }).ToListAsync();

            return await orders;
        }
        public async Task<bool> CreateOrder(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddIntoDetail(List<CartProduct> products, int orderId)
        {
            foreach (var item in products)
            {
                OrderDetail detail = new OrderDetail
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };

                _context.OrderDetails.Add(detail);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
