<a id="readme-top"></a>

# Kytc.Boilerplate.Template

This is the template project.

ctrlf for //NOTE and //TODO

//TODO: Add DDD diagram.

//TODO: Handle appsettings....


## Project structure
The following is a listing of the project structure with links to additional information. 
<details open>
  <summary>Toggle Collapse</summary>

  :briefcase: Kytc.Boilerplate.Template\
 ┣ :ghost:[Hidden Folders](#hidden-folders)\
 ┣ :file_folder:[src/test](#src-test-folders)\
 ┣ :question:[Documentation](#documentation-folder)\
 ┃ ┗ :file_folder: [ADRs](#adrs) \
 ┃ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;┣  :scroll: [_ADR README.md](#adr-readme.md) \
 ┃ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;┗ :scroll: [_ADR Template.md](#adr-template.md) \
 ┣ :file_folder:[Solution Items](#solution-items)\
 ┃ ┣ :scroll: [README.md](#readme.md) _(This file)_ \
 ┃ ┣ :scroll: [LICENSE.txt](#license.txt) \
 ┃ ┣ :file_folder:[visual-studio-configurations](#visual-studio-configurations) \
 ┃ ┃ ┣ :gear:[.vsconfig](#vsconfig) \
 ┃ ┃ ┗ :gear:[.editorconfig](#editorconfig) \
 ┃ ┗ :file_folder:[github](#github) \
 ┃ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;┣ :gear:[.gitattributes](#gitattributes) \
 ┃ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;┗ :gear:[.gitignore](#gitignore) \
 ┣ :package: [API](#api) \
 ┃ ┣ :cloud: [Connected Services](https://learn.microsoft.com/en-us/visualstudio/azure/overview-connected-services?view=vs-2022) \
 ┃ ┣ :gear: [Dependencies](https://stackoverflow.com/questions/46432307/visual-studio-2017-references-vs-dependencies) \
 ┃ ┣ :file_folder: [Properties](#launchsettings.json) \
 ┃ ┃ ┗ :gear:[launchSettings.json](#launchsettings.json) \
 ┃ ┣ :gear: [appsettings.json](#appsettings.json) \
 ┃ ┃ ┣ :gear: [appsettings.Dev.json](#appsettings.json) \
 ┃ ┃ ┣ :gear: [appsettings.Prod.json](#appsettings.json) \
 ┃ ┃ ┣ :gear: [appsettings.Stage.json](#appsettings.json) \
 ┃ ┃ ┗ :gear: [appsettings.Test.json](#appsettings.json) 

</details>

## Project Administration
Once you've created a project based off this template, you'll need to coordinate with a development lead to organize getting your project in source control
and configured for the different environments as part of the project lifecycle. 

### Environments
Not all projects are identical in structure, but generally adhere to the following structure and flow: 
- Local
    - This is where you're working on code actively, and will be on the developer's machine. You may be pointed to a local DB or even Dev or Test. 
- Dev
    - This is where your checked in code will first be built and ran on a server. 
    - Projects generally will be automatically built and released based on check-ins. This environment should never be considered stable as any developer on the project can deploy new releases. 
    - This environment tends to be 'Internal' meaning no external internet traffic can access it.
- Test
    - This is where a project is intended to be tested against, often called QA. 
    - Development leads will have access to build and release changes to this environment as changes could affect testing efforts, for this reason it is not configured to run [CI/CD](https://en.wikipedia.org/wiki/CI/CD). 
    - This environment tends to be 'Internal' meaning no external internet traffic can access it.
- Stage
    - This is where a project is intended to undergo user acceptance testing (UAT)
    - Development leads will have access to request a build and release to this environment, but those changes have to be approved by authorized users as these changes could affect other project teams.
    - This environment could be internal, or external depending on the needs of the application.
- Prod
    - This is where a project eventually is released to the customer for use. It's production code.
    - Development leads will have access to request a build and release to this environment, but those changes have to be approved by authorized users as these changes could affect other project teams along with the customer themselves.
    - This environment could be internal, or external depending on the needs of the application.







# Glossary

This section has additional information about elements of the project that aren't documented anywhere else.

## Hidden Folders

You may notice there are a few folders after creating the solution that are 'hidden'.
These are just folders created by Visual Studio and other tools to do whatever they need to do, such as: [.vs](https://stackoverflow.com/questions/48897191/what-is-the-vs-folder-used-for-in-visual-studio-solutions) and [.git](https://stackoverflow.com/questions/29217859/what-is-the-git-folder) folders

## src test Folders

You may have noticed the source code is hosted in a folder called src, and test at the root level. While there's no rule, there is something of a unspoken standard.
If you look around in the software world, you're going to see this as a pretty common structure, and tooling follows conventions, so deviating really isn't bad, but not helpful either.
[Here's a short gist from David Fowler detailing the standard.](https://gist.github.com/davidfowl/ed7564297c61fe9ab814)

## Documentation Folder
This is what's called a 'Shared Project', and it's Visual Studio's solution to something that should have always been here, but now it's here, it's implemented poorly!
In theory this should just be a project that isn't meant to be compiled, and just a way to store documentation that visible in the Solution Explorer. 
What's great is in order to make that work, you have to edit the project's .projectitems file to use a wildcard to catch all lower files, and every time you add something
to any of the folders outside of Visual Studio, you have to unload and reload the project.  

Anyway, this is where we should put documentation.

  - ### ADRs
    This is where you'll find all the ADRs for the solution.
    - #### ADR README.md
        This document explains what an ADR (Architecture Decision Records) is and how to use it. 
    - #### ADR Template.md
        This is a template file to create an ADR. 

## Solution Items

Solution items are things that help the solution, from building, to enforcement of rules, to package management. In addition, we're going to keep documentation here too. A 'Solution Item' is something that's either an artifact of the solution, or something that in someway enriches it through development, or documentation.

**Why do the solution items not exist in the folders I'd expect?**
Well, take this file for example! This is called 'README.md', but exists in the root folder. But wait! There's no 'Solution Items' folder? That's right! It's annoyed me for years, and it's going to annoy me for years to come, but get used to it!

You're going to find files that are physically stored at the root solution folder, are logically stored here in the 'Solution Items' folder.

This is entirely because some of those files (often .something) files need to be at a root level to function, but for organization, we will display them in a logical structure here in Solution Items.

- ### Top level Files
  - #### LICENSE.txt
    Currently this is just using the MIT license.\
    [This stackexchange answer describes what and why.](https://webmasters.stackexchange.com/questions/86315/what-does-license-txt-belong-to-exactly)
  - #### README.md
    This should be the entry point for a developer to get started with a project, and is so for this one too.
- ### visual-studio-configurations
  Here you're going to find those files that are used to configure Visual Studio.
  - #### vsconfig
    This is a file that automatically suggests components and extensions. [Microsoft's 'documentation'](https://learn.microsoft.com/en-us/visualstudio/install/import-export-installation-configurations?view=vs-2022) \
    :warning: ` Just incase your IDE doesn't prompt you to install these extensions, just go here to see what's suggested.` :warning:
  - #### editorconfig
    This is a file that helps enforce code styles. using [this extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.CodeCleanupOnSave) which is in the .vsconfig file, C# code will be enforced on save.  
    [Microsoft's recommendations](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/code-style-rule-options)
- ### github
  Github related files, your solution may not use Github, in which case, these don't matter to you.
  - #### .gitattributes
    [Stackoverflow answer explaining what it really is.](https://stackoverflow.com/questions/73086622/is-a-gitattributes-file-really-necessary-for-git)\
    [Documentation](https://git-scm.com/docs/gitattributes)
  - #### .gitignore
    This is used to tell Git to ignore certain files (Like the NPM or build folder)
    [Useful documentation](https://docs.github.com/en/get-started/git-basics/ignoring-files) \
    [Documentation](https://git-scm.com/docs/gitignore)
- ### Api
  - #### launchSettings.json
    This file really is just here to tell Visual Studio how to run your webproject. \
    ['Documentation'](https://learn.microsoft.com/en-us/visualstudio/containers/container-launch-settings?view=vs-2022) \
    [Actual useful information](https://www.tvaidyan.com/2023/03/16/a-guide-to-launchsettings-json-in-asp-net/)
  - #### appsettings.json
    This is the file that stores variables meant to be configured for the application per environment.  \
    :warning: `You should read ADR001 - Appsettings Changes.md found in the ADR documentation section to understand how this file should be used. ` :warning:














 <!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->

  <a id="readme-top"></a>
  <!--
  *** Thanks for checking out the Best-README-Template. If you have a suggestion
  *** that would make this better, please fork the repo and create a pull request
  *** or simply open an issue with the tag "enhancement".
  *** Don't forget to give the project a star!
  *** Thanks again! Now go create something AMAZING! :D
  -->

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![project_license][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/github_username/repo_name">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">project_title</h3>

  <p align="center">
    project_description
    <br />
    <a href="https://github.com/github_username/repo_name"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/github_username/repo_name">View Demo</a>
    &middot;
    <a href="https://github.com/github_username/repo_name/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    &middot;
    <a href="https://github.com/github_username/repo_name/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->

## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

Here's a blank template to get started. To avoid retyping too much info, do a search and replace with your text editor for the following: `github_username`, `repo_name`, `twitter_handle`, `linkedin_username`, `email_client`, `email`, `project_title`, `project_description`, `project_license`

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

- [![Next][Next.js]][Next-url]
- [![React][React.js]][React-url]
- [![Vue][Vue.js]][Vue-url]
- [![Angular][Angular.io]][Angular-url]
- [![Svelte][Svelte.dev]][Svelte-url]
- [![Laravel][Laravel.com]][Laravel-url]
- [![Bootstrap][Bootstrap.com]][Bootstrap-url]
- [![JQuery][JQuery.com]][JQuery-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->

## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

### Prerequisites

This is an example of how to list things you need to use the software and how to install them.

- npm
  ```sh
  npm install npm@latest -g
  ```

### Installation

1. Get a free API Key at [https://example.com](https://example.com)
2. Clone the repo
   ```sh
   git clone https://github.com/github_username/repo_name.git
   ```
3. Install NPM packages
   ```sh
   npm install
   ```
4. Enter your API in `config.js`
   ```js
   const API_KEY = 'ENTER YOUR API';
   ```
5. Change git remote url to avoid accidental pushes to base project
   ```sh
   git remote set-url origin github_username/repo_name
   git remote -v # confirm the changes
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->

## Usage

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

_For more examples, please refer to the [Documentation](https://example.com)_

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->

## Roadmap

- [ ] Feature 1
- [ ] Feature 2
- [ ] Feature 3
  - [ ] Nested Feature

See the [open issues](https://github.com/github_username/repo_name/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Top contributors:

<a href="https://github.com/github_username/repo_name/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=github_username/repo_name" alt="contrib.rocks image" />
</a>

<!-- LICENSE -->

## License

Distributed under the project_license. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTACT -->

## Contact

Your Name - [@twitter_handle](https://twitter.com/twitter_handle) - email@email_client.com

Project Link: [https://github.com/github_username/repo_name](https://github.com/github_username/repo_name)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->

## Acknowledgments

- []()
- []()
- []()

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo_name.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo_name/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo_name.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo_name/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo_name.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo_name/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo_name/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/linkedin_username
[product-screenshot]: images/screenshot.png
[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[Laravel.com]: https://img.shields.io/badge/Laravel-FF2D20?style=for-the-badge&logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com
