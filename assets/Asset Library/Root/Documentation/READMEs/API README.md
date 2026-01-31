<a id="readme-top"></a>

# API Documentation

//TODO: Pointout the ADR001 Appsettings

<details>
  <summary>Table of Contents</summary>
  //TODO: Add ToC
</details>

## Getting Started
The API project is the project that is responsible for handling all the .net magic that creates 
the server-side part of our web project.  

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Placement in Domain Driven Design

//TODO: Describe why this project exists where it does in the DDD
<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Information
//TODO: Add information here

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Project Files
### :briefcase: `Api.csproj`
  - :scroll: `Properties/launchSettings.json`: This file really is just here to tell Visual Studio how to run your webproject. 
    - :warning: `You should read ADR001 - Appsettings Changes.md found in the ADR documentation section to understand how this file should be used. ` :warning:
    - ['Documentation'](https://learn.microsoft.com/en-us/visualstudio/containers/container-launch-settings?view=vs-2022) 
    - [Actual useful information](https://www.tvaidyan.com/2023/03/16/a-guide-to-launchsettings-json-in-asp-net/)        
  - :file_folder: `Configurations/`: This folder contains classes that are used to handle the configurations used by .net for startup and services. Organized here to stop the `Program.cs` from getting bloated. 
    This README doesn't cover all the classes in this folder, but each class here should be documented and explained in the code through comments.
  - :scroll: `AppSettings.cs`: This is the class that represents the appsetting.json file.  It's cast as an object during application startup so we can easily reference them through intellisense along with compile time checking.
  - :scroll: `appsettings.json`: This is the file that stores variables meant to be configured for the application per environment.
    - :warning: `You should read ADR001 - Appsettings Changes.md found in the ADR documentation section to understand how this file should be used. ` :warning:
  - :scroll: `Program.cs`: This is really the starting point for the API, which in reality means the starting point that gets ran by the web server.  
    - [Documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-9.0&tabs=windows)
  


<p align="right">(<a href="#readme-top">back to top</a>)</p>

