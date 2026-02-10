namespace TestShared;


public static class TestingConstants
{
    public const string TestingEnvironmentName = "Testing";
    public const string TestingDatabaseName = "Kytc.Boilerplate.Template.Testing";

    public const string BenchmarkTestsEnvironmentName = "BenchmarkTests";
    public const string BenchmarkTestsDatabaseName = "Kytc.Boilerplate.Template.BenchmarkTests";

    public const string ApplicationName = "Kytc.Boilerplate.Template.Testing";


    public const string IntentionallyBadEnvironmentName = "IntentionallyBadUnitTest";
    public const string AlternativeUnitTestEnvironmentName = "AlternativeUnitTest";


    public const string TestingAppSettingKey = "TestKey";
    public const string TestingAppSettingValue = "I am a testing key.";
}

public static class TestingCategoryConstants
{
    public const string ApiStartupTests = "ApiStartupTests";
    public const string nUnitFrameworkTests = "nUnitFrameworkTests"; //These are weird tests, but are for testing the testing framework.
    public const string UniqueContextTests = "UniqueContextTests"; //Seeing this trait in the test explorer tells you each test is going to rebuild the context, this means it's going to be a slow test.
    public const string BenchmarkTests = "BenchmarkTests"; //Seeing this trait in the test explorer tells you each test is going to rebuild the context, and it's also used in performance testing, so it's going to be a VERY slow test.
}
