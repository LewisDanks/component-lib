namespace Papercut.Web.Infrastructure.Auth;

public sealed class AuthSeedOptions
{
    public string Email { get; init; } = "demo@papercut.local";
    public string Password { get; init; } = "Papercut!123";
    public string DisplayName { get; init; } = "Demo User";
    public string PreferredTimeZoneId { get; init; } = "UTC";
    public string PreferredCulture { get; init; } = "en-GB";
}
