using System.Diagnostics;
using ZealITMobile.Models;
using ZealITMobile.ViewModels;

namespace ZealITMobile.Pages;

public partial class EventMemoryLeakPage : ContentPage
{
    Button _dynamicButton;
    LargeDataModel _largeData;
    public EventMemoryLeakPage()
    {
        InitializeComponent();
        AddNewButton();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is EventMemoryLeakViewModel eventMemoryLeakViewModel)
        {
            // dispose of any resources when this view is no longer on top
            eventMemoryLeakViewModel.Dispose();
        }
        
        _dynamicButton.Clicked -= DynamicButtonOnClicked;
    }

    private void AddNewButton()
    {
        _dynamicButton = new Button
        {
            Text = "Dynamically Generated Button",
            HorizontalOptions = LayoutOptions.Fill,
            Margin = new Thickness(0,20,0,0),
        };
        _dynamicButton.Clicked += DynamicButtonOnClicked;
        MainStackLayout.Children.Add(_dynamicButton);
    }

    private void DynamicButtonOnClicked(object? sender, EventArgs e)
    {
        // do some stuff
        Debug.WriteLine("Content Page Event Triggered");
        MessageLabel.Text = "Dynamically Generated Button's event triggered.";
        _largeData = new LargeDataModel(5);

        // this event needs to be unregistered when the user navigates away from this page.
        // not doing so will lead to memory leaks. this is because an unregistered event handler
        // can keep a reference to this page and it's resources ( viewmodel ) even if it's not being used.
        // an unregistered event handler can make the GC think it's a valid resource bc it sees a strong reference
    }

    /// <summary>
    /// overriding this page life cycle method so that we can un-register any
    /// custom event handlers
    /// </summary>
    /// <param name="args"></param>
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        if (BindingContext is EventMemoryLeakViewModel eventMemoryLeakViewModel)
        {
            // dispose of any resources when this view is no longer on top
            eventMemoryLeakViewModel.Dispose();
        }
        
        _dynamicButton.Clicked -= DynamicButtonOnClicked;
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        // do some stuff
        // we do not need to worry about this event handler needing to be unregsitered
        // this is because the framework will automatically handles unregistering eveng handlers
        // that were wired up via markup
    }
}