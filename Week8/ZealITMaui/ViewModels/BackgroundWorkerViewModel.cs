using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ZealITMaui.ViewModels;

public partial class BackgroundWorkerViewModel: ObservableObject
{
    [ObservableProperty]
    private int _progress;
    [ObservableProperty]
    private bool _isRunning;
    
    private BackgroundWorker _backgroundWorker;
    
    public BackgroundWorkerViewModel()
    {
        _backgroundWorker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        // Attach event handlers
        _backgroundWorker.DoWork += BackgroundWorker_DoWork;
        _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
        _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
    }

    [RelayCommand]
    private void StarBackgroundWorker()
    {
        if (_backgroundWorker.IsBusy) return;

        IsRunning = true;
        _backgroundWorker.RunWorkerAsync();
    }

    [RelayCommand]
    private void CancelBackgroundWorker()
    {
        if (_backgroundWorker.IsBusy)
        {
            _backgroundWorker.CancelAsync();
        }
    }

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        for (int i = 1; i <= 100; i++)
        {
            if (_backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            Thread.Sleep(100); // Simulate work
            _backgroundWorker.ReportProgress(i);
        }
    }

    private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        Progress = e.ProgressPercentage;
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        IsRunning = false;

        if (e.Cancelled)
        {
            Console.WriteLine("Task was cancelled.");
        }
        else
        {
            Console.WriteLine("Task completed successfully!");
        }
    }
}