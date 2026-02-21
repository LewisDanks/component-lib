using MediatR;
using Microsoft.EntityFrameworkCore;
using Papercut.Web.Infrastructure.Auth;
using Papercut.Web.Infrastructure.Localization;

namespace Papercut.Web.Application.Auth.Settings;

public sealed record SavePreferencesCommand(
    string? UserId,
    string DisplayName,
    string PreferredTimeZoneId,
    string PreferredCulture,
    bool MarketingEmailsEnabled)
    : IRequest<SettingsActionResult>;

public sealed class SavePreferencesCommandHandler(AuthDbContext db)
    : IRequestHandler<SavePreferencesCommand, SettingsActionResult>
{
    private readonly AuthDbContext _db = db;

    public async Task<SettingsActionResult> Handle(SavePreferencesCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            return SettingsActionResult.Invalid("Session expired. Please sign in again.");
        }

        var user = await _db.Users.SingleOrDefaultAsync(candidate => candidate.Id == request.UserId, cancellationToken);
        if (user is null)
        {
            return SettingsActionResult.Invalid("Session expired. Please sign in again.");
        }

        var displayName = request.DisplayName.Trim();
        if (displayName.Length == 0)
        {
            return SettingsActionResult.Invalid("Display name is required.");
        }

        if (!UserPreferenceCatalog.IsValidTimeZone(request.PreferredTimeZoneId))
        {
            return SettingsActionResult.Invalid("Choose a valid time zone.");
        }

        if (!UserPreferenceCatalog.IsValidCulture(request.PreferredCulture))
        {
            return SettingsActionResult.Invalid("Choose a valid locale.");
        }

        user.DisplayName = displayName;
        user.PreferredTimeZoneId = request.PreferredTimeZoneId;
        user.PreferredCulture = request.PreferredCulture;
        user.MarketingEmailsEnabled = request.MarketingEmailsEnabled;

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return SettingsActionResult.Invalid("Failed to save preferences.");
        }

        return SettingsActionResult.Success("Preferences updated.");
    }
}
