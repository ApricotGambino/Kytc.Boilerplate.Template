namespace IntegrationTests.ApplicationStartTests;

using TestShared.Fixtures;

[Category(TestingCategoryConstants.ApiStartupTests)]
//NOTE: These tests focus on the application starting, and are testing the Program class, found in Program.cs
//There's not a lot of unit testing you can, nor should do for the Program.cs file.
//This file/class is considered a Composition Root (https://blog.ploeh.dk/2011/07/28/CompositionRoot/)
//Most of what this does is just call other methods (which should be tested),
//but we can test that our idea of the state of the application is as expected.
//It's going to look like we're actually testing TestCustomWebApplicationFactory, but really we're testing Program.cs here.
//The TestCustomWebApplicationFactory uses program.cs, and any functionality specifically relevant to TestCustomWebApplicationFactory's
//modifications to program.cs are tested in IntegrationTests.WebApplicationTests.TestCustomWebApplicationFactoryTests.cs
//Also, I think you could argue that these tests should be in the Functional Testing project, but I'd like for us to keep those tests very
//Customer Behaviour focused, and the customer doesn't really care about the app itself, only how it works.
//Also again, I think you could argue this should be in the TestShared project since ultimately Integration and Functional tests
//will rely on the function of of Program.cs, but the reason that feels wrong is because the tests in TestShared should really
//only test things that are unique to testing, which is why TestCustomWebApplicationFactory is tested there. 
public class ProgramStartTests : UniqueContextTestFixture
{
    [Test]
    public async Task StartupTest1()
    {
        Assert.Pass();
    }
}
//    //NOTE: At this time I'm not sure how to get the builder from the WebApplicationFactory to test what it's doing. 
//    [Test]
//    public async Task TestCustomWebApplicationFactoryConstructor_ConstructorWithoutEnvironmentNameParameter_InstanceHasUnitTestAsEnvironmentName()
//    {
//        //Arrange & Act
//        var customWebFactory = new TestCustomWebApplicationFactory();

//        //Assert
//        Assert.That(customWebFactory._environmentName, Is.EqualTo("UnitTest"));
//    }

//    [Test]
//    public async Task TestCustomWebApplicationFactoryConstructor_ConstructorWithEnvironmentNameParameter_InstanceHasProvidedEnvironmentName()
//    {
//        //Arrange & Act
//        var environmentName = "CustomEnvironmentName";
//        var customWebFactory = new TestCustomWebApplicationFactory(environmentName: environmentName);

//        //Assert
//        Assert.That(customWebFactory._environmentName, Is.EqualTo(environmentName));
//    }

//    [Test]
//    public async Task TestCustomWebApplicationFactory_Initialized_HasDefaultEnviornmentNameInConfiguration()
//    {
//        //Arrange & Act
//        var customWebFactory = new TestCustomWebApplicationFactory();
//        var configuration = customWebFactory.Services.GetService<IConfiguration>();

//        //Assert
//        Assert.That(configuration, Is.Not.Null);
//        Assert.That(configuration.GetValue<string>("environment"), Is.EqualTo("UnitTest"));
//    }

//    [Test]
//    public async Task TestCustomWebApplicationFactory_Initialized_HasDefaultConnectionStringInConfiguration()
//    {
//        //Arrange & Act
//        var customWebFactory = new TestCustomWebApplicationFactory();
//        var configuration = customWebFactory.Services.GetService<IConfiguration>();
//        var defaultConnectionString = configuration!.GetSection("AppSettings:ConnectionStrings:DefaultConnection").Value;

//        //Act
//        Assert.That(defaultConnectionString, Is.EqualTo("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template.UnitTest;Trusted_Connection=True;MultipleActiveResultSets=true"));
//    }
