using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Papercut.Web.Infrastructure.Auth;

public static class AuthSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<AuthDbContext>();
        await db.Database.EnsureCreatedAsync();

        var options = services.GetRequiredService<IOptions<AuthSeedOptions>>().Value;
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        var existingUser = await userManager.FindByEmailAsync(options.Email);
        if (existingUser is not null)
        {
            var deleteResult = await userManager.DeleteAsync(existingUser);
            if (!deleteResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to reset demo identity user: {string.Join(", ", deleteResult.Errors.Select(e => e.Description))}");
            }
        }

        var user = new ApplicationUser
        {
            UserName = options.Email,
            Email = options.Email,
            EmailConfirmed = true,
            DisplayName = options.DisplayName,
            PreferredTimeZoneId = options.PreferredTimeZoneId,
            PreferredCulture = options.PreferredCulture,
            MarketingEmailsEnabled = false,
            TwoFactorEnabled = false,
        };

        var createResult = await userManager.CreateAsync(user, options.Password);
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to seed demo identity user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
        }

        await userManager.ResetAuthenticatorKeyAsync(user);
    }
}
