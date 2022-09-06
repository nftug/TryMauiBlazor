namespace TryMauiBlazor.Models;

internal static class ThisDevice
{
    public static bool IsAndroid => DeviceInfo.Current.Platform == DevicePlatform.Android;
    public static bool IsIOS => DeviceInfo.Current.Platform == DevicePlatform.iOS;
    public static bool IsWinUI => DeviceInfo.Current.Platform == DevicePlatform.WinUI;
}