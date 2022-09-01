using Microsoft.AspNetCore.Components;
using MudBlazor;
using TryMauiBlazor.Services;

namespace TryMauiBlazor.Shared;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject] private LayoutService LayoutService { get; set; } = null!;

    private MudThemeProvider? _mudThemeProvider = null!;

    protected override void OnInitialized()
    {
        LayoutService.MajorUpdateOccurred += LayoutServiceOnMajorUpdateOccurred;
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await ApplyUserPreferences();
            StateHasChanged();
        }
    }

    private async Task ApplyUserPreferences()
    {
        if (_mudThemeProvider == null) return;
        var defaultDarkMode = await _mudThemeProvider.GetSystemPreference();
        LayoutService.ApplyUserPreferences(defaultDarkMode);
    }


    public void Dispose()
    {
        LayoutService.MajorUpdateOccurred -= LayoutServiceOnMajorUpdateOccurred;
        GC.SuppressFinalize(this);
    }

    private void LayoutServiceOnMajorUpdateOccurred(object? sender, EventArgs e) => StateHasChanged();
}
