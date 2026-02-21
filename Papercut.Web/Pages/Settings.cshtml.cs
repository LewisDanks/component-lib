using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papercut.Web.Infrastructure.Auth;
using Papercut.Web.Infrastructure.ClientContext;

namespace Papercut.Web.Pages;

[ClientContext(typeof(SettingsClientContextBuilder))]
public class SettingsModel : PageModel
{
    public string DisplayName { get; private set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (!DummyUserSession.TryGetDisplayName(Request, out var displayName))
        {
            var returnUrl = Url.Page("/Settings");
            return RedirectToPage("/Login", new { returnUrl });
        }

        DisplayName = displayName;
        return Page();
    }
}

public sealed class SettingsClientContext : IClientContext
{
    public string PageName => "Settings";
    public required SettingsParams Params { get; init; }
    public required SettingsState State { get; init; }
}

public sealed class SettingsParams
{
}

public sealed class SettingsState
{
    public required string DisplayName { get; init; }
    public required string Email { get; init; }
    public required string Timezone { get; init; }
    public required IReadOnlyList<SettingsOption> TimezoneOptions { get; init; }
    public required bool MarketingEmailsEnabled { get; init; }
    public required bool TwoFactorEnabled { get; init; }
    public required string DashboardPath { get; init; }
}

public sealed class SettingsOption
{
    public required string Value { get; init; }
    public required string Label { get; init; }
}

public sealed class SettingsClientContextBuilder : IClientContextBuilder
{
    public Task<IClientContext> BuildAsync(PageModel pageModel, CancellationToken cancellationToken)
    {
        if (pageModel is not SettingsModel settingsPageModel)
        {
            throw new InvalidOperationException(
                $"{nameof(SettingsClientContextBuilder)} can only build context for {nameof(SettingsModel)}.");
        }

        var displayName = settingsPageModel.DisplayName;
        var emailLocalPart = displayName.Replace(" ", ".", StringComparison.Ordinal).ToLowerInvariant();
        var dashboardPath = settingsPageModel.Url.Page("/Dashboard") ?? "/Dashboard";

        var timezoneOptions = new List<SettingsOption>
        {
            new() { Value = "utc", Label = "UTC" },
            new() { Value = "est", Label = "US Eastern" },
            new() { Value = "pst", Label = "US Pacific" },
        };

        IClientContext context = new SettingsClientContext
        {
            Params = new SettingsParams(),
            State = new SettingsState
            {
                DisplayName = displayName,
                Email = $"{emailLocalPart}@example.test",
                Timezone = "utc",
                TimezoneOptions = timezoneOptions,
                MarketingEmailsEnabled = false,
                TwoFactorEnabled = false,
                DashboardPath = dashboardPath,
            },
        };

        return Task.FromResult(context);
    }
}
