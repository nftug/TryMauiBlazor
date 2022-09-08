using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TryMauiBlazor.Extensions;
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

    private Note _note = new();
    private bool _isFileExist => NoteStoreService.IsExisted(_note.Filename);
    private bool _isSaving;
    private bool _isDeleting;
    private bool _isProcessing => _isSaving || _isDeleting;
    private bool _isLoading = true;
    private string? _imageBase64Source;

    private string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private string LocalImagePath => Path.Combine(AppDataPath, $"{_note.Filename}.img");

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        _isLoading = true;
        StateHasChanged();

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

        if (File.Exists(LocalImagePath))
            _imageBase64Source = await File.ReadAllTextAsync(LocalImagePath);

        _isLoading = false;
        StateHasChanged();
    }

    private async Task OnSaveButtonClicked()
    {
        _isSaving = true;
        await NoteStoreService.SaveNoteAsync(_note);

        // save the image file into local storage
        if (_imageBase64Source != null)
        {
            await File.WriteAllTextAsync(LocalImagePath, _imageBase64Source);
        }
        else if (_imageBase64Source == null && File.Exists(LocalImagePath))
        {
            File.Delete(LocalImagePath);
        }

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
        if (File.Exists(LocalImagePath))
            File.Delete(LocalImagePath);

        _isDeleting = false;

        await (Toast.Make("Deleted the note.", ToastDuration.Short)).Show();
        NavigationManager.NavigateTo("/", false, true);
    }

    private async void TakePhoto()
    {
        var image = await MediaPicker.Default.CapturePhotoAsync();
        if (image == null) return;

        await SetImageSource(image);
    }

    private async void PickImage()
    {
        var customFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "image/jpeg", "image/png" } },
                { DevicePlatform.WinUI, new[] { ".jpg", ".jpeg", ".png" } },
            });
        PickOptions options = new()
        {
            PickerTitle = "Please select an image file.",
            FileTypes = customFileType,
        };

        var image = await FilePicker.Default.PickAsync(options);
        if (image == null) return;

        await SetImageSource(image);
    }

    private void DeleteImage()
    {
        _imageBase64Source = null;
    }

    private async Task SetImageSource(FileResult image)
    {
        if (image == null) return;

        _isLoading = true;
        StateHasChanged();

        using Stream sourceStream = await image.OpenReadAsync();
        var imageBase64 = await sourceStream.ConvertToBase64StringAsync();
        _imageBase64Source = string.Format("data:image/jpeg;base64,{0}", imageBase64);

        _isLoading = false;
        StateHasChanged();
    }
}
