
using Data.EntityFramework;
using Kernel.Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestShared.Fixtures;

namespace Kernel.Api.Unit.Tests.ConfigurationTests;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class DatabaseConfigurationTests : BaseTestFixture
{
    [Test]
    public async Task AddDbContextConfiguration_TestingDBServiceRegistered_CanGetDBService()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.TestingEnvironmentName;
        builder.AddAppSettings<TestAppSettings>();
        var appSettings = builder.GetAppSettings<TestAppSettings>();

        //Act
        builder.AddDbContextConfiguration<ApplicationDbContext>(appSettings);
        //NOTE: Because TestingDatabaseContext inherits the ApplicationDbContext, in order to
        //register TestingDatabaseContext service, we actually ALSO have to register ApplicationDbContext
        builder.AddDbContextConfiguration<TestingDatabaseContext>(appSettings);


        var applicationDbContext = builder.Services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();
        var testingDbContext = builder.Services.BuildServiceProvider().GetRequiredService<TestingDatabaseContext>();
        var testingDbContextConection = testingDbContext.Database.GetDbConnection();
        var applicationDbContextConection = applicationDbContext.Database.GetDbConnection();

        using (Assert.EnterMultipleScope())
        {
            //Act
            Assert.That(testingDbContextConection.Database, Is.EqualTo(TestingConstants.TestingDatabaseName));
            Assert.That(applicationDbContextConection.Database, Is.EqualTo(TestingConstants.TestingDatabaseName));

            //These assertions test that the contexts created really are the versions of the contexts we expect.
            //The testingdtabasecontext has an additional entity set (TestEntities) that no other context should have.
            Assert.That(testingDbContext.GetType().GetProperty("TestEntities"), Is.Not.Null);
            Assert.That(applicationDbContextConection.GetType().GetProperty("TestEntities"), Is.Null);
        }
    }



}
