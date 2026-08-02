using System;

namespace CapstoneTask.Models
{
    // The system includes this class because project information needs a clear structure that
    // other features can rely on. This helps the application keep track of who owns a project,
    // what it contains, and whether it is visible to others. That’s why the Project model stores
    // identifiers, titles, descriptions, visibility settings, and timestamps like below.
    public class Project
    {
        public int ProjectId { get; set; }
        public int OwnerUserId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}