using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TryMauiBlazor.Models;
using TryMauiBlazor.Services;
using TryMauiBlazor.Shared;
using Color = MudBlazor.Color;

namespace TryMauiBlazor.Pages;

public partial class NotePage : ComponentBase
{
    [Parameter]
    public string? Filename { get; set; }

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
                await (Toast.Make("Cannot find the note.")).Show();
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
        await NoteStoreService.SaveNoteAsync(_note);
        _isSaving = false;

        await (Toast.Make("Saved the note.", ToastDuration.Short)).Show();
        StateHasChanged();
    }

    private async void OnDeleteButtonClicked()
    {
        if (!_isFileExist)
        {
            await (Toast.Make("The file does not exist")).Show();
            return;
        }

        var parameters = new DialogParameters()
        {
            ["ContentText"] = "Are you sure to delete this note?",
            ["ButtonText"] = "Delete",
            ["Color"] = Color.Error,
            ["Variant"] = Variant.Filled,
            ["StartIcon"] = Icons.Filled.Delete
        };
        var dialog = DialogService.Show<Dialog>("Delete", parameters);
        var result = await dialog.Result;
        if (result.Cancelled) return;

        _isDeleting = true;
        StateHasChanged();

        NoteStoreService.DeleteNote(_note.Filename);
        _isDeleting = false;

        await (Toast.Make("Deleted the note.", ToastDuration.Short)).Show();
        NavigationManager.NavigateTo("/", false, true);
    }
}

