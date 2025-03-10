using ZealITMaui.Pages;

namespace ZealITMaui;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
	private void TaskWhenAllButton_OnClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new TaskWhenAllPage());
	}

	private void CallBackHellButton_OnClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new CallBackHellPage());
	}

	private void AsyncAwaitButton_OnClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new AsyncAwaitPage());
	}

	private void SynchronousButton_OnClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new SynchronousPage());
	}
}

