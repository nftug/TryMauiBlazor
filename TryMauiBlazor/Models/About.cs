namespace TryMauiBlazor.Models;

internal static class About
{
    public static string Title => AppInfo.Name;
    public static string Version => AppInfo.VersionString;
    public static string MoreInfoUrl => "https://mudblazor.com";
    public static string Message => "This app is written with MudBlazor and .NET MAUI.";
}
