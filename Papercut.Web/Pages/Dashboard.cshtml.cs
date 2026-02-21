using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papercut.Web.Infrastructure.Auth;
using Papercut.Web.Infrastructure.ClientContext;

namespace Papercut.Web.Pages;

[ClientContext(typeof(DashboardClientContextBuilder))]
public class DashboardModel : PageModel
{
    public string DisplayName { get; private set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (!DummyUserSession.TryGetDisplayName(Request, out var displayName))
        {
            var returnUrl = Url.Page("/Dashboard");
            return RedirectToPage("/Login", new { returnUrl });
        }

        DisplayName = displayName;
        return Page();
    }

    public IActionResult OnGetSignOut()
    {
        DummyUserSession.SignOut(Response);
        return RedirectToPage("/Login");
    }
}

public sealed class DashboardClientContext : IClientContext
{
    public string PageName => "Dashboard";
    public required DashboardParams Params { get; init; }
    public required DashboardState State { get; init; }
}

public sealed class DashboardParams
{
}

public sealed class DashboardState
{
    public required string DisplayName { get; init; }
    public required string SignOutPath { get; init; }
}

public sealed class DashboardClientContextBuilder : IClientContextBuilder
{
    public Task<IClientContext> BuildAsync(PageModel pageModel, CancellationToken cancellationToken)
    {
        if (pageModel is not DashboardModel dashboardPageModel)
        {
            throw new InvalidOperationException(
                $"{nameof(DashboardClientContextBuilder)} can only build context for {nameof(DashboardModel)}.");
        }

        var signOutPath = dashboardPageModel.Url.Page("/Dashboard", pageHandler: "SignOut")
            ?? "/Dashboard?handler=SignOut";

        IClientContext context = new DashboardClientContext
        {
            Params = new DashboardParams(),
            State = new DashboardState
            {
                DisplayName = dashboardPageModel.DisplayName,
                SignOutPath = signOutPath,
            },
        };

        return Task.FromResult(context);
    }
}
