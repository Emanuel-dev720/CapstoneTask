using Microsoft.AspNetCore.Mvc;
using CapstoneTask.Data;
using CapstoneTask.Models;
using System;
using System.Linq;
using TaskModel = CapstoneTask.Models.Task;

// This controller is included because the system needs one place to handle everything related
// to tasks. Keeping all task actions together makes the application easier to understand and
// ensures that task behavior stays consistent. That’s why this controller manages listing,
// creating, editing, and exporting tasks.
public class TasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Before the task list can be shown, the system needs to prepare information that is easy
    // for the user to read. This includes combining task data with project names and user names
    // so the list makes sense at a glance. That’s why this method gathers, filters, and formats
    // task information before sending it to the view.
    public IActionResult Index(string search, int? status, int? priority, int? projectId)
    {
        var query =
            from t in _context.Tasks
            join p in _context.Projects on t.ProjectId equals p.ProjectId
            join u in _context.Users on t.CreatedByUserId equals u.UserId
            where !t.IsDeleted
            select new
            {
                t.TaskId,
                t.Title,
                ProjectId = p.ProjectId,
                ProjectTitle = p.Title,
                t.Status,
                t.Priority,
                t.DueDate,
                AssignedUserName = u.DisplayName ?? u.UserName
            };

        // The system applies filters here because it needs a simple and predictable way to narrow
        // down the list of tasks. Doing this in one place keeps the behavior consistent and avoids
        // repeating the same logic elsewhere. That’s why search text, status, priority, and project
        // filters are handled in this method.
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.ToLower();
            query = query.Where(x =>
                (x.Title != null && x.Title.ToLower().Contains(term)) ||
                (x.ProjectTitle != null && x.ProjectTitle.ToLower().Contains(term)) ||
                (x.AssignedUserName != null && x.AssignedUserName.ToLower().Contains(term))
            );
        }

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(x => x.Priority == priority.Value);

        if (projectId.HasValue)
            query = query.Where(x => x.ProjectId == projectId.Value);

        var raw = query.ToList();

        // The system then converts raw values into readable labels here because it helps the user
        // understand the information more easily. This keeps the view clean and avoids making
        // the user interpret numbers or codes. That’s why we turn status, priority, and dates
        // into friendly text before sending them to the page.
        var tasks = raw.Select(x => new TaskListItem
        {
            TaskId = x.TaskId,
            Title = x.Title ?? "",
            ProjectName = x.ProjectTitle,
            Status = x.Status == 0 ? "Not Started"
                   : x.Status == 1 ? "In Progress"
                   : x.Status == 2 ? "Completed"
                   : "Unknown",
            Priority = x.Priority == 1 ? "Low"
                     : x.Priority == 2 ? "Medium"
                     : x.Priority == 3 ? "High"
                     : "Unknown",
            DueDate = x.DueDate?.ToShortDateString(),
            AssignedUser = x.AssignedUserName
        }).ToList();

        ViewBag.Tasks = tasks;

        // The system keeps track of the user’s filter choices so the page can remember them
        // when it reloads. This makes the experience smoother and prevents the user from having
        // to re-enter the same information. That’s why we store the filter values in ViewBag.
        ViewBag.Search = search;
        ViewBag.Status = status;
        ViewBag.Priority = priority;
        ViewBag.ProjectId = projectId;

        ViewBag.Projects = _context.Projects
            .Where(p => !p.IsDeleted)
            .Select(p => new { p.ProjectId, p.Title })
            .ToList();

        return View();
    }

    // The system then loads project and user lists here because the Create Task form needs these
    // options ready before it can be shown. This makes sure the form is complete and prevents
    // missing information. That’s why this method prepares the supporting data first.
    public IActionResult Create()
    {
        ViewBag.Projects = _context.Projects
            .Where(p => !p.IsDeleted)
            .Select(p => new { p.ProjectId, p.Title })
            .ToList();

        ViewBag.Users = _context.Users
            .Where(u => u.IsActive)
            .Select(u => new { u.UserId, u.DisplayName })
            .ToList();

        return View();
    }

    // When a new task is submitted, the system needs to fill in important details that the
    // user does not provide directly, such as when the task was created and who created it.
    // This keeps the stored information complete and reliable. That’s why this method sets
    // fields like CreatedAt, IsDeleted, and CreatedByUserId before saving the task like below.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TaskModel model, int CreatedByUserId)
    {
        if (ModelState.IsValid)
        {
            model.CreatedAt = DateTime.UtcNow;
            model.IsDeleted = false;
            model.CreatedByUserId = CreatedByUserId;

            _context.Tasks.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // If something goes wrong, the system still needs to show the form with all its
        // options so the user can fix the issue. This prevents the form from breaking or
        // losing information. That’s why we reload the project and user lists here.
        ViewBag.Projects = _context.Projects
            .Where(p => !p.IsDeleted)
            .Select(p => new { p.ProjectId, p.Title })
            .ToList();

        ViewBag.Users = _context.Users
            .Where(u => u.IsActive)
            .Select(u => new { u.UserId, u.DisplayName })
            .ToList();

        return View(model);
    }

    // The system loads the task and its supporting information here because the Edit form
    // needs everything ready before it can be shown. This keeps the form complete and avoids
    // missing options. That’s why this method retrieves the task, project list, and user list.
    public IActionResult Edit(int id)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);
        if (task == null)
            return NotFound();

        ViewBag.Projects = _context.Projects
            .Where(p => !p.IsDeleted)
            .Select(p => new { p.ProjectId, p.Title })
            .ToList();

        ViewBag.Users = _context.Users
            .Where(u => u.IsActive)
            .Select(u => new { u.UserId, u.DisplayName })
            .ToList();

        ViewBag.Task = task;

        return View();
    }

    // When updating a task, the system needs to change only the information the user intended
    // to modify. This protects important details, such as when the task was originally created.
    // That’s why this method updates only the editable fields and leaves the rest untouched.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, TaskModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Projects = _context.Projects
                .Where(p => !p.IsDeleted)
                .Select(p => new { p.ProjectId, p.Title })
                .ToList();

            ViewBag.Users = _context.Users
                .Where(u => u.IsActive)
                .Select(u => new { u.UserId, u.DisplayName })
                .ToList();

            ViewBag.Task = model;

            return View();
        }

        var existing = _context.Tasks.FirstOrDefault(t => t.TaskId == id);
        if (existing == null)
            return NotFound();

        existing.Title = model.Title;
        existing.Description = model.Description;
        existing.ProjectId = model.ProjectId;
        existing.Status = model.Status;
        existing.Priority = model.Priority;
        existing.DueDate = model.DueDate;

        // The system updates the assigned user here because tasks need to show who is
        // responsible for them. This keeps the information accurate without changing how
        // the data is stored behind the scenes. That’s why we read the AssignedUserId from
        // the form and save it into CreatedByUserId.
        if (int.TryParse(Request.Form["AssignedUserId"], out var assignedUserId))
        {
            existing.CreatedByUserId = assignedUserId;
        }

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // The system then uses soft deletion because it allows tasks to be hidden without removing
    // them permanently. This helps preserve history while keeping deleted tasks out of the
    // main views. That’s why this method marks a task as deleted instead of removing it.
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);
        if (task == null)
            return NotFound();

        task.IsDeleted = true;
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // A CSV export function is then provided as it gives users a simple way to download and
    // share task information. That’s why this method gathers task details, formats them, and returns
    // them as a downloadable file.
    public IActionResult ExportCsv()
    {
        var data = (
            from t in _context.Tasks
            join p in _context.Projects on t.ProjectId equals p.ProjectId
            join u in _context.Users on t.CreatedByUserId equals u.UserId
            where !t.IsDeleted
            select new
            {
                ProjectName = p.Title,
                TaskName = t.Title,
                TaskDescription = t.Description,
                Status = t.Status == 0 ? "Not Started"
                        : t.Status == 1 ? "In Progress"
                        : t.Status == 2 ? "Completed"
                        : "Unknown",
                Priority = t.Priority == 1 ? "Low"
                         : t.Priority == 2 ? "Medium"
                         : t.Priority == 3 ? "High"
                         : "Unknown",
                DueDate = t.DueDate,
                CreatedDate = t.CreatedAt,
                AssignedUser = u.DisplayName ?? u.UserName
            }
        ).ToList();

        var csv = new System.Text.StringBuilder();

        csv.AppendLine("ProjectName,TaskName,TaskDescription,Status,Priority,DueDate,CreatedDate,AssignedUser");
        foreach (var row in data)
        {
            string Escape(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return "";

                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }

            csv.AppendLine(string.Join(",",
                Escape(row.ProjectName),
                Escape(row.TaskName),
                Escape(row.TaskDescription),
                Escape(row.Status),
                Escape(row.Priority),
                row.DueDate?.ToString("yyyy-MM-dd") ?? "",
                row.CreatedDate.ToString("yyyy-MM-dd") ?? "",
                Escape(row.AssignedUser)
            ));
        }

        // Finally, the system returns the completed CSV file here as users may want to download
        // or share their task information outside the application. We simply convert the CSV text
        // into a file and send it back as a download to be able to open in another program (like Excel for example).
        var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", "tasks.csv");
    }
}