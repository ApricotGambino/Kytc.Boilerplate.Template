<a id="readme-top"></a>

# Kytc.Boilerplate.Template

This is the template project.

ctrlf for //NOTE and //TODO

//TODO: Add DDD diagram.

//TODO: Handle appsettings....

thing: 
[a relative link](LICENSE.txt)

[a relative link](path%20with%20spaces/other_file.md)

https://transportation.ky.gov/Pages/New-Team-Kentucky.aspx

# Getting Started




# Glossary

This section has additional information about elements of the project that aren't documented anywhere else.


- ### Api
  - #### launchSettings.json
    This file really is just here to tell Visual Studio how to run your webproject. \
    ['Documentation'](https://learn.microsoft.com/en-us/visualstudio/containers/container-launch-settings?view=vs-2022) \
    [Actual useful information](https://www.tvaidyan.com/2023/03/16/a-guide-to-launchsettings-json-in-asp-net/)
  - #### appsettings.json
    This is the file that stores variables meant to be configured for the application per environment.  \
    :warning: `You should read ADR001 - Appsettings Changes.md found in the ADR documentation section to understand how this file should be used. ` :warning:
  - #### AppSettings.cs
    This is the class that represents the appsetting.json file.  It's cast as an object during application startup so we can easily reference them through intellisense along with compile time checking.
  - #### Configurations
    This folder contains classes that are used to handle the configurations used by .net for startup and services. Organized here to stop the `Program.cs` from getting bloated. 
    This README doesn't cover all the classes in this folder, but each class here should be documented and explained in the code through comments.












