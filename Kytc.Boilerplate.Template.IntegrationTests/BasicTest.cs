namespace Kytc.Boilerplate.Template.IntegrationTests;

using Domain.Entities.Admin;
using NUnit.Framework;

public class CreateTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task AddLogEntry()
    {
        var before = await TestContext.CountAsync<Log>();


        await TestContext.AddAsync(new Log
        {
            Message = "Test Log Entry1",
            Level = "good",
            MessageTemplate = "goodgood"
        });

        var after = await TestContext.CountAsync<Log>();
        var a = 1;

        Assert.That(after, Is.EqualTo(before + 1));
    }

    [Test]
    public async Task AddLogEntry1()
    {
        var before = await TestContext.CountAsync<Log>();

        await TestContext.AddAsync(new Log
        {
            Message = "Test Log Entry2",
            Level = "bad",
            MessageTemplate = "badbad"
        });

        var after = await TestContext.CountAsync<Log>();
        var a = 1;

        Assert.That(after, Is.EqualTo(before + 1));
    }
}
