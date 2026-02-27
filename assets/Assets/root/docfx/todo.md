//TODO: Add these: [CSE Playbook docs](https://github.com/microsoft/code-with-engineering-playbook/tree/main/docs)

//TODO: Explain where Nuget packages are installed, an why they are in Domain only.

//TODO: Explain Directory.Packages.props

//TODO: DB Logging
//TODO: DB Logging Tests
//TODO: How to handle EF core migrations.  

//TODO: Explain nuget central package management

//TODO: Add Domain readme, and explain schema folder naming for Entities
//TODO: Explain configurations vs annotations

//TODO: Logging

//TODO: Add DDD diagram.

//TODO: Handle appsettings....

//TODO: Talk about code formatting

//TODO: Test BP Template

//TODO: How to update template?

//TODO: Explain the 'aaa' snippet and how to import it

//TODO: How to update my BP app without startin gnew?

//TODO: Explain extensions, and how sometimes VS doesn't prompt you to install them.
//TODO: Explain that you need enterprise for code coverage.

//TODO: Explain `<!--#if (templateSolutionOnly) -->`

//TODO: Explain the testing setup for infrastruction.
//The execution works like this:

```text
//TestingSetup 
<!--    => RunBeforeAnyTests
TestContext
    => SetupTestContext
TestCustomWebApplicationFactory
    TestCustomWebApplicationFactory()
    ConfigureWebHost()
program
    everything
BaseTestFixture
    =TestSetUp
LoggingTests
    =>AddLogEntry1-->
```

//TODO: Explain testing categoryes [TestingFramework]

//TODO: Explain that the code coverage results suck and just say 'MoveNext()' for all async methods...

//TODO: Central Package Management ui doesn't remove it from Directory.Packages.props, also, explain what these are. <https://ryanbuening.com/posts/central-package-management/<https://ryanbuening.com/posts/central-package-management/>>

//TODO: <https://transportation.ky.gov/Pages/New-Team-Kentucky.aspx>

//TODO: Explain extensions, like comments: <https://github.com/madskristensen/CommentsVS>

//TODO: Should write a test to scan all entities to ensure they both inherit from BaseEntity, and also don't contain DateTime

//Configure SCALAR
