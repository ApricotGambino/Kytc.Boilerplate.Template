<a id="readme-top"></a>

# Solution Documentation

<details>
  <summary>Table of Contents</summary>
  //TODO: Add ToC
</details>

## Getting Started

This is the documenation for the whole solution, this document won't go into specifics for any
given project, as there should be a `README.md` for that project.  This document will instead go over
specifics for the solution as a whole, and things that the `.sln` file uses.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Domain Driven Design

//TODO: Explain DDD

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Information

//TODO: Any info?

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Project Files

### :ghost: Hidden Folders

You may notice there are a few folders after creating the solution that are 'hidden'.
These are just folders created by Visual Studio and other tools to do whatever they need to do, such as: [.vs](https://stackoverflow.com/questions/48897191/what-is-the-vs-folder-used-for-in-visual-studio-solutions) and [.git](https://stackoverflow.com/questions/29217859/what-is-the-git-folder) folders

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### :file_folder: `src` `test` Folders

You may have noticed the source code is hosted in a folder called src, and test at the root level. While there's no rule, there is something of a unspoken standard.
If you look around in the software world, you're going to see this as a pretty common structure, and tooling follows conventions, so deviating really isn't bad, but not helpful either.
[Here's a short gist from David Fowler detailing the standard.](https://gist.github.com/davidfowl/ed7564297c61fe9ab814)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### :file_folder: `Solution Items`

Solution items are things that help the solution, from building, to enforcement of rules, to package management. In addition, we're going to keep documentation here too. A 'Solution Item' is something that's either an artifact of the solution, or something that in someway enriches it through development, or documentation.

**Why do the solution items not exist in the folders I'd expect?**
You're going to find files that are physically stored at the root solution folder, are logically stored here in the 'Solution Items' folder.

This is entirely because some of those files (often .something) files need to be at a root level to function, but for organization, we will display them in a logical structure here in Solution Items.

- :scroll: `LICENSE.txt`:
    Currently this is just using the MIT license.
    [This stackexchange answer describes what and why.](https://webmasters.stackexchange.com/questions/86315/what-does-license-txt-belong-to-exactly)
- :scroll: `README.md`:
    This should be the entry point for a developer to get started with a project
- :file_folder: `source-control/`:  
    - :file_folder: `github/`:  Github related files, your solution may not use Github, in which case, these don't matter to you.
      - :scroll: `.gitattributes`: [Stackoverflow answer explaining what it really is.](https://stackoverflow.com/questions/73086622/is-a-gitattributes-file-really-necessary-for-git)\
        Official ['Documentation'](https://git-scm.com/docs/gitattributes)
      - :scroll: `.gitignore`: This is used to tell Git to ignore certain files (Like the NPM or build folder)
        [Useful documentation](https://docs.github.com/en/get-started/git-basics/ignoring-files) \
        Official [Documentation](https://git-scm.com/docs/gitignore)

- :file_folder: `visual-studio-configurations/`:  Here you're going to find those files that are used to configure Visual Studio.
  - :scroll: `.vsconfig`: This is a file that automatically suggests components and extensions. [Microsoft's 'documentation'](https://learn.microsoft.com/en-us/visualstudio/install/import-export-installation-configurations?view=vs-2022) \
    :warning: `Just incase your IDE doesn't prompt you to install these extensions, just go here to see what's suggested.` :warning:
  - :scroll: `.editorconfig`: This is a file that helps enforce code styles. using [this extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.CodeCleanupOnSave) which is in the .vsconfig file, C# code will be enforced on save.  
    [Microsoft's recommendations](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/code-style-rule-options)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### :briefcase: `Asset Library.csproj`

This is just a C# project that you can add files to that aren't meant to be used to build the solution such as Documentation.
Why is it a C# project?  Because Visual Studio still doesn't have a proper 'folder' that you can add to by wildcard...

- :file_folder: `Documentation/`
    - :file_folder: `ADRs/`: This is where you'll find all the ADRs for the solution.
        - :scroll: `ADR README.md`: This document explains what an ADR (Architecture Decision Records) is and how to use it.
        - :scroll: `ADR Template.md`: This is a template file to create an ADR.
    - :file_folder: `READMEs/`: This is where you'll find all the READMEs for the different projects in the solution.
        - :scroll: `DEV README.md`: This is the starting point for following the READMEs. The other READMEs are further described in this document.
- :file_folder: `Assets/`: This is where you'll find assets used in the project, this project doesn't build so consider it just a folder.
      :warning: `This isn't where things like favicons or logos for a web project would be, those are in the webproject` :warning:

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<p align="right">(<a href="#readme-top">back to top</a>)</p>
