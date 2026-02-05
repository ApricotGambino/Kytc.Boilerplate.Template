namespace Sandbox.Concept_Demos.Async;

using static Sandbox.Concept_Demos.AsyncBreakfast.BreakfastShared;


//This comes from: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/
internal static class SynchronousBreakfast
{

    internal static Breakfast MakeBreakfast()
    {
        var breakfast = new Breakfast();

        breakfast.Coffee = breakfast.PourCoffee();
        breakfast.Messages.Add("coffee is ready");

        breakfast.Eggs = breakfast.FryEggs(2);
        breakfast.Messages.Add("eggs are ready");

        breakfast.HashBrowns = breakfast.FryHashBrowns(3);
        breakfast.Messages.Add("hash browns are ready");

        breakfast.Toast = breakfast.ToastBread(2);
        breakfast.ApplyButter(breakfast.Toast);
        breakfast.ApplyJam(breakfast.Toast);
        breakfast.Messages.Add("toast is ready");

        breakfast.Juice = breakfast.PourOJ();
        breakfast.Messages.Add("oj is ready");
        breakfast.Messages.Add("Breakfast is ready!");
        breakfast.IsDone = true;
        return breakfast;
    }
}
