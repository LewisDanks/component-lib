namespace Papercut.Web.Infrastructure.ClientContext;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class ClientContextAttribute : Attribute
{
    public ClientContextAttribute(Type clientContextType)
    {
        if (!typeof(IClientContextBuilder).IsAssignableFrom(clientContextType))
        {
            throw new ArgumentException(
                $"{clientContextType.Name} must implement {nameof(IClientContextBuilder)}.",
                nameof(clientContextType));
        }

        ClientContextType = clientContextType;
    }

    public Type ClientContextType { get; }
}
