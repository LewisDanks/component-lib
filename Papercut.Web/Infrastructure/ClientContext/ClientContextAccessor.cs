namespace Papercut.Web.Infrastructure.ClientContext
{
    public class ClientContextAccessor : IClientContextAccessor
    {
        private IClientContext? _clientContext;

        public void Set(IClientContext clientContext)
        {
            _clientContext = clientContext ?? throw new ArgumentNullException(nameof(clientContext));
        }

        public IClientContext GetRequired()
        {
            return _clientContext ?? throw new InvalidOperationException(
                @"ClientContext was not set for this request. Ensure the page has [ClientContext(...)] 
                and the filter is registered.");
        }

        public bool TryGet(out IClientContext? clientContext)
        {
            clientContext = _clientContext;
            return clientContext is not null;
        }
    }
}