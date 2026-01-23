namespace TestShared;


public static class TestingConstants
{
    public const string EnvironmentName = "UnitTest";
    public const string IntentionallyBadEnvironmentName = "IntentionallyBadUnitTest";
    public const string AppSettingKey = "ValueUsedOnlyForAUnitTest";
    public const string AppSettingValue = "Leave this value alone, I am just here for a unit test";
    public const string UnitTestDatabaseName = "Kytc.Boilerplate.Template.UnitTest";


}

public static class TestingCategoryConstants
{
    public const string ApiStartupTests = "ApiStartupTests";
    public const string nUnitFrameworkTests = "nUnitFrameworkTests"; //These are weird tests, but are for testing the testing framework.
    public const string UniqueContextTests = "UniqueContextTests"; //Seeing this trait in the test explorer tells you each test is going to rebuild the context, this means it's going to be a slow test.
}
