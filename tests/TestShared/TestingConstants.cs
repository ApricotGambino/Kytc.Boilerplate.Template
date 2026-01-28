namespace TestShared;


public static class TestingConstants
{
    public const string EnvironmentName = "UnitTest";
    public const string PerformanceUnitTestEnvironmentName = "PerformanceUnitTest";
    public const string IntentionallyBadEnvironmentName = "IntentionallyBadUnitTest";
    public const string AlternativeUnitTestEnvironmentName = "AlternativeUnitTest";
    public const string AppSettingKey = "ValueUsedOnlyForAUnitTest";
    public const string AppSettingValue = "Leave this value alone, I am just here for a unit test";
    public const string UnitTestDatabaseName = "Kytc.Boilerplate.Template.UnitTest";
    public const string PerformanceUnitTestDatabaseName = "Kytc.Boilerplate.Template.PerformanceUnitTest";



}

public static class TestingCategoryConstants
{
    public const string ApiStartupTests = "ApiStartupTests";
    public const string nUnitFrameworkTests = "nUnitFrameworkTests"; //These are weird tests, but are for testing the testing framework.
    public const string UniqueContextTests = "UniqueContextTests"; //Seeing this trait in the test explorer tells you each test is going to rebuild the context, this means it's going to be a slow test.
    public const string PerformanceTests = "PerformanceTests"; //Seeing this trait in the test explorer tells you each test is going to rebuild the context, and it's also used in performance testing, so it's going to be a VERY slow test.
}
