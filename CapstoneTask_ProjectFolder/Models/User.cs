using System;

namespace CapstoneTask.Models
{
    // This class is included because the system needs a clear and dependable way to store
    // information about each person who uses the application. Having one place for these
    // details makes it easier for tasks, projects, and other features to refer back to the
    // correct user. That’s why we define a User model with identifiers, names, activity
    // status, and timestamps like below.
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? DisplayName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}