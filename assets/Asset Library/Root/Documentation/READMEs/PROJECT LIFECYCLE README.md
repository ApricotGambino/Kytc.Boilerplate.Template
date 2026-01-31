<a id="readme-top"></a>

# Project Lifecycle Documentation

<details>
  <summary>Table of Contents</summary>
  //TODO: Add ToC
</details>

## Getting Started
This document should be used as a starting point for the project team to review and modify for the needs 
of this project.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Source Control

//TODO: Github or TFS
<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Task Tracking

//TODO: Github or TFS
<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Environments
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

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Other Project Dependencies

//TODO: What other projects are relied on?
<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Deployments

//TODO: How are deployments done?
<p align="right">(<a href="#readme-top">back to top</a>)</p>




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