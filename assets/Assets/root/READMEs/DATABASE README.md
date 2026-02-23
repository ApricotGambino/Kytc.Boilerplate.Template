<a id="readme-top"></a>

# Database Documentation

<details>
  <summary>Table of Contents</summary>
  //TODO: Add ToC
</details>

## About

This project uses EF Core for database access.  To modify the schema, you'll need to use EF core tools to add migrations.  The project is developed to auto-migrate so the application will run it's own schema migrations against the database.

//TODO: Update this further

## Getting Started

To get started with modifying the schema, make sure you have the .net tools installed.

:warning:Change the version with your version of .NET :warning:

```sh 
dotnet tool install --global dotnet-ef
```

Once those are installed, you can add a new migration
//TODO: Explain adding migrations and whatnot.
//TODO: Add automigrate
from the developer powershell in Visual Studio:
Navigate to the `Infrastructure` project and add your migration:  

```sh
C:\YourProject> cd Infrastructure
C:\YourProject\Infrastructure> dotnet ef migrations add InitialCreate
```

dotnet ef database update
<p align="right">(<a href="#readme-top">back to top</a>)</p>

 actually this one
 dotnet ef --project src\Infrastructure --startup-project src\Api migrations add Initial -- --environment Local

 ACTUALLY:thinking:
 dotnet ef --verbose --project src/Infrastructure/Infrastructure.csproj --startup-project src/Api/Api.csproj migrations add Initial -- --environment Local
 then this:100:
 dotnet ef --verbose --project src/Infrastructure/Infrastructure.csproj --startup-project src/Api/Api.csproj database update -- --environment Local

## Auto Migration

//TODO: Explain
ACCCTLUALLY THIS:

dotnet ef --verbose --project src/Infrastructure/Infrastructure.csproj --startup-project src/Api/Api.csproj  migrations add Initial -- --environment Local --context ApplicationDbContext
dotnet ef --verbose --project src/Infrastructure/Infrastructure.csproj --startup-project src/Api/Api.csproj  database update -- --environment Local --context ApplicationDbContext

## Placement in Domain Driven Design

//TODO: Describe why this project exists where it does in the DDD
<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Information

//TODO: Add information here

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Project Files

//TODO: Add project file descriptions here.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
