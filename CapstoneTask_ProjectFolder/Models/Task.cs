using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneTask.Models
{
    // This class is part of the system because tasks need a complete and organized structure
    // that all features can depend on. This makes it easier to create tasks, update them,
    // filter them, and show them in different parts of the application. That’s why the Task
    // model includes fields for titles, descriptions, dates, status, priority, and ownership.
    public class Task
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public short Status { get; set; }
        public byte Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsDeleted { get; set; }
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}