using TryMauiBlazor.Models;

namespace TryMauiBlazor.Services;

internal class NoteRepositoryService
{
    public async Task<Note> LoadNoteAsync(string filename, DateTime? date = null)
    {
        var noteModel = new Note();
        noteModel.Filename = filename;

        if (date != null || File.Exists(filename))
        {
            noteModel.Date = date ?? File.GetCreationTime(filename);
            noteModel.Text = await File.ReadAllTextAsync(filename);
        }

        return noteModel;
    }

    public async Task<Note> LoadNewNoteAsync()
    {
        string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";
        return await LoadNoteAsync(Path.Combine(appDataPath, randomFileName));
    }

    public async IAsyncEnumerable<Note> LoadNotesAsync()
    {
        var items = Directory
           .EnumerateFiles(appDataPath, "*.notes.txt")
           .Select(filename => new
           {
               filename,
               date = File.GetCreationTime(filename)
           })
           .OrderBy(x => x.date);

        foreach (var item in items)
            yield return await LoadNoteAsync(item.filename, item.date);
    }

    public async Task SaveNoteAsync(Note note)
    {
        await File.WriteAllTextAsync(note.Filename, note.Text);
    }

    public void DeleteNote(string filename)
    {
        File.Delete(filename);
    }

    public bool IsExisted(string filename) => File.Exists(filename);

    private string appDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
}

