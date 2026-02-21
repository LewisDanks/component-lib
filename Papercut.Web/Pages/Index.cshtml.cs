using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papercut.Web.Infrastructure.Auth;

namespace Papercut.Web.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        if (DummyUserSession.TryGetDisplayName(Request, out _))
        {
            return RedirectToPage("/Dashboard");
        }

        return RedirectToPage("/Login");
    }
}
