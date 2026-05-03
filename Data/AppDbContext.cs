using Microsoft.EntityFrameworkCore;
using StudyNoteProject.Models;
using System.Collections.Generic;


namespace StudyNoteProject.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
