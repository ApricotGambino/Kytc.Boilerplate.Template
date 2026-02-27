
# Kytc.Boilerplate.Template Template Creation

//TODO: UPDATE THIS

## Getting Started

If you want to make changes to the boilerplate template, you're going to want to get started by building locally.

After making your changes there are a few ways to test your changes:

- `.\LocalTesting.ps1`
    - will handle creating a local version of the template, and creating a solution using it. This is the fastest and easiest way to test locally.
    - This script calls `.\CreateAndInstallNugetTemplate.ps1` then `.\CreateTemplateProjectForTesting.ps1`
- Using the `New Solution` dialogue
    - If you want to not create a new project using the powershell script, you can execute `.\CreateAndInstallNugetTemplate.ps1`

For more information on the template: `dotnet new kytcbp --help`

## Actually Read this

Templating is VERY hard to find documentation for.

:warning: If you're going to modify _THIS_ solution, the 'Kytc.Boilerplate.Template' solution, and if that modification adjusts the .sln file (adding Solution Items, adding projects, etc) it's going to overwrite the .sln. This .sln has been modified
to use if() directives based on the template symbols. So by doing that, Visual Studio is going to remove your modifications.  You need to re-add them, and the only way you can is to look at a previous version and manually insert them.  Welcome to .sln [This you? Go here.](#vs-has-modified-the-sln-file) :warning:

## How to modify the solution based on symbols.## How to modify the solution based on symbols

A symbol is something you've defined in template.json, which is basically a parameter for when you're creating a project using the template. This lets us allow a developer to do something like create a Boilerplate project with, or without the Api project.

### Modifying the .sln to include/exclude files

We can use the symbols defined above to have the solution include/exclude files, those are done by modifying the .sln to include
>//#if (SYMBOL)\
SomeSolutionFileLine\
//#endif

Which is how we can include/exclude projects based on the selection of options upon creation of a project through the template.

### Modifying the Directory.Packages.props

If we're not using a project, say, the API, then we don't need nuget packages related to that project. That we can modify
by doing something like this:
> \<!--#if (Api) -->\
\<PackageVersion Include="Microsoft.AspNetCore.OpenApi" Version="9.0.4" />\
\<!--#endif -->

### VS has modified the sln file?

By doing that, the custom directives have been removed because VS has auto generated the solution file., this means that the next time a template is used, the symbols won't work.

This means if the next person creates a project based on this template, and they don't want the Api, then the solution is still going to have project references to a missing Api project.
It's a small thing, but it's annoying, and the solution will be broken.

So to fix that we need to modify the .sln do something like:
>//#if (Api)\
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Kytc.Boilerplate.Template.Api", "src\Kytc.Boilerplate.Template.Api\Kytc.Boilerplate.Template.Api.csproj", "{433D02F8-B3E4-F77D-1B3C-DBAE7A2F6062}"\
EndProject\
//#endif

Yeah, it's annoying.

## Relevant Files

- ### :gear:CreateAndInstallNugetTemplate.ps1

    This is a file that you can run using ./CreateAndInstallNugetTemplate.ps1 to create a local version of the template. (Without needing to go through Nuget package manager)

- ### :gear:KytcBoilerplateTemplate.nuspec

.nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.    .nuspec files are just used for package metadata (I think...) I'd link to documentation if I had it myself, this has been trail by fire, and Microsoft is an unkind documentorian.

- ### :gear:nuget.exe

    This is literally just the nuget executable.  

- ### :gear:template.json

    template.json files are used to create the actual template, which is different than the metadata of the .nuspec [Documentation](https://github.com/dotnet/templating/wiki/Reference-for-template.json)
