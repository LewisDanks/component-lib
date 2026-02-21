using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Papercut.Web.Application.Auth.Settings;
using Papercut.Web.Application.Auth.Shared;
using Papercut.Web.Infrastructure.ClientContext;
using Papercut.Web.Infrastructure.Localization;

namespace Papercut.Web.Pages;

[Authorize]
[ClientContext(typeof(SettingsClientContextBuilder))]
public class SettingsModel(ISender sender) : PageModel
{
    private readonly ISender _sender = sender;

    public string DisplayName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PreferredTimeZoneId { get; private set; } = "UTC";
    public string PreferredCulture { get; private set; } = "en-GB";
    public bool MarketingEmailsEnabled { get; private set; }
    public bool TwoFactorEnabled { get; private set; }
    public string AuthenticatorSharedKey { get; private set; } = string.Empty;
    public string AuthenticatorUri { get; private set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetSettingsQuery(GetCurrentUserId()), cancellationToken);
        if (!result.IsAuthenticated)
        {
            await _sender.Send(new SignOutCommand(), cancellationToken);
            return RedirectToPage("/Login", new { returnUrl = Url.Page("/Settings") });
        }

        HydratePageState(result);
        return Page();
    }

    public async Task<IActionResult> OnPostSavePreferencesAsync(
        [FromForm] SettingsSavePreferencesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new SavePreferencesCommand(
                UserId: GetCurrentUserId(),
                DisplayName: request.DisplayName,
                PreferredTimeZoneId: request.PreferredTimeZoneId,
                PreferredCulture: request.PreferredCulture,
                MarketingEmailsEnabled: request.MarketingEmailsEnabled),
            cancellationToken);

        return new JsonResult(ToActionResponse(result));
    }

    public async Task<IActionResult> OnPostEnableTwoFactorAsync(
        [FromForm] SettingsEnableTwoFactorRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new EnableTwoFactorCommand(GetCurrentUserId(), request.Code),
            cancellationToken);

        return new JsonResult(ToActionResponse(result));
    }

    public async Task<IActionResult> OnPostDisableTwoFactorAsync(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DisableTwoFactorCommand(GetCurrentUserId()), cancellationToken);
        return new JsonResult(ToActionResponse(result));
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    private void HydratePageState(GetSettingsQueryResult result)
    {
        DisplayName = result.DisplayName;
        Email = result.Email;
        PreferredTimeZoneId = result.PreferredTimeZoneId;
        PreferredCulture = result.PreferredCulture;
        MarketingEmailsEnabled = result.MarketingEmailsEnabled;
        TwoFactorEnabled = result.TwoFactorEnabled;
        AuthenticatorSharedKey = result.AuthenticatorSharedKey;
        AuthenticatorUri = result.AuthenticatorUri;
    }

    private static SettingsActionResponse ToActionResponse(SettingsActionResult result)
    {
        return new SettingsActionResponse
        {
            Outcome = result.Outcome,
            Message = result.Message,
        };
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
    public required string PreferredTimeZoneId { get; init; }
    public required IReadOnlyList<SettingsOption> TimeZoneOptions { get; init; }
    public required string PreferredCulture { get; init; }
    public required IReadOnlyList<SettingsOption> CultureOptions { get; init; }
    public required bool MarketingEmailsEnabled { get; init; }
    public required bool TwoFactorEnabled { get; init; }
    public required string AuthenticatorSharedKey { get; init; }
    public required string AuthenticatorUri { get; init; }
    public required string DashboardPath { get; init; }
    public required string SavePreferencesPath { get; init; }
    public required string EnableTwoFactorPath { get; init; }
    public required string DisableTwoFactorPath { get; init; }
    public required string AntiForgeryToken { get; init; }
    public required string AntiForgeryHeaderName { get; init; }
}

public sealed class SettingsOption
{
    public required string Value { get; init; }
    public required string Label { get; init; }
}

