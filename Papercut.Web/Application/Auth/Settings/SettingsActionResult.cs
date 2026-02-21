namespace Papercut.Web.Application.Auth.Settings;

public sealed class SettingsActionResult
{
    public required string Outcome { get; init; }
    public required string Message { get; init; }

    public static SettingsActionResult Success(string message)
    {
        return new SettingsActionResult
        {
            Outcome = "success",
            Message = message,
        };
    }

    public static SettingsActionResult Invalid(string message)
    {
        return new SettingsActionResult
        {
            Outcome = "invalid",
            Message = message,
        };
    }
}
