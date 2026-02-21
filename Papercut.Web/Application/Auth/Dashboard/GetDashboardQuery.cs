using MediatR;
using Microsoft.EntityFrameworkCore;
using Papercut.Web.Infrastructure.Auth;

namespace Papercut.Web.Application.Auth.Dashboard;

public sealed record GetDashboardQuery(string? UserId) : IRequest<GetDashboardQueryResult>;

public sealed class GetDashboardQueryHandler(AuthDbContext db)
    : IRequestHandler<GetDashboardQuery, GetDashboardQueryResult>
{
    private readonly AuthDbContext _db = db;

    public async Task<GetDashboardQueryResult> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            return GetDashboardQueryResult.NotAuthenticated();
        }

        var user = await _db.Users
            .Where(candidate => candidate.Id == request.UserId)
            .Select(candidate => new
            {
                candidate.DisplayName,
                candidate.PreferredTimeZoneId,
                candidate.PreferredCulture,
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return GetDashboardQueryResult.NotAuthenticated();
        }

        return new GetDashboardQueryResult
        {
            IsAuthenticated = true,
            DisplayName = user.DisplayName,
            PreferredTimeZoneId = user.PreferredTimeZoneId,
            PreferredCulture = user.PreferredCulture,
        };
    }
}

public sealed class GetDashboardQueryResult
{
    public bool IsAuthenticated { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public string PreferredTimeZoneId { get; init; } = "UTC";
    public string PreferredCulture { get; init; } = "en-GB";

    public static GetDashboardQueryResult NotAuthenticated()
    {
        return new GetDashboardQueryResult
        {
            IsAuthenticated = false,
        };
    }
}
