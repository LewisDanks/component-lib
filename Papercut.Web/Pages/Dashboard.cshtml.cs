using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papercut.Web.Application.Auth.Dashboard;
using Papercut.Web.Application.Auth.Shared;
using Papercut.Web.Infrastructure.ClientContext;

namespace Papercut.Web.Pages;

[Authorize]
[ClientContext(typeof(DashboardClientContextBuilder))]
public class DashboardModel(ISender sender) : PageModel
{
    private readonly ISender _sender = sender;

    public string DisplayName { get; private set; } = string.Empty;
    public string PreferredTimeZoneId { get; private set; } = "UTC";
    public string PreferredCulture { get; private set; } = "en-GB";

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetDashboardQuery(GetCurrentUserId()), cancellationToken);
        if (!result.IsAuthenticated)
        {
            await _sender.Send(new SignOutCommand(), cancellationToken);
            return RedirectToPage("/Login", new { returnUrl = Url.Page("/Dashboard") });
        }

        DisplayName = result.DisplayName;
        PreferredTimeZoneId = result.PreferredTimeZoneId;
        PreferredCulture = result.PreferredCulture;

        return Page();
    }

    public async Task<IActionResult> OnGetSignOutAsync(CancellationToken cancellationToken)
    {
        await _sender.Send(new SignOutCommand(), cancellationToken);
        return RedirectToPage("/Login");
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
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
    public required string SettingsPath { get; init; }
    public required string PreferredTimeZoneId { get; init; }
    public required string PreferredCulture { get; init; }
    public required string ServerUtcIso { get; init; }
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
        var settingsPath = dashboardPageModel.Url.Page("/Settings") ?? "/Settings";

        IClientContext context = new DashboardClientContext
        {
            Params = new DashboardParams(),
            State = new DashboardState
            {
                DisplayName = dashboardPageModel.DisplayName,
                SignOutPath = signOutPath,
                SettingsPath = settingsPath,
                PreferredTimeZoneId = dashboardPageModel.PreferredTimeZoneId,
                PreferredCulture = dashboardPageModel.PreferredCulture,
                ServerUtcIso = DateTimeOffset.UtcNow.ToString("O"),
            },
        };

        return Task.FromResult(context);
    }
}
