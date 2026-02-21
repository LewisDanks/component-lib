using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papercut.Web.Infrastructure.Auth;

namespace Papercut.Web.Application.Auth.Settings;

public sealed record GetSettingsQuery(string? UserId) : IRequest<GetSettingsQueryResult>;

public sealed class GetSettingsQueryHandler(AuthDbContext db, UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetSettingsQuery, GetSettingsQueryResult>
{
    private readonly AuthDbContext _db = db;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<GetSettingsQueryResult> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            return GetSettingsQueryResult.NotAuthenticated();
        }

        var user = await _db.Users.SingleOrDefaultAsync(candidate => candidate.Id == request.UserId, cancellationToken);
        if (user is null)
        {
            return GetSettingsQueryResult.NotAuthenticated();
        }

        var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrWhiteSpace(authenticatorKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user)
                ?? throw new InvalidOperationException("Authenticator key could not be generated.");
        }

        return new GetSettingsQueryResult
        {
            IsAuthenticated = true,
            DisplayName = user.DisplayName,
            Email = user.Email ?? string.Empty,
            PreferredTimeZoneId = user.PreferredTimeZoneId,
            PreferredCulture = user.PreferredCulture,
            MarketingEmailsEnabled = user.MarketingEmailsEnabled,
            TwoFactorEnabled = user.TwoFactorEnabled,
            AuthenticatorSharedKey = FormatKey(authenticatorKey),
            AuthenticatorUri = BuildAuthenticatorUri(user.Email ?? user.UserName ?? "papercut", authenticatorKey),
        };
    }

    private static string FormatKey(string unformattedKey)
    {
        var builder = new StringBuilder();
        var currentPosition = 0;

        while (currentPosition + 4 < unformattedKey.Length)
        {
            builder.Append(unformattedKey.AsSpan(currentPosition, 4));
            builder.Append(' ');
            currentPosition += 4;
        }

        if (currentPosition < unformattedKey.Length)
        {
            builder.Append(unformattedKey.AsSpan(currentPosition));
        }

        return builder.ToString().ToLowerInvariant();
    }

    private static string BuildAuthenticatorUri(string email, string unformattedKey)
    {
        const string issuer = "Papercut";
        return $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(email)}" +
               $"?secret={Uri.EscapeDataString(unformattedKey)}&issuer={Uri.EscapeDataString(issuer)}&digits=6";
    }
}

public sealed class GetSettingsQueryResult
{
    public bool IsAuthenticated { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PreferredTimeZoneId { get; init; } = "UTC";
    public string PreferredCulture { get; init; } = "en-GB";
    public bool MarketingEmailsEnabled { get; init; }
    public bool TwoFactorEnabled { get; init; }
    public string AuthenticatorSharedKey { get; init; } = string.Empty;
    public string AuthenticatorUri { get; init; } = string.Empty;

    public static GetSettingsQueryResult NotAuthenticated()
    {
        return new GetSettingsQueryResult
        {
            IsAuthenticated = false,
        };
    }
}
