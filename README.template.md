//TODO: UPDATE
Welcome to your new project generated with the Clean Architecture template!

<br />
<div align="center">  
    <img src="Asset%20Library/Root/Assets/Images/logo.png" height="80" alt="Logo">
    <h3 align="center">Kytc.Boilerplate.Template</h3>
    <p align="center">
        This is the template project to generate all the resources needed to install the Boilerplate template to get started developing applications.
        <br />
        <br />
        <a href="//TODO: URL TO DEMO SITE">View Demo</a>
        &middot;
        <a href="//TODO: URL TO PROJECT">Report Bug</a>
        &middot;
        <a href="//TODO: URL TO PROJECT">Request Feature</a>

    </p>
</div>

## Getting Started

- Build the solution:
  ```sh
  dotnet build
  ```
- Run the application:
  ```sh
  dotnet run --project src/Your.ProjectName.Web
  ```
- Update the database (if using EF Core):
  ```sh
  dotnet ef database update -c AppDbContext -p src/Your.ProjectName.Infrastructure/Your.ProjectName.Infrastructure.csproj -s src/Your.ProjectName.Web/Your.ProjectName.Web.csproj
  ```

## Solution Structure

- **Core**: Domain entities, value objects, interfaces
- **UseCases**: Application logic, CQRS handlers
- **Infrastructure**: Data access, external services
- **Web**: API endpoints (FastEndpoints)

For more details, see the documentation or visit the [Clean Architecture Template repository](https://github.com/ardalis/CleanArchitecture).
