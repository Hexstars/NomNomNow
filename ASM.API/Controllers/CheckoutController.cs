using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASM.API.Controllers
{
    public class CheckoutController : Controller
    {
        private IVnPaySvc _vnPayService;

        public CheckoutController(IVnPaySvc vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }
    }
}
