// See https://aka.ms/new-console-template for more information

using CallbackHell;
using System;
using System.ComponentModel;
using System.Threading;

Console.WriteLine("Breakfast will be ready soon!");

var egg = new Egg(TimeSpan.FromSeconds(5));
egg.CookAsync();


Console.WriteLine("Breakfast is ready!");
Console.ReadKey();