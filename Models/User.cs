namespace StudyNoteProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<Notes> Notes { get; set; } = new List<Notes>();

    }
}
