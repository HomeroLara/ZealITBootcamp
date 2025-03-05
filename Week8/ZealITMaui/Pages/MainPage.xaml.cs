using ZealITMaui.Pages;

namespace ZealITMaui;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
	private async void BreakfastButton_OnClicked(object? sender, EventArgs e)
	{
		await Navigation.PushAsync(new BreakfastPage());
	}

	private async void CallBackHellButton_OnClicked(object? sender, EventArgs e)
	{
		await Navigation.PushAsync(new CallBackHellPage());
	}
}

