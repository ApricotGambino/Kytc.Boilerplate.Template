//namespace IntegrationTests.WebApplicationTests;

//using IntegrationTests.IntegrationTestConfigurations;


//public class WebApplicationBuilderConfigurationTests : BaseTestFixture
//{
//    //NOTE:TODO UPDATE THIS UPDATE
//    //There's not a lot of unit testing you can, nor should do for the Program.cs file.
//    //This file/class is considered a Composition Root (https://blog.ploeh.dk/2011/07/28/CompositionRoot/)
//    //Most of what this does is just call other methods (which should be tested),
//    //but we can test that our idea of the state of the application is as expected.
//    //Also, we're not using the testing context intentionally because we want to test the actual unmodified web application builder.



//    //[Test]
//    //public async Task helpme()
//    //{
//    //    WebApplicationBuilder test = new WebApplicationBuilder();
//    //    var before = await TestContext.CountAsync<Log>();

//    //    var client = TestContext.GetTestCustomWebApplicationFactory().CreateClient();

//    //    // Act
//    //    var response = await client.GetAsync("/appSettings");


//    //    await TestContext.AddAsync(new Log
//    //    {
//    //        Message = "Test Log Entry1",
//    //        Level = "good",
//    //        MessageTemplate = "goodgood"
//    //    });

//    //    var after = await TestContext.CountAsync<Log>();

//    //    Serilog.Log.Logger.Fatal("Added log entry??");

//    //    Assert.That(after, Is.EqualTo(before + 1));
//    //}
//}
