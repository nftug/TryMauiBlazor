using Microsoft.AspNetCore.Components;
using TryMauiBlazor.Services;

namespace TryMauiBlazor.Shared;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject] private LayoutService LayoutService { get; set; } = null!;

    protected override void OnInitialized()
    {
        LayoutService.MajorUpdateOccurred += LayoutServiceOnMajorUpdateOccurred;
        ApplyUserPreferences();

        base.OnInitialized();
    }

    private void ApplyUserPreferences()
    {
        var currentTheme = Application.Current?.RequestedTheme;
        var defaultDarkMode = currentTheme == AppTheme.Dark;
        LayoutService.ApplyUserPreferences(defaultDarkMode);
    }

    public void Dispose()
    {
        LayoutService.MajorUpdateOccurred -= LayoutServiceOnMajorUpdateOccurred;
        GC.SuppressFinalize(this);
    }

    private void LayoutServiceOnMajorUpdateOccurred(object? sender, EventArgs e) => StateHasChanged();
}
