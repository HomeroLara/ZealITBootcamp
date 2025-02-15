namespace ZealITMobile.Pages;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }

    private void StringOptimizationButton_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new StringOptimizationPage());
    }

    private void IDisposableButton_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new IDisposablePage());
    }
}