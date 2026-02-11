namespace Sandbox.Concept_Demos.AsyncBreakfast;


#pragma warning disable
public static class BreakfastShared
{
    internal sealed class Breakfast
    {
        internal bool IsDone { get; set; }
        internal List<string> Messages { get; set; } = new List<string>();
        internal HashBrown? HashBrowns { get; set; }
        internal Coffee? Coffee { get; set; }
        internal Egg? Eggs { get; set; }
        internal Juice? Juice { get; set; }
        internal Toast? Toast { get; set; }
        internal Juice PourOJ()
        {
            Messages.Add("Pouring orange juice");
            return new Juice();
        }


        internal void ApplyJam(Toast toast) =>
            Messages.Add("Putting jam on the toast");


        internal void ApplyButter(Toast toast) =>
            Messages.Add("Putting butter on the toast");

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "<Pending>")]
        internal Toast ToastBread(int slices)
        {
            for (var slice = 0; slice < slices; slice++)
            {
                Messages.Add("Putting a slice of bread in the toaster");
            }
            Messages.Add("Start toasting...");
            Task.Delay(Toast.TimeItTakesToToastBread).Wait();
            Messages.Add("Remove toast from toaster");

            return new Toast();
        }

        internal async Task<Toast> ToastBreadAsync(int slices)
        {
            for (var slice = 0; slice < slices; slice++)
            {
                Messages.Add("Putting a slice of bread in the toaster");
            }
            Messages.Add("Start toasting...");
            await Task.Delay(Toast.TimeItTakesToToastBread);
            Messages.Add("Remove toast from toaster");
            return new Toast();
        }

        internal async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        internal async Task<Toast> MakeToastWithButterAndJamFinalAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);
            Messages.Add("toast is ready");
            return toast;
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "<Pending>")]
        internal HashBrown FryHashBrowns(int patties)
        {
            Messages.Add($"putting {patties} hash brown patties in the pan");
            Messages.Add("cooking first side of hash browns...");
            Task.Delay(HashBrown.TimeItTakesToCookSideOfPatty).Wait();
            for (var patty = 0; patty < patties; patty++)
            {
                Messages.Add("flipping a hash brown patty");
            }
            Messages.Add("cooking the second side of hash browns...");
            Task.Delay(HashBrown.TimeItTakesToCookSideOfPatty).Wait();
            Messages.Add("Put hash browns on plate");

            return new HashBrown();
        }

        internal async Task<HashBrown> FryHashBrownsAsync(int patties)
        {
            Messages.Add($"putting {patties} hash brown patties in the pan");
            Messages.Add("cooking first side of hash browns...");
            await Task.Delay(HashBrown.TimeItTakesToCookSideOfPatty);
            for (var patty = 0; patty < patties; patty++)
            {
                Messages.Add("flipping a hash brown patty");
            }
            Messages.Add("cooking the second side of hash browns...");
            await Task.Delay(HashBrown.TimeItTakesToCookSideOfPatty);
            Messages.Add("Put hash browns on plate");

            return new HashBrown();
        }

        internal async Task<HashBrown> FryHashBrownsFinalAsync(int patties)
        {
            Messages.Add($"putting {patties} hash brown patties in the pan");
            Messages.Add("cooking first side of hash browns...");
            await Task.Delay(HashBrown.TimeItTakesToCookSideOfPatty);
            for (var patty = 0; patty < patties; patty++)
            {
                Messages.Add("flipping a hash brown patty");
            }
            Messages.Add("cooking the second side of hash browns...");
            await Task.Delay(HashBrown.TimeItTakesToCookSideOfPatty);
            Messages.Add("Put hash browns on plate");
            Messages.Add("hash browns are ready");

            return new HashBrown();
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "<Pending>")]
        internal Egg FryEggs(int howMany)
        {
            Messages.Add("Warming the egg pan...");
            Task.Delay(Egg.TimeItTakesToHeatTheEggPan).Wait();
            Messages.Add($"cracking {howMany} eggs");
            Messages.Add("cooking the eggs ...");
            Task.Delay(Egg.TimeItTakesToCookTheEggs).Wait();
            Messages.Add("Put eggs on plate");

            return new Egg();
        }
        internal async Task<Egg> FryEggsAsync(int howMany)
        {
            Messages.Add("Warming the egg pan...");
            await Task.Delay(Egg.TimeItTakesToHeatTheEggPan);
            Messages.Add($"cracking {howMany} eggs");
            Messages.Add("cooking the eggs ...");
            await Task.Delay(Egg.TimeItTakesToCookTheEggs);
            Messages.Add("Put eggs on plate");

            return new Egg();
        }
        internal async Task<Egg> FryEggsFinalAsync(int howMany)
        {
            Messages.Add("Warming the egg pan...");
            await Task.Delay(Egg.TimeItTakesToHeatTheEggPan);
            Messages.Add($"cracking {howMany} eggs");
            Messages.Add("cooking the eggs ...");
            await Task.Delay(Egg.TimeItTakesToCookTheEggs);
            Messages.Add("Put eggs on plate");
            Messages.Add("eggs are ready");

            return new Egg();
        }

        internal Coffee PourCoffee()
        {
            Messages.Add("Pouring coffee");
            return new Coffee();
        }
    }


    internal sealed class HashBrown
    {
        public const int TimeItTakesToCookSideOfPatty = 200;
    }

    internal sealed class Coffee;
    internal sealed class Egg
    {
        public const int TimeItTakesToHeatTheEggPan = 100;
        public const int TimeItTakesToCookTheEggs = 600;
    }
    internal sealed class Juice;
    internal sealed class Toast
    {
        public const int TimeItTakesToToastBread = 300;
    }
}


