using Microsoft.AspNetCore.Identity;

namespace Papercut.Web.Infrastructure.Auth;

public sealed class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = "";
    public string PreferredTimeZoneId { get; set; } = "UTC";
    public string PreferredCulture { get; set; } = "en-GB";
    public bool MarketingEmailsEnabled { get; set; }
}
