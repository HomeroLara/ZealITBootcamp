using CallbackHell;

Console.WriteLine("Breakfast will be ready soon!");

var eggs = new Eggs(TimeSpan.FromSeconds(4));
var toast = new Toast(TimeSpan.FromSeconds(3));
var bacon = new Bacon(TimeSpan.FromSeconds(2));
var coffee = new Coffee(TimeSpan.FromSeconds(2));

await eggs.CookAsync();
await toast.CookAsync();
await bacon.CookAsync();
await coffee.CookAsync();

Console.WriteLine($"Current Thread Id {Thread.CurrentThread.ManagedThreadId}...");

await Task.WhenAll(eggs.CookAsync(), toast.CookAsync(), bacon.CookAsync(), coffee.CookAsync());
