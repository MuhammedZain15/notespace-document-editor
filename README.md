# NoteSpace — Personal Document Editor

NoteSpace is a secure, responsive personal writing workspace built with ASP.NET Core MVC. Users can register, sign in, create rich-text documents, search their library, edit content, and delete documents that they own.

## Highlights

- Account registration and login with ASP.NET Core Identity
- Private, per-user document library
- Server-side ownership checks on every edit and delete operation
- Rich-text editing powered by TinyMCE
- Create, search, edit, and delete workflows
- Anti-forgery protection and validated view models
- Async Entity Framework Core queries and SQL Server persistence
- Responsive custom interface with empty and success states
- Health check and GitHub Actions build workflow
- Automated view-model validation tests with xUnit

## Tech stack

- .NET 10 and ASP.NET Core MVC
- Razor Views and Bootstrap
- ASP.NET Core Identity
- Entity Framework Core 10
- SQL Server / SQL Server LocalDB
- TinyMCE rich-text editor

## Security design

Document ownership is never trusted from a hidden form field. The application obtains the authenticated user ID from server-side claims and includes it in every document query. A user receives `404 Not Found` when requesting a document they do not own, avoiding resource disclosure.

Other protections include:

- `[Authorize]` on the document controller
- Anti-forgery tokens for all state-changing forms
- Dedicated input view models to prevent over-posting
- Unique email and stronger password requirements

## Project structure

```text
project2/
├── Areas/Identity/    Identity UI pages
├── Controllers/       Document and home workflows
├── Data/              Entity Framework Core context
├── Migrations/        Database schema history
├── Models/            Persistent entities
├── ViewModels/        Validated form input models
├── Views/             Razor UI
└── wwwroot/           CSS, JavaScript, and client assets
```

## Run locally

Prerequisites: .NET 10 SDK and SQL Server LocalDB (or another SQL Server instance).

```bash
git clone <your-repository-url>
cd notespace-document-editor/project2
dotnet restore
dotnet run
```

The database is created and migrated automatically on startup. To use another SQL Server, override `ConnectionStrings__DefaultConnection` or edit `project2/appsettings.json`.

Open the HTTPS URL printed in the terminal, register an account, and create your first document. The health endpoint is available at `/health`.

## Portfolio talking points

- Preventing horizontal privilege escalation with ownership-scoped queries
- Using a dedicated view model to prevent mass assignment and over-posting
- Integrating Identity authentication with application data
- Building accessible empty, search, create, edit, and delete states
- Using async database access and server-side validation

## Future improvements

- Document autosave and revision history
- Tags, folders, and sharing permissions
- File export to PDF and DOCX
- Integration tests for authenticated document workflows
- Configurable TinyMCE API key through user secrets

## License

This project is available for educational and portfolio use.
