using Microsoft.JSInterop;
using System.Collections.Specialized;
using System.Web;

namespace TryMauiBlazor.Services;

internal class NavigationService
{
    private IJSRuntime _jsRuntime;

    public NavigationService(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime;
    }

    public event Action? QueryChanged;
    private NameValueCollection _queries = new();
    public NameValueCollection Queries
    {
        get => _queries;
        private set
        {
            if (value == _queries) return;
            _queries = value;
            QueryChanged?.Invoke();
        }
    }

    public void SetQueries(string path)
    {
        var pathSplitted = path.Split('?');
        Queries = pathSplitted.Length > 1
            ? HttpUtility.ParseQueryString(pathSplitted[1])
            : new();
    }

    public event Action? TitleChanged;
    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        private set
        {
            if (value == _title) return;
            _title = value;
            TitleChanged?.Invoke();
        }
    }

    public void SetTitle(string title)
    {
        Title = title;
    }
}