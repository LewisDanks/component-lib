namespace Papercut.Web.Infrastructure.Localization;

public static class UserPreferenceCatalog
{
    public static readonly IReadOnlyList<PreferenceOption> TimeZoneOptions =
    [
        new("UTC", "UTC"),
        new("Europe/London", "Europe/London"),
        new("America/New_York", "America/New_York"),
        new("America/Los_Angeles", "America/Los_Angeles"),
        new("Asia/Tokyo", "Asia/Tokyo"),
    ];

    public static readonly IReadOnlyList<PreferenceOption> CultureOptions =
    [
        new("en-GB", "English (UK)"),
        new("en-US", "English (US)"),
        new("fr-FR", "French (France)"),
        new("de-DE", "German (Germany)"),
        new("ja-JP", "Japanese (Japan)"),
    ];

    public static bool IsValidTimeZone(string? value)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               TimeZoneOptions.Any(option => string.Equals(option.Value, value, StringComparison.Ordinal));
    }

    public static bool IsValidCulture(string? value)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               CultureOptions.Any(option => string.Equals(option.Value, value, StringComparison.Ordinal));
    }
}

public sealed record PreferenceOption(string Value, string Label);
