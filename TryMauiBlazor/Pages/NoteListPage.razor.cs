using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using TryMauiBlazor.Services;

namespace TryMauiBlazor.Pages;

public partial class NoteListPage : ComponentBase
{
    [Inject]
    private NoteStoreService NoteStoreService { get; set; } = null!;
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    private bool _isLoading = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender) return;

        await JSRuntime.InvokeVoidAsync("setScrollY", NoteStoreService.ScrollY);
        NoteStoreService.ScrollY = 0;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        InvokeAsync(async () => await LoadNotes());
    }

    private async Task LoadNotes()
    {
        if (NoteStoreService.Notes.Any()) return;

        _isLoading = true;
        StateHasChanged();

        await NoteStoreService.LoadNotes();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task GoToNote(string? filename)
    {
        var result = await JSRuntime.InvokeAsync<JsonElement>("getScrollY");
        double.TryParse(result.GetRawText(), out double scrollY);
        NoteStoreService.ScrollY = scrollY;

        NavigationManager.NavigateTo($"/note/{filename}");
    }
}
