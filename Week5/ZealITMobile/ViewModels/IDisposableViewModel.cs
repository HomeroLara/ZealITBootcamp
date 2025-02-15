using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZealITMobile.Models;

namespace ZealITMobile.ViewModels;

public partial class IDisposableViewModel: ObservableObject
{
    private LargeDataModel _largeDataModel;

    [RelayCommand]
    public void CreateLargeDataModel()
    {
        _largeDataModel = new LargeDataModel(10);
    }

    [RelayCommand]
    public void DisposeLargeDataModel()
    {
        _largeDataModel?.Dispose();
        _largeDataModel = null;
    }
}