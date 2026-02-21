using Microsoft.Extensions.DependencyInjection;

namespace Papercut.Web.Infrastructure.ClientContext;

public static class DependencyInjection
{
    public static IServiceCollection AddClientContext(this IServiceCollection services)
    {
        // Per-request storage
        services.AddScoped<IClientContextAccessor, ClientContextAccessor>();

        // Register the filter (added to Razor Pages in Program.cs)
        services.AddSingleton<ClientContextPageFilter>();

        return services;
    }
}