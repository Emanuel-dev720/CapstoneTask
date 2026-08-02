namespace CapstoneTask.Models
{
    // This simplified model exists because the system needs a lighter version of task data
    // when showing lists. Using a smaller structure keeps the application fast and avoids
    // loading information that the list view does not need. That’s why TaskListItem includes
    // only the fields required for displaying task summaries.
    public class TaskListItem
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = "";
        public string ProjectName { get; set; } = "";
        public string Status { get; set; } = "";
        public string Priority { get; set; } = "";
        public string? DueDate { get; set; }
        public string AssignedUser { get; set; } = "";
    }
}