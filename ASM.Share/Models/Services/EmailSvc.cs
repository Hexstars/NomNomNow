using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models.Services
{
    // IEmailSvc.cs
    public interface IEmailSvc
    {
        Task<bool> SendOrderConfirmationEmailAsync(string toEmail, OrderConfirmationEmail model);
    }

    // EmailSvc.cs
    public class EmailSvc : IEmailSvc
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSvc> _logger;

        public EmailSvc(IConfiguration config, ILogger<EmailSvc> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendOrderConfirmationEmailAsync(string toEmail, OrderConfirmationEmail model)
        {
            try
            {
                var smtpServer = _config["EmailSettings:SmtpServer"];
                var port = int.Parse(_config["EmailSettings:Port"]);
                var username = _config["EmailSettings:Username"];
                var password = _config["EmailSettings:Password"];
                var fromEmail = _config["EmailSettings:FromEmail"];
                var fromName = _config["EmailSettings:FromName"];

                using var client = new SmtpClient(smtpServer, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = $"Xác nhận đơn hàng #{model.OrderNumber}",
                    Body = BuildEmailBody(model),
                    IsBodyHtml = true
                };
                mail.To.Add(toEmail);

                await client.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email");
                return false;
            }
        }

        private string BuildEmailBody(OrderConfirmationEmail model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='UTF-8'>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: 'Arial', sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; }");
            sb.AppendLine(".header { color: #2c3e50; font-size: 24px; margin-bottom: 10px; }");
            sb.AppendLine(".order-number { font-size: 16px; margin-bottom: 20px; }");
            sb.AppendLine("table { width: 100%; border-collapse: collapse; margin: 20px 0; }");
            sb.AppendLine("th { background-color: #f8f9fa; text-align: left; padding: 12px; border-bottom: 2px solid #ddd; }");
            sb.AppendLine("td { padding: 10px 12px; border-bottom: 1px solid #eee; }");
            sb.AppendLine(".text-right { text-align: right; }");
            sb.AppendLine(".total-row td { font-weight: bold; padding-top: 15px; border-top: 2px solid #ddd; }");
            sb.AppendLine(".footer { margin-top: 30px; font-size: 14px; color: #777; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            // Header
            sb.AppendLine("<div class='header'>🎉 Đơn hàng đã được xác nhận</div>");
            sb.AppendLine($"<div class='order-number'>Mã đơn hàng: <strong>#{model.OrderNumber}</strong></div>");

            // Order Items Table
            sb.AppendLine("<table>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>Sản phẩm</th>");
            sb.AppendLine("<th style='text-align: center;'>Số lượng</th>");  // Quantity column
            sb.AppendLine("<th style='text-align: right;'>Đơn giá</th>");
            sb.AppendLine("<th style='text-align: right;'>Thành tiền</th>");
            sb.AppendLine("</tr>");

            foreach (var item in model.Items)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{item.ProductName}</td>");
                sb.AppendLine($"<td style='text-align: center;'>{item.Quantity}</td>");
                sb.AppendLine($"<td style='text-align: right;'>{item.Price:N0} ₫</td>");
                sb.AppendLine($"<td style='text-align: right;'>{(item.Price * item.Quantity):N0}₫</td>");
                sb.AppendLine("</tr>");
            }

            // Total Row
            sb.AppendLine("<tr class='total-row'>");
            sb.AppendLine("<td colspan='3' style='text-align: right;'>Tổng cộng:</td>");
            sb.AppendLine($"<td style='text-align: right;'><strong>{model.TotalAmount:N0}₫</strong></td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");

            // Footer
            sb.AppendLine("<div class='footer'>");
            sb.AppendLine("<p>Cảm ơn bạn đã đặt hàng tại <strong>Nom Nom Now</strong>!</p>");
            sb.AppendLine("<p>Mọi thắc mắc vui lòng liên hệ: <a href='mailto:baotnc19@gmail.com' style='color: #3498db;'>baotnc19@gmail.com</a></p>");
            sb.AppendLine("</div>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
