namespace TryMauiBlazor.Extensions;

internal static class StreamExtension
{
    public static async Task<string> ConvertToBase64StringAsync(this Stream stream)
    {
        byte[] bytes;
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            bytes = memoryStream.ToArray();
        }

        return await Task.Run(() => Convert.ToBase64String(bytes));
    }
}
