using ASM.Share.Models.Requests;

namespace ASM.Share.Models.Services
{
    public interface IAccountSvc
    {
        public Account Login(LoginRequest request);
    }
    public class AccountSvc : IAccountSvc
    {
        public Account Login(LoginRequest request)
        {
            return new Account();
        }

    }

}
