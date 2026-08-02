# CapstoneTask — Team-Based Full Stack Task & Project Management System

A full-stack ASP.NET Core MVC application built as part of my Moraine Park Technical College capstone project. The system allows users to manage tasks, projects, filtering, soft delete, CSV export, and more.

# Team Members
- Devon Matter (Project Manager / Documentation)
- Emanuel Osorio (Backend)
- Jared Gamache (Frontend / QA)
- Trevor Tourdot (Database)

# Features
- Task CRUD (Create, Read, Update, Edit, Soft Delete)
- Project organization
- Filtering & search
- Soft delete system
- CSV export
- MVC architecture
- SQL Server integration
- Entity Framework Core tracking fields

# Tech Stack
- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- HTML/CSS/JavaScript
- Git/GitHub

# My Contributions
- Built core backend architecture & logic for task CRUD, filtering, soft delete, and CSV export.
- Integrated SQL Server with EF Core, including implementing key tracking fields (Status, Priority, DueDate, UpdatedAt, CompletedAt, IsDeleted, RowVersion).
- Ensured backend–database–frontend alignment through testing, debugging, and workflow coordination.
- Wrote backend documentation and contributed to final project documentation and presentation.
- Managed Git/GitHub version control for backend work, including resolving merge conflicts and maintaining clean controller/model structure.

# Instructions
- Clone the repository and open the solution in Visual Studio 2022.
- Add your SQL Server connection string to appsettings.json (this file is empty by default).
- Use this if you're on the default LocalDB setup: "ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CapstoneTaskDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
- If you use SQL Server Express, change the server name: DefaultConnection: Server=YOURPCNAME\\SQLEXPRESS;Database=CapstoneTaskDB;Trusted_Connection=True;TrustServerCertificate=True;
- Press Start in Visual Studio to run the application.
- The app will open automatically in your browser at a local https://localhost:xxxx/ address.
- Navigate to https://localhost:xxxx/Tasks in your browser.
- You can now create tasks, edit tasks, soft delete tasks, filter/search, export CSV, and view projects.

# Screenshots
