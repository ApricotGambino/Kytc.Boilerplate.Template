namespace TestShared;


public static class TestingConstants
{
    public const string TestingEnvironmentName = "Testing";
    public const string TestingDatabaseName = "Kytc.Boilerplate.Template.Testing";

    public const string BenchmarkTestsEnvironmentName = "Benchmark";
    public const string BenchmarkTestsDatabaseName = "Kytc.Boilerplate.Template.BenchmarkTests";

    public const string ApplicationName = "Kytc.Boilerplate.Template.Testing";


    public const string AlternativeUnitTestEnvironmentName = "AlternativeUnitTest";


    public const string TestingAppSettingKey = "TestKey";
    public const string TestingAppSettingValue = "I am a testing key.";
}

public static class TestingCategoryConstants
{
    /// <summary>
    /// Seeing this trait in the test explorer tells you each test is going to rebuild the context, this means it's going to be a slow test.
    /// </summary>
    public const string ApiStartupTests = "ApiStartupTests";
    /// <summary>
    /// These are weird tests, but are for testing the testing framework.
    /// </summary>
    public const string NUnitFrameworkTests = "nUnitFrameworkTests";
    /// <summary>
    /// Seeing this trait in the test explorer tells you each test is going to rebuild the context, this means it's going to be a slow test.
    /// </summary>
    public const string UniqueContextTests = "UniqueContextTests";
    /// <summary>
    /// /// Seeing this trait in the test explorer tells you each test is going to rebuild the context, this means it's going to be a slow test.
    /// </summary>
    public const string BenchmarkTests = "BenchmarkTests";
}
