using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papercut.Web.Infrastructure.Auth;
using Papercut.Web.Infrastructure.ClientContext;

namespace Papercut.Web.Pages;

[ClientContext(typeof(LoginClientContextBuilder))]
public class LoginModel : PageModel
{
    public string? ReturnUrl { get; private set; }

    public IActionResult OnGet([FromQuery] string? returnUrl = null)
    {
        if (DummyUserSession.TryGetDisplayName(Request, out _))
        {
            return RedirectToPage("/Dashboard");
        }

        ReturnUrl = NormalizeReturnUrl(returnUrl);
        return Page();
    }

    public IActionResult OnGetSignIn([FromQuery] string? returnUrl = null)
    {
        var normalizedReturnUrl = NormalizeReturnUrl(returnUrl);
        DummyUserSession.SignIn(Response, DummyUserSession.DefaultDisplayName);

        if (normalizedReturnUrl is not null)
        {
            return LocalRedirect(normalizedReturnUrl);
        }

        return RedirectToPage("/Dashboard");
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
    public required string DefaultDisplayName { get; init; }
    public required string SignInPath { get; init; }
    public required string DashboardPath { get; init; }
}

public sealed class LoginClientContextBuilder : IClientContextBuilder
{
    public Task<IClientContext> BuildAsync(PageModel pageModel, CancellationToken cancellationToken)
    {
        if (pageModel is not LoginModel loginPageModel)
        {
            throw new InvalidOperationException(
                $"{nameof(LoginClientContextBuilder)} can only build context for {nameof(LoginModel)}.");
        }

        var signInPath = loginPageModel.Url.Page(
            "/Login",
            pageHandler: "SignIn",
            values: new { returnUrl = loginPageModel.ReturnUrl }) ?? "/Login?handler=SignIn";

        var dashboardPath = loginPageModel.Url.Page("/Dashboard") ?? "/Dashboard";

        IClientContext context = new LoginClientContext
        {
            Params = new LoginParams
            {
                ReturnUrl = loginPageModel.ReturnUrl,
            },
            State = new LoginState
            {
                DefaultDisplayName = DummyUserSession.DefaultDisplayName,
                SignInPath = signInPath,
                DashboardPath = dashboardPath,
            },
        };

        return Task.FromResult(context);
    }
}
