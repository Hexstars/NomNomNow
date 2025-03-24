using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ASM.Share.Models;
using ASM.Share.Models.Requests;
using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace ASM.Server.Components.Pages.Auth
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [Inject]
        public IAccountSvc _accountService { get; set; }
        public LoginModel(IAccountSvc accountSvc)
        {
            _accountService = accountSvc;
        }
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string parameUsername, string paramPassword)
        {
            string returnUrl = Url.Content("~/");
            try
            {
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch
            {
            }

            bool flagLogin = false;
            var loginRequest = new LoginRequest() { Username = parameUsername, Password = paramPassword };

            Account nguoidung = _accountService.Login(loginRequest);

            if (nguoidung != null)
            {
                flagLogin = true;
            }

            if (flagLogin)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, parameUsername),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    RedirectUri = returnUrl
                };
                try
                {
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
                return LocalRedirect(returnUrl);
            }
            else
            {
            }
            return LocalRedirect(returnUrl);

        }
    }
}
