using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyNoteProject.Models;
using StudyNoteProject.Services;
using StudyNoteService.Services;
using System;

namespace StudyNoteProject.Pages
{
    public class AddNotesModel : PageModel
    {
        private readonly INoteService _noteService;
        private readonly ICurrentUserService _currentUserService;

        public AddNotesModel(INoteService noteService, ICurrentUserService currentUserService)
        {
            _noteService = noteService;
            _currentUserService = currentUserService;
        }

        [BindProperty]
        public Notes NoteForm { get; set; } = new Notes();

        public string PageTitle { get; set; } = "Добавить заметку";

        public IActionResult OnGet(int? id)
        {

            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            if (id.HasValue)
            {
                var note = _noteService.GetNoteById(id.Value);
                var userId = _currentUserService.GetCurrentUserId(HttpContext);

                if (note != null && note.AuthorId == userId!.Value)
                {
                    NoteForm = note;
                    PageTitle = "Редактировать заметку";
                }
                else
                {
                    return RedirectToPage("/MyNotes");
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!_currentUserService.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var userId = _currentUserService.GetCurrentUserId(HttpContext);
            if (userId == null) return RedirectToPage("/Index");

            if (NoteForm.Id == 0)
            {
                NoteForm.AuthorId = userId.Value;
                NoteForm.CreatedAt = DateTime.Now;

                _noteService.AddNote(NoteForm);
            }
            else
            {
                var existingNote = _noteService.GetNoteById(NoteForm.Id);

                if (existingNote != null && existingNote.AuthorId == userId.Value)
                {
                    existingNote.Title = NoteForm.Title;
                    existingNote.Category = NoteForm.Category;
                    existingNote.Description = NoteForm.Description;

                    _noteService.UpdateNote(existingNote);
                }
            }

            return RedirectToPage("/MyNotes");
        }
    }
}