using ASM.Share.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace ASM.Share.Models.Services
{
    public interface IAccountSvc
    {
        Task<Account> GetAccount(int id);
        Task<Account> Login(LoginRequest request);
        Task<bool> Register(Account account);
    }
    public class AccountSvc : IAccountSvc
    {
        private readonly ApplicationDbContext _context;
        private readonly IEncryptionHelper _encryptionHelper;

        public AccountSvc(ApplicationDbContext context, IEncryptionHelper encryptionHelper)
        {
            _context = context;
            _encryptionHelper = encryptionHelper;
        }

        public async Task<Account> GetAccount(int id)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == id);
            return account;
        }
        public async Task<Account> Login(LoginRequest request)
        {
            var account = _context.Accounts.Where(
                a => a.Email.Equals(request.Email)).FirstOrDefault();
            if (account == null)
            {
                return null;
            }


            bool validPass = await _encryptionHelper.VerifyPassword(request.Password, account.Password);

            if (account != null && validPass)
            {
                return account;
            }
            return null;
        }
        public async Task<bool> Register(Account account)
        {
            if (_context.Accounts.Any(a => a.Email == account.Email))
            {
                return false; // Email đã tồn tại
            }
            account.Password = await _encryptionHelper.EncryptPassword(account.Password);

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
