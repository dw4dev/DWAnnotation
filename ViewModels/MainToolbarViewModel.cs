using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FloatingAnnotationTool.Models;
using FloatingAnnotationTool.Services;
using System.Windows.Media;

namespace FloatingAnnotationTool.ViewModels;

/// <summary>
/// ViewModel for the main toolbar window
/// </summary>
public sealed partial class MainToolbarViewModel : ObservableObject
{
    private readonly SettingsService _settingsService;

    [ObservableProperty]
    private DrawingTool _currentTool = DrawingTool.None;

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
    private bool _magicPenEnabled;

    [ObservableProperty]
    private bool _isAnnotationMode;

    [ObservableProperty]
    private int _magicPenFadeDurationMs = 800;

    public MainToolbarViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
        _ = LoadSettingsAsync();
    }

    private async Task LoadSettingsAsync()
    {
        var settings = await _settingsService.LoadSettingsAsync();
        
        PrimaryColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(settings.PrimaryColor);
        SecondaryColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(settings.SecondaryColor);
        PenWidth = settings.PenWidth;
        EraserSize = settings.EraserSize;
        GradientEnabled = settings.GradientEnabled;
        MagicPenFadeDurationMs = settings.MagicPenFadeDurationMs;
    }

    public async Task SaveSettingsAsync()
    {
        var settings = new AppSettings
        {
            PrimaryColor = PrimaryColor.ToString(),
            SecondaryColor = SecondaryColor.ToString(),
            PenWidth = PenWidth,
            EraserSize = EraserSize,
            GradientEnabled = GradientEnabled,
            MagicPenFadeDurationMs = MagicPenFadeDurationMs
        };

        await _settingsService.SaveSettingsAsync(settings);
    }

    [RelayCommand]
    private void ToggleAnnotationMode()
    {
        IsAnnotationMode = !IsAnnotationMode;
    }

    [RelayCommand]
    private void SelectPen()
    {
        CurrentTool = DrawingTool.Pen;
    }

    [RelayCommand]
    private void SelectLine()
    {
        CurrentTool = DrawingTool.Line;
    }

    [RelayCommand]
    private void SelectRectangle()
    {
        CurrentTool = DrawingTool.Rectangle;
    }

    [RelayCommand]
    private void SelectEraserPoint()
    {
        CurrentTool = DrawingTool.EraserPoint;
    }

    [RelayCommand]
    private void SelectEraserObject()
    {
        CurrentTool = DrawingTool.EraserObject;
    }

    [RelayCommand]
    private void ToggleGradient()
    {
        GradientEnabled = !GradientEnabled;
    }

    [RelayCommand]
    private void ToggleMagicPen()
    {
        MagicPenEnabled = !MagicPenEnabled;
    }

    [RelayCommand]
    private void Undo()
    {
        UndoRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Clear()
    {
        ClearRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void OpenSettings()
    {
        SettingsRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Exit()
    {
        ExitRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? UndoRequested;
    public event EventHandler? ClearRequested;
    public event EventHandler? SettingsRequested;
    public event EventHandler? ExitRequested;
}
