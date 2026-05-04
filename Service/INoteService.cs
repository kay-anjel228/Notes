using StudyNoteProject.Models;

namespace StudyNoteService.Services
{
    public interface INoteService
    {
        List<Notes> GetAllNotes();
        List<Notes> GetNoteByAuthorId(int authorId);
        Notes? GetNoteById(int auhorId);
        void AddNote(Notes project);
        void UpdateNote(Notes project);
        void DeleteNote(Notes project);
        bool NoteExists(int id);

    }
}
