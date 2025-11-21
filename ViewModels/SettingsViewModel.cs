using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DWAnnotation.Models;
using System.Windows.Media;

namespace DWAnnotation.ViewModels;

/// <summary>
/// ViewModel for the settings window
/// </summary>
public sealed partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private int _magicPenFadeDurationMs = 800;

    [ObservableProperty]
    private System.Windows.Media.Color _primaryColor = System.Windows.Media.Color.FromRgb(0, 120, 212);

    [ObservableProperty]
    private System.Windows.Media.Color _secondaryColor = System.Windows.Media.Color.FromRgb(255, 107, 107);

    [ObservableProperty]
    private double _penWidth = 3.0;

    [ObservableProperty]
    private double _eraserSize = 20.0;

    [ObservableProperty]
    private bool _gradientEnabled;

    [ObservableProperty]
    private bool _includeToolbarInScreenshot;

    public void LoadFromToolbarViewModel(MainToolbarViewModel toolbarViewModel)
    {
        MagicPenFadeDurationMs = toolbarViewModel.MagicPenFadeDurationMs;
        PrimaryColor = toolbarViewModel.PrimaryColor;
        SecondaryColor = toolbarViewModel.SecondaryColor;
        PenWidth = toolbarViewModel.PenWidth;
        EraserSize = toolbarViewModel.EraserSize;
        GradientEnabled = toolbarViewModel.GradientEnabled;
        IncludeToolbarInScreenshot = toolbarViewModel.IncludeToolbarInScreenshot;
    }

    public void ApplyToToolbarViewModel(MainToolbarViewModel toolbarViewModel)
    {
        toolbarViewModel.MagicPenFadeDurationMs = MagicPenFadeDurationMs;
        toolbarViewModel.PrimaryColor = PrimaryColor;
        toolbarViewModel.SecondaryColor = SecondaryColor;
        toolbarViewModel.PenWidth = PenWidth;
        toolbarViewModel.EraserSize = EraserSize;
        toolbarViewModel.GradientEnabled = GradientEnabled;
        toolbarViewModel.IncludeToolbarInScreenshot = IncludeToolbarInScreenshot;
    }
}
