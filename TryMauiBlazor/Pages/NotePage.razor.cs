using Microsoft.AspNetCore.Components;
using MudBlazor;
using TryMauiBlazor.Models;
using TryMauiBlazor.Services;

namespace TryMauiBlazor.Pages;

public partial class NotePage : ComponentBase
{
    [Parameter]
    public string? Filename { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    [Inject]
    private NoteStoreService NoteStoreService { get; set; } = null!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    private Note _note = null!;
    private bool _isFileExist => NoteStoreService.IsExisted(_note.Filename);
    private bool _isSaving;
    private bool _isDeleting;
    private bool _isProcessing => _isSaving || _isDeleting;

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
        _isSaving = true;
        await Task.Delay(1000);  // Dummy delay
        await NoteStoreService.SaveNoteAsync(_note);
        _isSaving = false;

        Snackbar.Add("Saved the note.", Severity.Success);
        StateHasChanged();
    }

    private async void OnDeleteButtonClicked()
    {
        if (!_isFileExist)
        {
            Snackbar.Add("The file does not exist.", Severity.Error);
            return;
        }

        bool? result = await DialogService.ShowMessageBox(
            "Confirm",
            "Are you sure to delete this note?",
            cancelText: "Cancel"
        );
        if (result == null) return;

        _isDeleting = true;
        StateHasChanged();

        await Task.Delay(1000);  // Dummy delay
        NoteStoreService.DeleteNote(_note.Filename);
        _isDeleting = false;

        Snackbar.Add("Deleted the note.", Severity.Success);
        NavigationManager.NavigateTo("/", false, true);
    }
}

