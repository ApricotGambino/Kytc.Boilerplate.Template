namespace Kytc.Boilerplate.Template;

using Domain.Entities.Admin;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

//NOTE: This is intentionally in a different namespace than the rest of the IntegrationTests, and also doesn't inherit from BaseTestFixture
//so that the TestContext isn't used.  This suite of tests means to test the TestContext,
//so we shouldn't rely on using it to test itself.
//Also, this isn't a great place to look at testing best practices, since these tests test the testing framework, and
//has to do some gross things.

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
        await IntegrationTests.TestContext.SetupTestContext();
        var databaseIdFromSql = IntegrationTestConfigurationTestUtilities.GetUnitTestDatabaseId();
        await IntegrationTests.TestContext.TearDownTestContext();

        //Assert
        Assert.That(databaseIdFromSql, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetDbContext_TestingContextNotInitialized_ThrowsError()
    {
        //Arrange
        await IntegrationTests.TestContext.TearDownTestContext();

        //Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => IntegrationTests.TestContext.GetDbContext());
    }

    [Test]
    public async Task TearDownTestContext_SetupContextThenTearDownAndAttempToAccessTheDbContext_ThrowsError()
    {
        //Arrange
        await IntegrationTests.TestContext.SetupTestContext();

        //Act
        await IntegrationTests.TestContext.TearDownTestContext();

        //Assert
        Assert.Throws<InvalidOperationException>(
            () => IntegrationTests.TestContext.GetDbContext());
    }


    [Test]
    public async Task AddAsync_AddLogRecordToUnitTestDatabase_RecordIsInsertedWithId()
    {
        //Arrange
        await IntegrationTests.TestContext.SetupTestContext();

        //Act
        var insertedRecord = await IntegrationTests.TestContext.AddAsync(new Log
        {
            Message = nameof(AddAsync_AddLogRecordToUnitTestDatabase_RecordIsInsertedWithId),
            Level = "UNITTEST",
            MessageTemplate = ""
        });
        await IntegrationTests.TestContext.TearDownTestContext();

        //Assert
        Assert.That(insertedRecord, Is.Not.EqualTo(null));
        Assert.That(insertedRecord.Id, Is.GreaterThan(0));
    }

    [Test]
    public async Task FindAsync_AddLogRecordToUnitTestDatabaseThenFindInsertedRecord_FetchInsertedRecordExists()
    {
        //Arrange
        await IntegrationTests.TestContext.SetupTestContext();
        var insertedRecord = await IntegrationTests.TestContext.AddAsync(new Log
        {
            Message = nameof(FindAsync_AddLogRecordToUnitTestDatabaseThenFindInsertedRecord_FetchInsertedRecordExists),
            Level = "UNITTEST",
            MessageTemplate = ""
        });

        //Act
        var foundRecord = await IntegrationTests.TestContext.FindAsync<Log>(insertedRecord.Id);
        await IntegrationTests.TestContext.TearDownTestContext();

        //Assert
        Assert.That(foundRecord, Is.Not.EqualTo(null));
    }

    [Test]
    public async Task CountAsync_AddLogRecordToUnitTestDatabaseThenFindInsertedRecord_CountOfLogIncreasedByOne()
    {
        //Arrange
        await IntegrationTests.TestContext.SetupTestContext();
        var countBeforeInsert = await IntegrationTests.TestContext.CountAsync<Log>();
        var insertedRecord = await IntegrationTests.TestContext.AddAsync(new Log
        {
            Message = nameof(CountAsync_AddLogRecordToUnitTestDatabaseThenFindInsertedRecord_CountOfLogIncreasedByOne),
            Level = "UNITTEST",
            MessageTemplate = ""
        });

        //Act
        var countAfterInsert = await IntegrationTests.TestContext.CountAsync<Log>();
        await IntegrationTests.TestContext.TearDownTestContext();

        //Assert
        Assert.That(countAfterInsert, Is.EqualTo(countBeforeInsert + 1));
    }

    [Test]
    public async Task SetupTestContext_TestContextHasTestingDatabase_DeletesOldTestDatabaseAndCreatesNewTestingDatabase()
    {
        //Arrange
        await IntegrationTests.TestContext.SetupTestContext();
        var recordInsertedIntoOldTestDatabase = await IntegrationTests.TestContext.AddAsync(new Log
        {
            Message = nameof(SetupTestContext_TestContextHasTestingDatabase_DeletesOldTestDatabaseAndCreatesNewTestingDatabase),
            Level = "UNITTEST",
            MessageTemplate = "This record was inserted into a database that should not be found after initialization."
        });

        //Act
        //Reinitialize to delete and then create the database. 
        await IntegrationTests.TestContext.SetupTestContext();
        var foundRecord = await IntegrationTests.TestContext.FindAsync<Log>(recordInsertedIntoOldTestDatabase.Id);
        await IntegrationTests.TestContext.TearDownTestContext();

        //Assert
        Assert.That(foundRecord, Is.Null);
    }

    [Test]
    public async Task SetupTestContext_TestContextTriesToUseADatabaseOtherThanTheTestingOne_ThrowsError()
    {
        //Arrange
        await IntegrationTests.TestContext.TearDownTestContext();

        //Arrange & Act
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            () => IntegrationTests.TestContext.SetupTestContext("IntentionallyBadUnitTest"));
    }

    [Test]
    public async Task GetScopeFactory_TestContextNotInitialized_ReturnsNull()
    {
        //Arrange
        await IntegrationTests.TestContext.TearDownTestContext();

        //Arrange & Act
        Assert.That(IntegrationTests.TestContext.GetScopeFactory(), Is.Null);
    }

    [Test]
    public async Task GetScopeFactory_TestContextInitialized_ReturnsFactory()
    {
        //Arrange
        await IntegrationTests.TestContext.TearDownTestContext();
        await IntegrationTests.TestContext.SetupTestContext();

        //Arrange & Act
        Assert.That(IntegrationTests.TestContext.GetScopeFactory(), Is.Not.Null);
    }

    //TODO: Check that unittestalt works
    //TODO: test that checks all entiteis are there.

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
        int unitTestDatabaseId = 0;

        using (var sqlConnection = GetLocalDbSqlConnection())
        {
            using (SqlCommand sqlCmd = new SqlCommand(string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", GetUnitTestDatabaseName()), sqlConnection))
            {
                sqlConnection.Open();

                object resultObj = sqlCmd.ExecuteScalar();

                int databaseID = 0;

                if (resultObj != null)
                {
                    int.TryParse(resultObj.ToString(), out databaseID);
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


