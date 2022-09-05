using TryMauiBlazor.Models;

namespace TryMauiBlazor.Services;

internal class NoteStoreService
{
    private NoteRepositoryService _repositoryService;

    public NoteStoreService(NoteRepositoryService repositoryService)
    {
        _repositoryService = repositoryService;
    }

    public List<Note> Notes { get; set; } = new();

    public async Task LoadNotes()
    {
        Notes.Clear();
        await foreach (var note in _repositoryService.LoadNotesAsync())
            Notes.Add(note);
    }

    public Note? GetNote(string filename) => Notes.FirstOrDefault(x => x.Filename == filename);

    public Task<Note> GetNewNoteAsync() => _repositoryService.LoadNewNoteAsync();

    public async Task SaveNoteAsync(Note note)
    {
        var savedNote = await _repositoryService.SaveNoteAsync(note);

        if (Notes.Any(x => x.Filename == note.Filename))
        {
            int targetIndex = Notes.FindIndex(x => x.Filename == note.Filename);
            Notes[targetIndex] = savedNote;
        }
        else
        {
            Notes.Add(savedNote);
        }
    }

    public void DeleteNote(string filename)
    {
        if (!_repositoryService.IsExisted(filename)) return;
        _repositoryService.DeleteNote(filename);
        Notes = Notes.Where(x => x.Filename != filename).ToList();
    }

    public bool IsExisted(string filename) => _repositoryService.IsExisted(filename);
}
