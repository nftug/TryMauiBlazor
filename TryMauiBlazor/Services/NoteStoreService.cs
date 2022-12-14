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

    public double ScrollY { get; set; }

    public async Task LoadNotes()
    {
        Notes = await _repositoryService.LoadNotesAsync();
    }

    public Note? GetNote(string filename) => Notes.FirstOrDefault(x => x.Filename == filename);

    public Task<Note> GetNewNoteAsync() => _repositoryService.LoadNewNoteAsync();

    public async Task SaveNoteAsync(Note note)
    {
        var savedNote = await _repositoryService.SaveNoteAsync(note);

        Notes = Notes.Where(x => x.Filename != savedNote.Filename).ToList();
        Notes.Add(savedNote);
        Notes = Notes.OrderByDescending(x => x.Date).ToList();
    }

    public void DeleteNote(string filename)
    {
        if (!_repositoryService.IsExisted(filename)) return;
        _repositoryService.DeleteNote(filename);
        Notes = Notes.Where(x => x.Filename != filename).ToList();
    }

    public bool IsExisted(string filename) => _repositoryService.IsExisted(filename);
}
