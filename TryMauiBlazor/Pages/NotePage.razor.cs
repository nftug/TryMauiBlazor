using Microsoft.AspNetCore.Components;
using MudBlazor;
using TryMauiBlazor.Models;
using TryMauiBlazor.Services;

namespace TryMauiBlazor.Pages;

public partial class NotePage : ComponentBase
{
    [Parameter]
    [SupplyParameterFromQuery]
    public string? Filename { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    [Inject]
    private NoteStoreService NoteStoreService { get; set; } = null!;

    private Note _note = null!;
    private bool _isFileExist => NoteStoreService.IsExisted(_note.Filename);

    protected override async Task OnParametersSetAsync()
    {
        if (Filename != null)
        {
            var note = NoteStoreService.GetNote(Filename);
            if (note == null)
            {
                Snackbar.Add("Cannot find the note.", Severity.Error);
                _note = await NoteStoreService.GetNewNoteAsync();
            }
            else
            {
                _note = note;
            }
        }
        else
        {
            _note = await NoteStoreService.GetNewNoteAsync();
        }

        StateHasChanged();

        await base.OnParametersSetAsync();
    }

    private async Task OnSaveButtonClicked()
    {
        await NoteStoreService.SaveNoteAsync(_note);
        Snackbar.Add("Saved the note.", Severity.Success);
        StateHasChanged();
    }

    private async void OnDeleteButtonClicked()
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm",
            "Are you sure to delete this note?",
            cancelText: "Cancel"
        );
        if (result == null) return;

        if (_isFileExist)
            NoteStoreService.DeleteNote(_note.Filename);

        _note = await NoteStoreService.GetNewNoteAsync();
        Snackbar.Add("Deleted the note.", Severity.Success);
        StateHasChanged();
    }
}

