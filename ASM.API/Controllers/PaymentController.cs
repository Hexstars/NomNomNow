using ASM.Share.Models.Requests;
using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPaySvc _vnPayService;
        public PaymentController(IVnPaySvc vnPayService)
        {
            _vnPayService = vnPayService;
        }
        [HttpPost("CreatePaymentUrlVnpay")]
        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(new { redirectUrl = url }); // gửi URL về dưới dạng JSON
        }
    }
}
