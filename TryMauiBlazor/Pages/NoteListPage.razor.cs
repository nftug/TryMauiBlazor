using Microsoft.AspNetCore.Components;
using TryMauiBlazor.Services;

namespace TryMauiBlazor.Pages;

public partial class NoteListPage : ComponentBase
{
    [Inject]
    private NoteStoreService NoteStoreService { get; set; } = null!;

    private bool _isLoading = false;

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
}
