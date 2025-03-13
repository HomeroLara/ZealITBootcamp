﻿using ZealITMaui.Pages;

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
		await Navigation.PushAsync(new AsyncAwaitPage());
	}

	private async void SynchronousButton_OnClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new SynchronousPage());
	}
}

