using Microsoft.AspNetCore.Components;
using TryMauiBlazor.Services;

namespace TryMauiBlazor.Pages;

public partial class NoteListPage : ComponentBase
{
    [Inject]
    private NoteStoreService NoteStoreService { get; set; } = null!;

    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadNotes();
    }

    private async Task LoadNotes()
    {
        if (NoteStoreService.Notes.Any()) return;

        _isLoading = true;
        await Task.Delay(2000);  // Dummy delay
        await NoteStoreService.LoadNotes();
        _isLoading = false;
    }
}
