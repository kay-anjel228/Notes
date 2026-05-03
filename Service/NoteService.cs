using StudyNoteProject.Data;
using StudyNoteProject.Models;
using StudyNoteService.Services;
namespace StudyNoteProject.Services
{
    public class NoteService : INoteService
    {
        private readonly AppDbContext _context;
        public NoteService(AppDbContext context)
        {
            _context = context;
        }
        public List<Notes> GetAllNotes()
        {
            return _context.Notes
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.Id)
                .ToList();
        }
        public List<Notes> GetNoteByAuthorId(int authorId)
        {
            return _context.Notes
                .Include(p => p.Author)
                .Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.Id)
                .ToList();
        }
        public Notes? GetNoteById(int id)
        {
            return _context.Notes
                .Include(p => p.Author)
                .FirstOrDefault(p => p.Id == id);
        }
        public void AddNote(Notes project)
        {
            _context.Notes.Add(project);
            _context.SaveChanges();
        }
        public void UpdateNote(Notes project)
        {
            _context.Notes.Update(project);
            _context.SaveChanges();
        }
        public void DeleteNote(Notes project)
        {
            _context.Notes.Remove(project);
            _context.SaveChanges();
        }
        public bool NoteExists(int id)
        {
            return _context.Notes.Any(p => p.Id == id);
        }


    }
}

