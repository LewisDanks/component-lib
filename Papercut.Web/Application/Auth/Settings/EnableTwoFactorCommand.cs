using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papercut.Web.Infrastructure.Auth;

namespace Papercut.Web.Application.Auth.Settings;

public sealed record EnableTwoFactorCommand(string? UserId, string Code) : IRequest<SettingsActionResult>;

public sealed class EnableTwoFactorCommandHandler(AuthDbContext db, UserManager<ApplicationUser> userManager)
    : IRequestHandler<EnableTwoFactorCommand, SettingsActionResult>
{
    private readonly AuthDbContext _db = db;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<SettingsActionResult> Handle(EnableTwoFactorCommand request, CancellationToken cancellationToken)
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

        if (user.TwoFactorEnabled)
        {
            return SettingsActionResult.Success("Two-factor authentication is already enabled.");
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return SettingsActionResult.Invalid("Authenticator code is required.");
        }

        var authenticatorCode = request.Code
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("-", string.Empty, StringComparison.Ordinal);

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(
            user,
            _userManager.Options.Tokens.AuthenticatorTokenProvider,
            authenticatorCode);

        if (!isValid)
        {
            return SettingsActionResult.Invalid("Invalid authenticator code.");
        }

        user.TwoFactorEnabled = true;
        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return SettingsActionResult.Invalid("Failed to enable two-factor authentication.");
        }

        return SettingsActionResult.Success("Two-factor authentication enabled.");
    }
}
