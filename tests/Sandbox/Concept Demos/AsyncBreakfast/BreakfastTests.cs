using Sandbox.Concept_Demos.Async;
using TestShared.Fixtures;

namespace Sandbox.Concept_Demos.AsyncBreakfast;

[Category(TestingCategoryConstants.SandboxTests)]
public class BreakfastTests : BaseTestFixture
{
    [Test]
    public async Task RunSynchronousBreakfast()
    {
        var breakfast = SynchronousBreakfast.MakeBreakfast();
        foreach (var message in breakfast.Messages)
        {
            Console.WriteLine(message);
        }

        Assert.That(breakfast.IsDone);
    }

    [Test]
    public async Task RunAsynchronousBreakfastLevel1()
    {
        var breakfast = await AsynchronousBreakfast.MakeBreakfastLevel1();
        foreach (var message in breakfast.Messages)
        {
            Console.WriteLine(message);
        }

        Assert.That(breakfast.IsDone);
    }

    [Test]
    public async Task RunAsynchronousBreakfastLevel2()
    {
        var breakfast = await AsynchronousBreakfast.MakeBreakfastLevel2();
        foreach (var message in breakfast.Messages)
        {
            Console.WriteLine(message);
        }

        Assert.That(breakfast.IsDone);
    }

    [Test]
    public async Task RunAsynchronousBreakfastLevel3()
    {
        var breakfast = await AsynchronousBreakfast.MakeBreakfastLevel3();
        foreach (var message in breakfast.Messages)
        {
            Console.WriteLine(message);
        }

        Assert.That(breakfast.IsDone);
    }

    [Test]
    public async Task RunAsynchronousBreakfastLevel4()
    {
        var breakfast = await AsynchronousBreakfast.MakeBreakfastLevel4();
        foreach (var message in breakfast.Messages)
        {
            Console.WriteLine(message);
        }

        Assert.That(breakfast.IsDone);
    }

    [Test]
    public async Task RunAsynchronousBreakfastLevel5()
    {
        var breakfast = await AsynchronousBreakfast.MakeBreakfastLevel5();
        foreach (var message in breakfast.Messages)
        {
            Console.WriteLine(message);
        }

        Assert.That(breakfast.IsDone);
    }

    [Test]
    public async Task RunAsynchronousBreakfastLevel6()
    {
        var breakfast = await AsynchronousBreakfast.MakeBreakfastLevel6();
        foreach (var message in breakfast.Messages)
        {
            Console.WriteLine(message);
        }

        Assert.That(breakfast.IsDone);
    }
}
