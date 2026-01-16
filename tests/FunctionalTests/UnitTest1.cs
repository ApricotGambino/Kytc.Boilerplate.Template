namespace FunctionalTests;

using System.Diagnostics.CodeAnalysis;
using Api.Configurations;
using Microsoft.AspNetCore.Builder;
using NUnit.Framework;

[ExcludeFromCodeCoverage]
[Category("WebApplicationBuilderConfigurationTests")]
public class UnitTest1 : BaseTestFixture
{

    [Test]
    public async Task AddAppSettingsJsonFile_MissingAppSettingFile_ThrowsException()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = "woiajsdflaksjdf";
        builder.AddAppSettingsJsonFile();
        var a = 1;
        //Replace with the name of the test method.
        //EX: GetUserRoles_UserHasNoRoles_ReturnsEmptyListOfRoles
        //EX: GetUserRoles_UserIsAdmin_ReturnsListThatContainsAdminRole
        //EX: GetUserRoles_UserDoesNotExist_ThrowsUserNotFoundException

        //Arrange
        //EX: userService.Add(new User { Name = "Test User", Roles = ["Admin"] });

        //Act
        //EX: var result = await userService.GetUserRoles("Test User");

        //Assert
        //EX: Assert.Contains("Admin", result);
    }

    [Test]
    public async Task test11()
    {
        //var before = await TestContext.CountAsync<Log>();


        var a = 1;
    }
}
