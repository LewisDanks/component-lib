using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papercut.Web.Infrastructure.Auth;

namespace Papercut.Web.Application.Auth.Login;

public sealed record SignInCommand(
    bool IsAlreadyAuthenticated,
    string RedirectPath,
    string Email,
    string Password,
    string? TwoFactorCode)
    : IRequest<SignInCommandResult>;

public sealed class SignInCommandHandler(AuthDbContext db, SignInManager<ApplicationUser> signInManager)
    : IRequestHandler<SignInCommand, SignInCommandResult>
{
    private readonly AuthDbContext _db = db;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<SignInCommandResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        if (request.IsAlreadyAuthenticated)
        {
            return SignInCommandResult.Success(request.RedirectPath);
        }

        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return SignInCommandResult.Invalid("Email and password are required.", requiresTwoFactor: false);
        }

        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var user = await _db.Users
            .SingleOrDefaultAsync(candidate => candidate.NormalizedEmail == normalizedEmail, cancellationToken);

        if (user is null || string.IsNullOrWhiteSpace(user.UserName))
        {
            return SignInCommandResult.Invalid("Invalid email or password.", requiresTwoFactor: false);
        }

        var passwordResult = await _signInManager.PasswordSignInAsync(
            user.UserName,
            request.Password,
            isPersistent: true,
            lockoutOnFailure: true);

        if (passwordResult.Succeeded)
        {
            return SignInCommandResult.Success(request.RedirectPath);
        }

        if (passwordResult.RequiresTwoFactor)
        {
            if (string.IsNullOrWhiteSpace(request.TwoFactorCode))
            {
                return SignInCommandResult.TwoFactorRequired("Enter your authenticator code to continue.");
            }

            var twoFactorResult = await _signInManager.TwoFactorAuthenticatorSignInAsync(
                NormalizeTwoFactorCode(request.TwoFactorCode),
                isPersistent: true,
                rememberClient: false);

            if (twoFactorResult.Succeeded)
            {
                return SignInCommandResult.Success(request.RedirectPath);
            }

            return SignInCommandResult.Invalid("Invalid authenticator code.", requiresTwoFactor: true);
        }

        if (passwordResult.IsLockedOut)
        {
            return SignInCommandResult.Invalid(
                "Account is temporarily locked. Try again later.",
                requiresTwoFactor: false);
        }

        return SignInCommandResult.Invalid("Invalid email or password.", requiresTwoFactor: false);
    }

    private static string NormalizeTwoFactorCode(string twoFactorCode)
    {
        return twoFactorCode.Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("-", string.Empty, StringComparison.Ordinal);
    }
}

public sealed class SignInCommandResult
{
    public required string Outcome { get; init; }
    public string? RedirectPath { get; init; }
    public string? Message { get; init; }
    public bool RequiresTwoFactor { get; init; }

    public static SignInCommandResult Success(string redirectPath)
    {
        return new SignInCommandResult
        {
            Outcome = "success",
            RedirectPath = redirectPath,
            Message = null,
            RequiresTwoFactor = false,
        };
    }

    public static SignInCommandResult TwoFactorRequired(string message)
    {
        return new SignInCommandResult
        {
            Outcome = "twoFactorRequired",
            RedirectPath = null,
            Message = message,
            RequiresTwoFactor = true,
        };
    }

    public static SignInCommandResult Invalid(string message, bool requiresTwoFactor)
    {
        return new SignInCommandResult
        {
            Outcome = "invalid",
            RedirectPath = null,
            Message = message,
            RequiresTwoFactor = requiresTwoFactor,
        };
    }
}
