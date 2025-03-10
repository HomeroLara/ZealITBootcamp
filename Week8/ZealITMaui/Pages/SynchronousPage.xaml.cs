using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealITMaui.Pages;

public partial class SynchronousPage : ContentPage
{
    private Random _random = new Random();
    public SynchronousPage()
    {
        InitializeComponent();
    }

    private void ButtonTestUI_OnClicked(object? sender, EventArgs e)
    {
        // Generate a random color
        Color randomColor = GetRandomColor();

        // Set the button's background color
        ButtonTestUI.BackgroundColor = randomColor;
    }
    
    
    private Color GetRandomColor()
    {
        // Generate random RGB values
        byte red = (byte)_random.Next(256);
        byte green = (byte)_random.Next(256);
        byte blue = (byte)_random.Next(256);

        // Create and return a Color object
        return Color.FromRgb(red, green, blue);
    }
}