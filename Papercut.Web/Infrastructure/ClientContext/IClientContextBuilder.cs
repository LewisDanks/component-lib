using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Papercut.Web.Infrastructure.ClientContext
{
    public interface IClientContextBuilder
    {
        Task<IClientContext> BuildAsync(PageModel pageModel, CancellationToken cancellationToken);
    }
}