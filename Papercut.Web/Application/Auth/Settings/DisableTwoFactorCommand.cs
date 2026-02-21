using MediatR;
using Microsoft.EntityFrameworkCore;
using Papercut.Web.Infrastructure.Auth;

namespace Papercut.Web.Application.Auth.Settings;

public sealed record DisableTwoFactorCommand(string? UserId) : IRequest<SettingsActionResult>;

public sealed class DisableTwoFactorCommandHandler(AuthDbContext db)
    : IRequestHandler<DisableTwoFactorCommand, SettingsActionResult>
{
    private readonly AuthDbContext _db = db;

    public async Task<SettingsActionResult> Handle(DisableTwoFactorCommand request, CancellationToken cancellationToken)
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

        if (!user.TwoFactorEnabled)
        {
            return SettingsActionResult.Success("Two-factor authentication is already disabled.");
        }

        user.TwoFactorEnabled = false;
        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return SettingsActionResult.Invalid("Failed to disable two-factor authentication.");
        }

        return SettingsActionResult.Success("Two-factor authentication disabled.");
    }
}