public sealed class SettingsSavePreferencesRequest
{
    public string DisplayName { get; init; } = string.Empty;
    public string PreferredTimeZoneId { get; init; } = string.Empty;
    public string PreferredCulture { get; init; } = string.Empty;
    public bool MarketingEmailsEnabled { get; init; }
}

public sealed class SettingsEnableTwoFactorRequest
{
    public string Code { get; init; } = string.Empty;
}

public sealed class SettingsActionResponse
{
    public required string Outcome { get; init; }
    public required string Message { get; init; }

    public static SettingsActionResponse Success(string message)
    {
        return new SettingsActionResponse
        {
            Outcome = "success",
            Message = message,
        };
    }

    public static SettingsActionResponse Invalid(string message)
    {
        return new SettingsActionResponse
        {
            Outcome = "invalid",
            Message = message,
        };
    }
}

public sealed class SettingsClientContextBuilder(IAntiforgery antiForgery, IOptions<AntiforgeryOptions> antiForgeryOptions)
    : IClientContextBuilder
{
    private readonly IAntiforgery _antiForgery = antiForgery;
    private readonly IOptions<AntiforgeryOptions> _antiForgeryOptions = antiForgeryOptions;

    public Task<IClientContext> BuildAsync(PageModel pageModel, CancellationToken cancellationToken)
    {
        if (pageModel is not SettingsModel settingsPageModel)
        {
            throw new InvalidOperationException(
                $"{nameof(SettingsClientContextBuilder)} can only build context for {nameof(SettingsModel)}.");
        }

        var antiForgeryTokens = _antiForgery.GetAndStoreTokens(settingsPageModel.HttpContext);
        var dashboardPath = settingsPageModel.Url.Page("/Dashboard") ?? "/Dashboard";
        var savePreferencesPath = settingsPageModel.Url.Page("/Settings", pageHandler: "SavePreferences")
            ?? "/Settings?handler=SavePreferences";
        var enableTwoFactorPath = settingsPageModel.Url.Page("/Settings", pageHandler: "EnableTwoFactor")
            ?? "/Settings?handler=EnableTwoFactor";
        var disableTwoFactorPath = settingsPageModel.Url.Page("/Settings", pageHandler: "DisableTwoFactor")
            ?? "/Settings?handler=DisableTwoFactor";

        IClientContext context = new SettingsClientContext
        {
            Params = new SettingsParams(),
            State = new SettingsState
            {
                DisplayName = settingsPageModel.DisplayName,
                Email = settingsPageModel.Email,
                PreferredTimeZoneId = settingsPageModel.PreferredTimeZoneId,
                TimeZoneOptions = UserPreferenceCatalog.TimeZoneOptions
                    .Select(option => new SettingsOption { Value = option.Value, Label = option.Label })
                    .ToArray(),
                PreferredCulture = settingsPageModel.PreferredCulture,
                CultureOptions = UserPreferenceCatalog.CultureOptions
                    .Select(option => new SettingsOption { Value = option.Value, Label = option.Label })
                    .ToArray(),
                MarketingEmailsEnabled = settingsPageModel.MarketingEmailsEnabled,
                TwoFactorEnabled = settingsPageModel.TwoFactorEnabled,
                AuthenticatorSharedKey = settingsPageModel.AuthenticatorSharedKey,
                AuthenticatorUri = settingsPageModel.AuthenticatorUri,
                DashboardPath = dashboardPath,
                SavePreferencesPath = savePreferencesPath,
                EnableTwoFactorPath = enableTwoFactorPath,
                DisableTwoFactorPath = disableTwoFactorPath,
                AntiForgeryToken = antiForgeryTokens.RequestToken
                    ?? throw new InvalidOperationException("Antiforgery request token was not generated."),
                AntiForgeryHeaderName = _antiForgeryOptions.Value.HeaderName ?? "RequestVerificationToken",
            },
        };

        return Task.FromResult(context);
    }
}
