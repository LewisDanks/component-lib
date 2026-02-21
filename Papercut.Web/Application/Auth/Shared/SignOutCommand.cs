using MediatR;
using Microsoft.AspNetCore.Identity;
using Papercut.Web.Infrastructure.Auth;

namespace Papercut.Web.Application.Auth.Shared;

public sealed record SignOutCommand : IRequest;

public sealed class SignOutCommandHandler(SignInManager<ApplicationUser> signInManager)
    : IRequestHandler<SignOutCommand>
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
    }
}
