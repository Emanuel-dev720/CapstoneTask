using Microsoft.AspNetCore.Mvc;
using CapstoneTask.Data;
using System.Linq;

namespace CapstoneTask.Controllers
{
    // In this controller, the action exists because the system needs a clear way to gather project
    // information before showing it to the user. Preparing the data here keeps the rest of the
    // application simple and avoids repeating the same work in multiple places. That’s why
    // we collect project details, owner names, visibility, and task counts in the action method.
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var projects =
                from p in _context.Projects
                where !p.IsDeleted
                select new
                {
                    p.ProjectId,
                    p.Title,
                    p.Description,

                    OwnerName = _context.Users
                        .Where(u => u.UserId == p.OwnerUserId)
                        .Select(u => u.DisplayName ?? u.UserName)
                        .FirstOrDefault() ?? "Unknown User",

                    Visibility = p.IsPublic ? "Public" : "Private",

                    TaskCount = _context.Tasks
                        .Where(t => t.ProjectId == p.ProjectId && !t.IsDeleted)
                        .Count()
                };

            ViewBag.Projects = projects.ToList();
            return View();
        }
    }
}