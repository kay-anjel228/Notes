using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyNoteProject.Models;
using StudyNoteProject.Services;
using StudyNoteService.Services;
using System.Collections.Generic;

namespace StudyNoteProject.Pages
{
    public class MyNotesModel : PageModel
    {
        private readonly INoteService _noteService;
        private readonly ICurrentUserService _currentUserService;

        public MyNotesModel(INoteService noteService, ICurrentUserService currentUserService)
        {
            _noteService = noteService;
            _currentUserService = currentUserService;
        }

        public List<Notes> UserNotes { get; set; } = new List<Notes>();
        public string CurrentUserName { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var user = _currentUserService.GetCurrentUser(HttpContext);
            if (user != null)
            {
                CurrentUserName = user.Name;
                UserNotes = _noteService.GetNoteByAuthorId(user.Id);
            }

            return Page();
        }

        public IActionResult OnPostDeleteNote(int noteId)
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var userId = _currentUserService.GetCurrentUserId(HttpContext);
            var note = _noteService.GetNoteById(noteId);

            if (note != null && note.AuthorId == userId!.Value)
            {
                _noteService.DeleteNote(note);
            }

            return RedirectToPage();
        }
    }
}
