using Microsoft.EntityFrameworkCore;
using CapstoneTask.Models;
using TaskModel = CapstoneTask.Models.Task;

namespace CapstoneTask.Data
{
    // The system uses this class because it needs one place to manage how information is stored
    // and retrieved. Keeping everything in a single location makes the application easier to
    // maintain and ensures that all features use the same data sources. That’s why this context
    // provides access to users, projects, and tasks like below.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
    }
}