using ZealITMaui.Pages;

namespace ZealITMaui;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
	private async void TaskWhenAllButton_OnClicked(object? sender, EventArgs e)
	{
		await Navigation.PushAsync(new TaskWhenAllPage());
	}

	private async void CallBackHellButton_OnClicked(object? sender, EventArgs e)
	{
		await Navigation.PushAsync(new CallBackHellPage());
	}

	private async void AsyncAwaitButton_OnClicked(object? sender, EventArgs e)
	{
		try
		{
			await Navigation.PushAsync(new AsyncAwaitPage());
			
			// since async void methods do not return a Task,
			// exceptions cannot be awaited or caught and will 
			// most likely crash the app
			// DoSomething();
			//await DoSomething();
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception);
		}
	}

	private async void SynchronousButton_OnClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new SynchronousPage());
	}

	private async void DoSomething()
	{
		await Task.Delay(3000);
		throw new Exception("oops!");
	}
}

