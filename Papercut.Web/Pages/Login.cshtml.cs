using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Papercut.Web.Application.Auth.Login;
using Papercut.Web.Infrastructure.Auth;
using Papercut.Web.Infrastructure.ClientContext;

namespace Papercut.Web.Pages;

[AllowAnonymous]
[ClientContext(typeof(LoginClientContextBuilder))]
public class LoginModel(ISender sender) : PageModel
{
    private readonly ISender _sender = sender;

    public string? ReturnUrl { get; private set; }

    public IActionResult OnGet([FromQuery] string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Dashboard");
        }

        ReturnUrl = NormalizeReturnUrl(returnUrl);
        return Page();
    }

    public async Task<IActionResult> OnPostSignInAsync(
        [FromForm] LoginSignInRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SignInCommand(
            IsAlreadyAuthenticated: User.Identity?.IsAuthenticated == true,
            RedirectPath: ResolveReturnUrl(request.ReturnUrl),
            Email: request.Email,
            Password: request.Password,
            TwoFactorCode: request.TwoFactorCode);

        var result = await _sender.Send(command, cancellationToken);
        return new JsonResult(new LoginSignInResponse
        {
            Outcome = result.Outcome,
            RedirectPath = result.RedirectPath,
            Message = result.Message,
            RequiresTwoFactor = result.RequiresTwoFactor,
        });
    }

    private string ResolveReturnUrl(string? returnUrl)
    {
        return NormalizeReturnUrl(returnUrl) ?? (Url.Page("/Dashboard") ?? "/Dashboard");
    }

    private string? NormalizeReturnUrl(string? returnUrl)
    {
        return Url.IsLocalUrl(returnUrl) ? returnUrl : null;
    }

}

public sealed class LoginClientContext : IClientContext
{
    public string PageName => "Login";
    public required LoginParams Params { get; init; }
    public required LoginState State { get; init; }
}

public sealed class LoginParams
{
    public string? ReturnUrl { get; init; }
}

public sealed class LoginState
{
    public required string SignInPath { get; init; }
    public required string DashboardPath { get; init; }
    public required string AntiForgeryToken { get; init; }
    public required string AntiForgeryHeaderName { get; init; }
    public required string DemoEmail { get; init; }
    public required string DemoPassword { get; init; }
}

public sealed class LoginSignInRequest
{
    public string? ReturnUrl { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? TwoFactorCode { get; init; }
}

public sealed class LoginSignInResponse
{
    public required string Outcome { get; init; }
    public string? RedirectPath { get; init; }
    public string? Message { get; init; }
    public bool RequiresTwoFactor { get; init; }

    public static LoginSignInResponse Success(string redirectPath)
    {
        return new LoginSignInResponse
        {
            Outcome = "success",
            RedirectPath = redirectPath,
            Message = null,
            RequiresTwoFactor = false,
        };
    }

    public static LoginSignInResponse TwoFactorRequired(string message)
    {
        return new LoginSignInResponse
        {
            Outcome = "twoFactorRequired",
            RedirectPath = null,
            Message = message,
            RequiresTwoFactor = true,
        };
    }

    public static LoginSignInResponse Invalid(string message, bool requiresTwoFactor)
    {
        return new LoginSignInResponse
        {
            Outcome = "invalid",
            RedirectPath = null,
            Message = message,
            RequiresTwoFactor = requiresTwoFactor,
        };
    }
}

public sealed class LoginClientContextBuilder(
    IAntiforgery antiForgery,
    IOptions<AntiforgeryOptions> antiForgeryOptions,
    IOptions<AuthSeedOptions> authSeedOptions)
    : IClientContextBuilder
{
    private readonly IAntiforgery _antiForgery = antiForgery;
    private readonly IOptions<AntiforgeryOptions> _antiForgeryOptions = antiForgeryOptions;
    private readonly IOptions<AuthSeedOptions> _authSeedOptions = authSeedOptions;

    public Task<IClientContext> BuildAsync(PageModel pageModel, CancellationToken cancellationToken)
    {
        if (pageModel is not LoginModel loginPageModel)
        {
            throw new InvalidOperationException(
                $"{nameof(LoginClientContextBuilder)} can only build context for {nameof(LoginModel)}.");
        }

        var antiForgeryTokens = _antiForgery.GetAndStoreTokens(loginPageModel.HttpContext);
        var signInPath = loginPageModel.Url.Page("/Login", pageHandler: "SignIn") ?? "/Login?handler=SignIn";
        var dashboardPath = loginPageModel.Url.Page("/Dashboard") ?? "/Dashboard";
        var authSeed = _authSeedOptions.Value;

        IClientContext context = new LoginClientContext
        {
            Params = new LoginParams
            {
                ReturnUrl = loginPageModel.ReturnUrl,
            },
            State = new LoginState
            {
                SignInPath = signInPath,
                DashboardPath = dashboardPath,
                AntiForgeryToken = antiForgeryTokens.RequestToken
                    ?? throw new InvalidOperationException("Antiforgery request token was not generated."),
                AntiForgeryHeaderName = _antiForgeryOptions.Value.HeaderName ?? "RequestVerificationToken",
                DemoEmail = authSeed.Email,
                DemoPassword = authSeed.Password,
            },
        };

        return Task.FromResult(context);
    }
}
