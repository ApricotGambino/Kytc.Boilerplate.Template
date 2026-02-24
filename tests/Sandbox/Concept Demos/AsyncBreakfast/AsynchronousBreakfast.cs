using static Sandbox.Concept_Demos.AsyncBreakfast.BreakfastShared;

namespace Sandbox.Concept_Demos.Async;
//NOTE: This comes from: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/

public static class AsynchronousBreakfast
{
    internal static async Task<Breakfast> MakeBreakfastLevel1()
    {
        //From the article:
        //[MakeBreakfastLevelOne] doesn't yet take advantage of key features of asynchronous programming,
        //which can result in shorter completion times.
        //The code processes the tasks in roughly the same amount of time as the initial synchronous version.
        //For the full method implementations, see the final version of the code later in this article.

        //The thread doesn't block while the eggs or hash browns are cooking,
        //but the code also doesn't start other tasks until the current work completes.
        //You still put the bread in the toaster and stare at the toaster until the bread pops up,
        //but you can now respond to interruptions.
        //In a restaurant where multiple orders are placed,
        //the cook can start a new order while another is already cooking.

        //In [MakeBreakfastLevelOne], the thread working on the breakfast
        //isn't blocked while waiting for any started task that's unfinished.
        //For some applications, this change is all you need.
        //You can enable your app to support user interaction while data downloads from the web.
        //In other scenarios, you might want to start other tasks while waiting for the previous task to complete.


        var breakfast = new Breakfast();

        breakfast.Coffee = breakfast.PourCoffee(); //Always start cooking with a drink, here, coffee.
        breakfast.Messages.Add("coffee is ready");

        breakfast.Eggs = await breakfast.FryEggsAsync(2);
        breakfast.Messages.Add("eggs are ready");

        breakfast.HashBrowns = await breakfast.FryHashBrownsAsync(3);
        breakfast.Messages.Add("hash browns are ready");

        breakfast.Toast = await breakfast.ToastBreadAsync(2);
        breakfast.ApplyButter(breakfast.Toast);
        breakfast.ApplyJam(breakfast.Toast);
        breakfast.Messages.Add("toast is ready");

        breakfast.Juice = breakfast.PourOJ();
        breakfast.Messages.Add("oj is ready");
        breakfast.Messages.Add("Breakfast is ready!");
        breakfast.IsDone = true;
        return breakfast;
    }

    internal static async Task<Breakfast> MakeBreakfastLevel2()
    {
        //We've added code to split the async call from the await,
        //but this doesn't actually make things faster, this is here to showcase
        //how we came from Level 1, to Level 3.
        var breakfast = new Breakfast();

        breakfast.Coffee = breakfast.PourCoffee();//Always start cooking with a drink, here, coffee.
        breakfast.Messages.Add("coffee is ready");

        var eggsTask = breakfast.FryEggsAsync(2);
        breakfast.Eggs = await eggsTask;
        breakfast.Messages.Add("eggs are ready");

        var hashBrownTask = breakfast.FryHashBrownsAsync(3);
        breakfast.HashBrowns = await hashBrownTask;
        breakfast.Messages.Add("hash browns are ready");

        var toastTask = breakfast.ToastBreadAsync(2);
        breakfast.Toast = await toastTask;
        breakfast.ApplyButter(breakfast.Toast);
        breakfast.ApplyJam(breakfast.Toast);
        breakfast.Messages.Add("toast is ready");

        breakfast.Juice = breakfast.PourOJ();
        breakfast.Messages.Add("oj is ready");

        breakfast.Messages.Add("Breakfast is ready!");
        breakfast.IsDone = true;
        return breakfast;
    }

    internal static async Task<Breakfast> MakeBreakfastLevel3()
    {
        //NOW we're getting faster.
        //By moving the async calls to the start of the method, we can begin
        //those tasks 'now', and wait on them to finish later.

        //You can see this in action, consider the eggs in this method.
        //as is, it's going to output something like this:
        //Coffee is ready -> warming the egg pan... -> blah blah blah -> Put Eggs on plate -> eggs are ready
        //Which makes sense.

        //Comment out this line:
        ////  breakfast.Eggs = await eggsTask;
        //And you'll get:
        //Coffee is ready -> warming the egg pan... -> blah blah blah ->  eggs are ready -> cracking 2 eggs -> cooking the eggs -> [Put Eggs on Plate is missing]
        //It's out of order, because we're not telling our code to await eggs.
        //So we basically just sent that task off to do whatever it wanted, and never cared about when it finished.  In this case,
        //it finished after we were done, so we never even got the eggs plated message even though we said they were done.

        var breakfast = new Breakfast();

        breakfast.Coffee = breakfast.PourCoffee();//Always start cooking with a drink, here, coffee.
        breakfast.Messages.Add("coffee is ready");

        var eggsTask = breakfast.FryEggsAsync(2);
        var hashBrownTask = breakfast.FryHashBrownsAsync(3);
        var toastTask = breakfast.ToastBreadAsync(2);

        breakfast.Eggs = await eggsTask;
        breakfast.Messages.Add("eggs are ready");

        breakfast.HashBrowns = await hashBrownTask;
        breakfast.Messages.Add("hash browns are ready");

        breakfast.Toast = await toastTask;
        breakfast.ApplyButter(breakfast.Toast);
        breakfast.ApplyJam(breakfast.Toast);
        breakfast.Messages.Add("toast is ready");

        breakfast.Juice = breakfast.PourOJ();
        breakfast.Messages.Add("oj is ready");

        breakfast.Messages.Add("Breakfast is ready!");
        breakfast.IsDone = true;
        return breakfast;
    }

