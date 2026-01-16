namespace IntegrationTests.IntegrationTestConfigurations;

using Domain.Entities.Admin;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

//NOTE: This is intentionally in a different namespace than the rest of the IntegrationTests, and also doesn't inherit from BaseTestFixture
//so that the TestContext isn't used.  This suite of tests means to test the TestContext,
//so we shouldn't rely on using it to test itself.
//Also, this isn't a great place to look at testing best practices, since these tests test the testing framework, and
//has to do some gross things.

//Also, we SHOULD target to hit 100% code coverage on everything in the INtegrationTestConfigurations with this test suite, who watches the watchers and who tests the testers? 

[Category("TestingFrameworkTests")]
public class IntegrationTestConfiguration_TestContextTests
{
    [Test]
    [Order(1)] //Ensure this test runs first
    public async Task SetupTestContext_TestContextHasNoTestingDatabase_CreatesTestingDatabase()
    {
        //NOTE: We have to run this test first, since it specifically deletes the database not using the TestContext method, and
        //would otherwise fail depending on what other tests were ran before. 
        //Arrange
        IntegrationTestConfigurationTestUtilities.DeleteUnitTestDatabase();

        //Act
        await TestContext.SetupTestContext();
        var databaseIdFromSql = IntegrationTestConfigurationTestUtilities.GetUnitTestDatabaseId();
        await TestContext.TearDownTestContext();

        //Assert
        Assert.That(databaseIdFromSql, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetDbContext_TestingContextNotInitialized_ThrowsError()
    {
        //Arrange
        await TestContext.TearDownTestContext();

        //Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => TestContext.GetDbContext());
    }

    [Test]
    public async Task GetDbContext_TestContextGetsCorrectConnectionStringFromDefaultUnitTestAppSetting_ThrowsError()
    {
        //Arrange
        await TestContext.TearDownTestContext();
        await TestContext.SetupTestContext();

        //Act
        var dbContext = TestContext.GetDbContext();


        //Arrange & Act
        Assert.That(dbContext.Database.GetDbConnection().Database, Is.EqualTo("Kytc.Boilerplate.Template.UnitTest"));
    }

    [Test]
    public async Task TearDownTestContext_SetupContextThenTearDownAndAttempToAccessTheDbContext_ThrowsError()
    {
        //Arrange
        await TestContext.SetupTestContext();

        //Act
        await TestContext.TearDownTestContext();

        //Assert
        Assert.Throws<InvalidOperationException>(
            () => TestContext.GetDbContext());
    }


    [Test]
    public async Task AddAsync_AddLogRecordToUnitTestDatabase_RecordIsInsertedWithId()
    {
        //Arrange
        await TestContext.SetupTestContext();

        //Act
        var insertedRecord = await TestContext.AddAsync(new Log
        {
            Message = nameof(AddAsync_AddLogRecordToUnitTestDatabase_RecordIsInsertedWithId),
            Level = "UNITTEST",
            MessageTemplate = ""
        });
        await TestContext.TearDownTestContext();

        //Assert
        Assert.That(insertedRecord, Is.Not.Null);
        Assert.That(insertedRecord.Id, Is.GreaterThan(0));
    }

    [Test]
    public async Task FindAsync_AddLogRecordToUnitTestDatabaseThenFindInsertedRecord_FetchInsertedRecordExists()
    {
        //Arrange
        await TestContext.SetupTestContext();
        var insertedRecord = await TestContext.AddAsync(new Log
        {
            Message = nameof(FindAsync_AddLogRecordToUnitTestDatabaseThenFindInsertedRecord_FetchInsertedRecordExists),
            Level = "UNITTEST",
            MessageTemplate = ""
        });

        //Act
        var foundRecord = await TestContext.FindAsync<Log>(insertedRecord.Id);
        await TestContext.TearDownTestContext();

        //Assert
        Assert.That(foundRecord, Is.Not.Null);
    }

    [Test]
    public async Task CountAsync_AddLogRecordToUnitTestDatabaseThenFindInsertedRecord_CountOfLogIncreasedByOne()
    {
        //Arrange
        await TestContext.SetupTestContext();
        var countBeforeInsert = await TestContext.CountAsync<Log>();
        var insertedRecord = await TestContext.AddAsync(new Log
        {
            Message = nameof(CountAsync_AddLogRecordToUnitTestDatabaseThenFindInsertedRecord_CountOfLogIncreasedByOne),
            Level = "UNITTEST",
            MessageTemplate = ""
        });

        //Act
        var countAfterInsert = await TestContext.CountAsync<Log>();
        await TestContext.TearDownTestContext();

        //Assert
        Assert.That(countAfterInsert, Is.EqualTo(countBeforeInsert + 1));
    }

    [Test]
    public async Task SetupTestContext_TestContextHasTestingDatabase_DeletesOldTestDatabaseAndCreatesNewTestingDatabase()
    {
        //Arrange
        await TestContext.SetupTestContext();
        var recordInsertedIntoOldTestDatabase = await TestContext.AddAsync(new Log
        {
            Message = nameof(SetupTestContext_TestContextHasTestingDatabase_DeletesOldTestDatabaseAndCreatesNewTestingDatabase),
            Level = "UNITTEST",
            MessageTemplate = "This record was inserted into a database that should not be found after initialization."
        });

        //Act
        //Reinitialize to delete and then create the database. 
        await TestContext.SetupTestContext();
        var foundRecord = await TestContext.FindAsync<Log>(recordInsertedIntoOldTestDatabase.Id);
        await TestContext.TearDownTestContext();

        //Assert
        Assert.That(foundRecord, Is.Null);
    }

    [Test]
    public async Task SetupTestContext_TestContextTriesToUseADatabaseOtherThanTheTestingOne_ThrowsError()
    {
        //Arrange
        await TestContext.TearDownTestContext();

        //Arrange & Act
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            () => TestContext.SetupTestContext("IntentionallyBadUnitTest"));
    }

    [Test]
    public async Task GetScopeFactory_TestContextNotInitialized_ReturnsNull()
    {
        //Arrange
        await TestContext.TearDownTestContext();

        //Arrange & Act
        Assert.That(TestContext.GetScopeFactory(), Is.Null);
    }

    [Test]
    public async Task GetScopeFactory_TestContextInitialized_ReturnsFactory()
    {
        //Arrange
        await TestContext.TearDownTestContext();
        await TestContext.SetupTestContext();

        //Arrange & Act
        Assert.That(TestContext.GetScopeFactory(), Is.Not.Null);
    }
}


[Category("TestingFrameworkTests")]
public class IntegrationTestConfiguration_TestCustomWebApplicationFactoryTests()
{
    //NOTE: At this time I'm not sure how to get the builder from the WebApplicationFactory to test what it's doing. 
    [Test]
    public async Task TestCustomWebApplicationFactoryConstructor_ConstructorWithoutEnvironmentNameParameter_InstanceHasUnitTestAsEnvironmentName()
    {
        //Arrange & Act
        var customWebFactory = new TestCustomWebApplicationFactory();

        //Assert
        Assert.That(customWebFactory._environmentName, Is.EqualTo("UnitTest"));
    }

