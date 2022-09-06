using TryMauiBlazor.Extensions;
using TryMauiBlazor.Models;

namespace TryMauiBlazor.Services;

internal class NoteRepositoryService
{
    public async Task<Note> LoadNoteAsync(string filename, DateTime? date = null)
    {
        var noteModel = new Note();
        noteModel.Filename = filename;
        var noteFullPath = GetFullPath(filename);

        if (date != null || File.Exists(noteFullPath))
        {
            noteModel.Date = date ?? File.GetCreationTime(noteFullPath);
            var lines = await File.ReadAllLinesAsync(noteFullPath);

            noteModel.Text = string.Join(Environment.NewLine, lines);
            noteModel.Title = noteModel.Text.GetFirstLine();
        }

        return noteModel;
    }

    public async Task<Note> LoadNewNoteAsync()
    {
        string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";
        return await LoadNoteAsync(randomFileName);
    }

    public async Task<List<Note>> LoadNotesAsync()
    {
        var items = Directory
           .EnumerateFiles(AppDataPath, "*.notes.txt")
           .Select(filename => new
           {
               filename = Path.GetFileName(filename),
               date = File.GetCreationTime(GetFullPath(filename))
           })
           .OrderByDescending(x => x.date);

        var result = new List<Note>();

        foreach (var item in items)
        {
            var note = await LoadNoteAsync(item.filename, item.date);
            result.Add(note);
        }

        return result;
    }

    public async Task<Note> SaveNoteAsync(Note note)
    {
        await File.WriteAllTextAsync(GetFullPath(note.Filename), note.Text);
        return await LoadNoteAsync(note.Filename);
    }

    public void DeleteNote(string filename)
    {
        File.Delete(GetFullPath(filename));
    }

    public bool IsExisted(string filename) => File.Exists(GetFullPath(filename));

    private string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    private string GetFullPath(string filename) => Path.Combine(AppDataPath, filename);
}

