using System.Collections.Specialized;
using System.Web;

namespace TryMauiBlazor.Services;

internal class NavigationService
{
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
    private string? _title = string.Empty;
    public string? Title
    {
        get => _title;
        private set
        {
            if (value == _title) return;
            _title = value;
            TitleChanged?.Invoke();
        }
    }

    public void SetTitle(string? title)
    {
        Title = title;
    }

    public event Action? BackButtonChanged;
    private bool _hasBackButton { get; set; }
    public bool HasBackButton
    {
        get => _hasBackButton;
        private set
        {
            if (value == _hasBackButton) return;
            _hasBackButton = value;
            BackButtonChanged?.Invoke();
        }
    }

    public void SetHasBackButton(bool value)
    {
        HasBackButton = value;
    }
}