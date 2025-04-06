using ASM.Share.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace ASM.Share.Models.Services
{
    public interface IAccountSvc
    {
        public Account Login(LoginRequest request);
    }
    public class AccountSvc : IAccountSvc
    {
        private readonly ApplicationDbContext _context;
        private readonly IEncryptionHelper _encryptionHelper;
        public Account Login(LoginRequest request)
        {
            var account = _context.Accounts.Where(
                a => a.Email.Equals(request.Email)).Include(u => u.Role).FirstOrDefault();
            if (account == null)
            {
                return null;
            }


            bool validPass = _encryptionHelper.VerifyPassword(request.Password, account.Password);

            if (account != null && validPass)
            {
                return account;
            }
            return null;
        }
        public bool Register(Account account)
        {
            if (_context.Accounts.Any(a => a.Email == account.Email))
            {
                return false; // Email đã tồn tại
            }
            account.Password = _encryptionHelper.EncryptPassword(account.Password);

            _context.Add(account);
            _context.SaveChanges();

            // tạo cart
            var cart = new Cart
            {
                AccountId = account.AccountId  // set Id
            };

            // Add cart
            _context.Carts.Add(cart);
            _context.SaveChanges();  //lưu
            return true;
        }
    }

}
