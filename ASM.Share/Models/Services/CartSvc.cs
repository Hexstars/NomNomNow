using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models.Services
{
    public interface ICartSvc
    {
        Task<Cart> GetUserCart(int userId); //Lấy giỏ hàng
        Task<List<CartProduct>> GetCartItemsAsync(int id); //Hiển thị giỏ hàng
        Task<bool> AddToCart(int userId, int productId, int quantity);//Thêm vào giỏ hàng
        Task<bool> UpdateQuantity(int userId, int productId, int newQuantity);//Thay đổi số lượng
        Task<bool> DeleteFromCart(int userId, int productId);
        Task<bool> ClearCartAsync(int cartId);//Dọn sản phẩm trong giỏ hàng
    }
    public class CartSvc : ICartSvc
    {
        private readonly ApplicationDbContext _context;

        public CartSvc(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Cart> GetUserCart(int userId)
        {
            return await _context.Carts.FindAsync(userId);
        }
        public async Task<List<CartProduct>> GetCartItemsAsync(int id)
        {
            Cart cart = await GetUserCart(id);

            if (cart == null)
            {
                return new List<CartProduct>(); // Trả về rỗng nếu thiếu
            }

            //Query và tạo list chứa sản phẩm
            List<CartProduct> cartProducts = (from cd in _context.CartDetails
                                              join p in _context.Products on cd.ProductId equals p.ProductId
                                              where cd.CartId == cart.CartId
                                              select new CartProduct
                                              {
                                                  ProductId = p.ProductId,
                                                  Image = p.Image,
                                                  ProductName = p.ProductName,
                                                  UnitPrice = p.UnitPrice,
                                                  Quantity = cd.Quantity
                                              }).ToList();
            return cartProducts;
        }
        public async Task<bool> AddToCart(int userId, int productId, int Quantity)
        {
            // Kiểm tra giỏ hàng tồn tại chưa
            var userCart = await GetUserCart(userId);

            if (userCart == null)
            {
                // Nếu giỏ hàng chưa tồn tại, tạo mới
                userCart = new Cart
                {
                    AccountId = userId,
                };
                _context.Carts.Add(userCart);
                await _context.SaveChangesAsync();
            }

            // Kiểm tra nếu giỏ hàng đã có sản phẩm này chưa
            var existingCartDetail = _context.CartDetails
                .FirstOrDefault(cd => cd.CartId == userCart.CartId && cd.ProductId == productId);

            if (existingCartDetail != null)
            {
                // Nếu có rồi, tăng số lượng của sản phẩm trong giỏ hàng
                existingCartDetail.Quantity += 1;
                _context.CartDetails.Update(existingCartDetail);
            }
            else
            {
                // Nếu chưa có, thêm sản phẩm mới vào giỏ hàng
                var cartDetail = new CartDetail
                {
                    CartId = userCart.CartId,
                    ProductId = productId,
                    Quantity = Quantity,
                };
                _context.CartDetails.Add(cartDetail);
            }
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantity(int userId, int productId, int newQuantity)
        {
            try
            {
                var cart = await GetUserCart(userId);

                if (cart == null)
                {
                    return false; // Giỏ hàng không tồn tại
                }
                // Find all items in the cart
                var cartProduct = _context.CartDetails.FirstOrDefault(cd => cd.CartId == cart.CartId && cd.ProductId == productId);

                if (cartProduct == null)
                {
                    return false; // Sản phẩm không có trong giỏ hàng
                }
                cartProduct.Quantity = newQuantity;

                _context.Update(cartProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteFromCart(int userId, int productId)
        {
            try
            {
                var cart = await GetUserCart(userId);

                if (cart == null)
                {
                    return false; // Giỏ hàng không tồn tại
                }
                // Find all items in the cart
                var cartProduct = _context.CartDetails.FirstOrDefault(cd => cd.CartId == cart.CartId && cd.ProductId == productId);

                if (cartProduct == null)
                {
                    return false; // Sản phẩm không có trong giỏ hàng
                }
                // Remove the product from the cart
                _context.CartDetails.Remove(cartProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> ClearCartAsync(int cartId)
        {
            // Find all items in the cart
            var cartDetails = _context.CartDetails.Where(cd => cd.CartId == cartId).ToList();

            if (cartDetails == null || !cartDetails.Any())
            {
                return false; // No items to remove
            }

            // Remove all items
            _context.CartDetails.RemoveRange(cartDetails);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
