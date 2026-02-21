namespace Papercut.Web.Infrastructure.ClientContext
{
    public interface IClientContextAccessor
    {
        void Set(IClientContext clientContext);
        IClientContext GetRequired();
        bool TryGet(out IClientContext? clientContext);
    }
}