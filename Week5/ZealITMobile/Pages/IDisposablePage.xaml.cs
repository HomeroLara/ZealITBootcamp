using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealITMobile.ViewModels;

namespace ZealITMobile.Pages;

public partial class IDisposablePage : ContentPage
{
    public IDisposablePage()
    {
        InitializeComponent();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is IDisposableViewModel viewModel)
        {
            // dispose of any resources when this view is no longer on top
            viewModel.DisposeLargeDataModelCommand.Execute(null);   
        }
    }
}