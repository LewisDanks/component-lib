namespace Papercut.Web.Infrastructure.Auth;

public static class DummyUserSession
{
    private const string CookieName = "papercut_dummy_user";
    public const string DefaultDisplayName = "Demo User";

    public static bool TryGetDisplayName(HttpRequest request, out string displayName)
    {
        if (request.Cookies.TryGetValue(CookieName, out var cookieValue) &&
            !string.IsNullOrWhiteSpace(cookieValue))
        {
            displayName = cookieValue;
            return true;
        }

        displayName = string.Empty;
        return false;
    }

    public static void SignIn(HttpResponse response, string displayName)
    {
        response.Cookies.Append(
            CookieName,
            displayName,
            new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Secure = false
            });
    }

    public static void SignOut(HttpResponse response)
    {
        response.Cookies.Delete(CookieName);
    }
}