    [Test]
    public async Task TestCustomWebApplicationFactoryConstructor_ConstructorWithEnvironmentNameParameter_InstanceHasProvidedEnvironmentName()
    {
        //Arrange & Act
        var environmentName = "CustomEnvironmentName";
        var customWebFactory = new TestCustomWebApplicationFactory(environmentName: environmentName);

        //Assert
        Assert.That(customWebFactory._environmentName, Is.EqualTo(environmentName));
    }

    [Test]
    public async Task TestCustomWebApplicationFactory_Initialized_HasDefaultEnviornmentNameInConfiguration()
    {
        //Arrange & Act
        var customWebFactory = new TestCustomWebApplicationFactory();
        var configuration = customWebFactory.Services.GetService<IConfiguration>();

        //Assert
        Assert.That(configuration, Is.Not.Null);
        Assert.That(configuration.GetValue<string>("environment"), Is.EqualTo("UnitTest"));
    }

    [Test]
    public async Task TestCustomWebApplicationFactory_Initialized_HasDefaultConnectionStringInConfiguration()
    {
        //Arrange & Act
        var customWebFactory = new TestCustomWebApplicationFactory();
        var configuration = customWebFactory.Services.GetService<IConfiguration>();
        var defaultConnectionString = configuration!.GetSection("AppSettings:ConnectionStrings:DefaultConnection").Value;

        //Act
        Assert.That(defaultConnectionString, Is.EqualTo("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template.UnitTest;Trusted_Connection=True;MultipleActiveResultSets=true"));
    }
}


[Category("TestingFrameworkTests")]
public class IntegrationTestConfiguration_BaseTestFixture()
{
    //NOTE: At this time I'm not sure how to get the builder from the WebApplicationFactory to test what it's doing. 
    [Test]
    public async Task TestSetUp_Initialized_DoesNotThrowError()
    {
        //Arrange & Act
        var baseTestFixture = new BaseTestFixture();

        //Act & Assert
        Assert.DoesNotThrowAsync(baseTestFixture.TestSetUp);
    }
}


public static class IntegrationTestConfigurationTestUtilities
{
    public static string GetUnitTestDatabaseName()
    {
        //This is a hard-coded string because we really want to make sure that your unit tests are only ever
        //ran against an intentional unit test database.
        return "Kytc.Boilerplate.Template.UnitTest";
    }

    public static SqlConnection GetLocalDbSqlConnection()
    {
        return new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=SSPI;Trusted_Connection=yes;");
    }

    public static bool DeleteUnitTestDatabase()
    {
        using (var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=SSPI;Trusted_Connection=yes;"))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = string.Format("DROP DATABASE IF EXISTS [Kytc.Boilerplate.Template.UnitTest]");
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public static int GetUnitTestDatabaseId()
    {
        var unitTestDatabaseId = 0;

        using (var sqlConnection = GetLocalDbSqlConnection())
        {
            using (var sqlCmd = new SqlCommand(string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", GetUnitTestDatabaseName()), sqlConnection))
            {
                sqlConnection.Open();

                var resultObj = sqlCmd.ExecuteScalar();

                var databaseID = 0;

                if (resultObj != null)
                {
                    var _ = int.TryParse(resultObj.ToString(), out databaseID);
                }

                sqlConnection.Close();

                unitTestDatabaseId = databaseID;
            }
        }
        return unitTestDatabaseId;
    }

    //public static DateTime? GetUnitTestDatabaseCreatedDate()
    //{
    //    DateTime? unitTestDatabaseCreatedDate = 0;

    //    using (var sqlConnection = GetLocalDbSqlConnection())
    //    {
    //        using (SqlCommand sqlCmd = new SqlCommand(string.Format("SELECT create_date FROM sys.databases WHERE Name = '{0}'", GetUnitTestDatabaseName()), sqlConnection))
    //        {
    //            sqlConnection.Open();

    //            object resultObj = sqlCmd.ExecuteScalar();

    //            DateTime? createdDate = null;

    //            if (resultObj != null)
    //            {
    //                int.TryParse(resultObj.ToString(), out databaseID);
    //            }

    //            sqlConnection.Close();

    //            createdDate = databaseID;
    //        }
    //    }
    //    return unitTestDatabaseCreatedDate;
    //}
}


