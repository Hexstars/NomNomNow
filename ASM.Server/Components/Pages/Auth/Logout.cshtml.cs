using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASM.Server.Components.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            await HttpContext
                .SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return LocalRedirect(Url.Content("~/"));
        }
    }
}
