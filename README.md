# CareerTrack

CareerTrack is an ASP.NET Core MVC web application for organizing job applications, companies, and interviews. It was developed for **CSCE 547 — Minor Project 4** using **.NET 10**, **Entity Framework Core**, and **SQLite**.

## Features

- Dashboard with application, company, interview, and offer counts, plus recent applications.
- Create, view, edit, and delete companies, job applications, and interviews.
- Search job applications by **Job Title** and **Location**, separately or together.
- Administration page to manage application statuses, with a safeguard against deleting statuses currently used by job applications.
- Settings page with instant **Light / Dark** theme switching and browser-based preference persistence.
- Five related database tables and sample data.

## Technologies

- C# / ASP.NET Core MVC / .NET 10
- Razor views, Bootstrap 5.3.3, CSS, JavaScript
- Entity Framework Core and SQLite
- Visual Studio 2026 (Windows)

## Solution structure

```text
CareerTrack/
├── CareerTrack/                 # MVC web application
│   ├── Controllers/
│   ├── Views/
│   ├── wwwroot/
│   └── Scripts/
│       ├── DDL.sql             # Database schema
│       └── DML.sql             # Sample records
├── CareerTrack.Data/           # Separate data-layer class library
│   ├── Models/
│   ├── Data/
│   └── Migrations/
├── CareerTrack.DbExport/       # Data export and SQL verification utility
└── CareerTrack.slnx
```

## Running the application

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Visual Studio 2026 with ASP.NET and web development tools, or a terminal

### 1. Clone and restore

```powershell
git clone <YOUR-REPOSITORY-URL>
cd CareerTrack
dotnet restore
```

Replace `<YOUR-REPOSITORY-URL>` with this repository's clone URL. The commands below assume you are in the folder containing `CareerTrack.slnx`.

### 2. Build

```powershell
dotnet build CareerTrack.slnx
```

### 3. Configure the database

The web application uses SQLite with the connection string in `CareerTrack/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=careertrack.db"
}
```

For a fresh checkout, the SQLite database file is not committed to Git. The repository includes EF Core migrations, `DDL.sql`, and `DML.sql` to reconstruct the schema and sample records.

**Recreate the database using the supplied SQL scripts:** Execute `CareerTrack/Scripts/DDL.sql` against a new SQLite database, followed by `CareerTrack/Scripts/DML.sql`. Use any SQLite client capable of running SQL scripts. Keep the database at the path resolved by the application's working directory (typically `CareerTrack/careertrack.db` when started from the web project folder). Do not run the DML against an already populated database, since it inserts explicit IDs.

**Alternative (EF Core migrations):** Install the .NET EF CLI if needed, then run:

```powershell
dotnet tool install --global dotnet-ef --version "10.0.*"
dotnet ef database update --project CareerTrack.Data --startup-project CareerTrack --context CareerTrackDbContext
```

The application's startup seeder may add sample records depending on the current database state; do not run the DML script after the application has already seeded those same records.

### 4. Run

```powershell
dotnet run --project CareerTrack
```

Open the local URL displayed in the terminal. You can also set the **CareerTrack** web project as the startup project in Visual Studio and run it.

## Database

The five application tables are `Companies`, `JobApplications`, `Interviews`, `Contacts`, and `ApplicationStatuses`. The included sample dataset contains **40 records** across these tables.

The SQL scripts were verified by reconstructing a separate test SQLite database and checking the record counts and foreign-key integrity.

## Notes

- This is a coursework/demo application. The Administration page does **not** implement authentication or role-based authorization.
- Local SQLite database files, build output, and Visual Studio user files are excluded by `.gitignore`.
- Deleting companies or job applications can cascade to related records. Review confirmation screens carefully.
- The theme preference is stored in the current browser's `localStorage`.

## Future improvements

Authentication, per-user data isolation, reminders, advanced filtering, accessibility refinements, UI improvements, and deployment support.

## Author

Divyashanu Swain
