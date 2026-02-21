using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace Papercut.Web.Infrastructure.ClientContext
{
    public class ClientContextPageFilter : IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var executed = await next();

            if(executed.Exception is not null && !executed.ExceptionHandled)
            {
                return;
            }

            if(context.HandlerInstance is not PageModel pageModel)
            {
                return;
            }

            var attr = pageModel.GetType()
                .GetCustomAttributes(typeof(ClientContextAttribute), true)
                .FirstOrDefault() as ClientContextAttribute;
            
            if(attr is null)
            {
                return; 
            }

            var builder = context.HttpContext.RequestServices.GetRequiredService(attr.ClientContextType) as IClientContextBuilder;
            var clientContext = await builder!.BuildAsync(pageModel, context.HttpContext.RequestAborted);
            var accessor = context.HttpContext.RequestServices.GetRequiredService<IClientContextAccessor>();
            accessor.Set(clientContext);
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;
    }
}