    internal static async Task<Breakfast> MakeBreakfastLevel4()
    {
        //At this level, we're not gaining any speed, but
        //we're tidying up a composition problem.
        //Before we had an async task to create toast, then two sync tasks
        //that applied Butter and Jam.
        //Our code looks like it's synchronous, but it isn't.
        //So we really should bundle up those jobs that rely on asynchronous code
        //that look synchronous into a new method that describes the reality
        //of the asynchronous operation.

        //The thing you'll notice different from 3 here is that
        //butter and jam are applied immediately after removing toast from the toaster.
        //whereas before, we put the jam and button on the toast after the hashbrowns
        //were ready because the applyButter/Jam methods were called after the await
        //for the hashbrowns.  This effectively means that unless the hashbrowns
        //were finished, we can't put butter and jam on the toast, which makes no sense
        //becuase for putting butter/jam on toast, we care about the toast being done, not the hashbrowns.

        var breakfast = new Breakfast();

        breakfast.Coffee = breakfast.PourCoffee();//Always start cooking with a drink, here, coffee.
        breakfast.Messages.Add("coffee is ready");

        var eggsTask = breakfast.FryEggsAsync(2);
        var hashBrownTask = breakfast.FryHashBrownsAsync(3);
        var toastTask = breakfast.MakeToastWithButterAndJamAsync(2);

        breakfast.Eggs = await eggsTask;
        breakfast.Messages.Add("eggs are ready");

        breakfast.HashBrowns = await hashBrownTask;
        breakfast.Messages.Add("hash browns are ready");

        breakfast.Toast = await toastTask;
        breakfast.Messages.Add("toast is ready");

        breakfast.Juice = breakfast.PourOJ();
        breakfast.Messages.Add("oj is ready");

        breakfast.Messages.Add("Breakfast is ready!");
        breakfast.IsDone = true;
        return breakfast;
    }

    internal static async Task<Breakfast> MakeBreakfastLevel5()
    {
        //So at this point, we're efficient, and correct.
        //This version is technically more correct in our intent from level 4,
        //because you'll notice in the previous version,
        //you might see this:
        //Putting jam on the toast
        //Put hash browns on plate
        //Put eggs on plate
        //eggs are ready
        //hash browns are ready
        //toast is ready

        //And if you read the code, you may think this is ok,
        //and it may be, but we're we're putting everything on the plate,
        //then announcing those things are ready.
        //But in reality, we would want this:
        //Putting butter on the toast
        //Putting jam on the toast
        //toast is ready
        //Put hash browns on plate
        //hash browns are ready
        //Put eggs on plate
        //eggs are ready

        //Here we can really see what happens matches our intent.
        //We intended to display that a thing is ready, as soon as it's done.
        //This is actually really an issue because of the lesson learned in lesson 4.
        //The composition of synchronous and asynchronous tasks,
        //where we have async methods, but then call synchronous message.AddMessage

        //This change makes that the case.
        //But in my opinion, this is borderline nonsensical and quite unreadable.
        //Let's wrap this up, and move onto level 6.
        var breakfast = new Breakfast();

        breakfast.Coffee = breakfast.PourCoffee();//Always start cooking with a drink, here, coffee.
        breakfast.Messages.Add("coffee is ready");

        var eggsTask = breakfast.FryEggsAsync(2);
        var hashBrownTask = breakfast.FryHashBrownsAsync(3);
        var toastTask = breakfast.MakeToastWithButterAndJamAsync(2);

        var breakfastTasks = new List<Task> { eggsTask, hashBrownTask, toastTask };

        while (breakfastTasks.Count > 0)
        {
            var finishedTask = await Task.WhenAny(breakfastTasks);
            if (finishedTask == eggsTask)
            {
                breakfast.Messages.Add("eggs are ready");
            }
            else if (finishedTask == hashBrownTask)
            {
                breakfast.Messages.Add("hash browns are ready");
            }
            else if (finishedTask == toastTask)
            {
                breakfast.Messages.Add("toast is ready");
            }
            await finishedTask;
            breakfastTasks.Remove(finishedTask);
        }

        breakfast.Juice = breakfast.PourOJ();
        breakfast.Messages.Add("oj is ready");

        breakfast.Messages.Add("Breakfast is ready!");
        breakfast.IsDone = true;
        return breakfast;
    }

    internal static async Task<Breakfast> MakeBreakfastLevel6()
    {
        //And here we are, we've really just taken all the messages indicating
        //that something is ready, and jammed them into the async methods.
        //This is really no different from Level 4 where we made a new
        //method to toast, butter, and jam all at the same time.

        //While trivial here, just moving those 'ready' messages into
        //the async methods it isn't that simple usually in real life.
        //It could require some serious refactoring.
        //That being said, getting to Level 6 here is contrived, but real.
        //Usually you'll probably just be going to level 1 and that's it, because
        //you're likely mostly waiting on EF to finish up something, and
        //there's almost never much you can do until that's done.


        var breakfast = new Breakfast();

        breakfast.Coffee = breakfast.PourCoffee();//Always start cooking with a drink, here, coffee.
        breakfast.Messages.Add("coffee is ready");

        var eggsTask = breakfast.FryEggsFinalAsync(2);
        var hashBrownTask = breakfast.FryHashBrownsFinalAsync(3);
        var toastTask = breakfast.MakeToastWithButterAndJamFinalAsync(2);

        breakfast.Eggs = await eggsTask;
        breakfast.HashBrowns = await hashBrownTask;
        breakfast.Toast = await toastTask;

        breakfast.Juice = breakfast.PourOJ();
        breakfast.Messages.Add("oj is ready");

        breakfast.Messages.Add("Breakfast is ready!");
        breakfast.IsDone = true;
        return breakfast;
    }
}
