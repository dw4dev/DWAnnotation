using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FloatingAnnotationTool.Models;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;

namespace FloatingAnnotationTool.ViewModels;

/// <summary>
/// ViewModel for the overlay annotation window
/// </summary>
public sealed partial class OverlayViewModel : ObservableObject
{
    private readonly Stack<object> _undoStack = new();

    [ObservableProperty]
    private DrawingTool _currentTool = DrawingTool.None;

    [ObservableProperty]
    private System.Windows.Media.Brush _currentBrush = System.Windows.Media.Brushes.Blue;

    [ObservableProperty]
    private double _penWidth = 3.0;

    [ObservableProperty]
    private bool _magicPenEnabled;

    [ObservableProperty]
    private int _magicPenFadeDurationMs = 800;

    public void AddToUndoStack(object item)
    {
        _undoStack.Push(item);
    }

    public object? PopFromUndoStack()
    {
        return _undoStack.Count > 0 ? _undoStack.Pop() : null;
    }

    public void ClearUndoStack()
    {
        _undoStack.Clear();
    }

    public void UpdateBrush(System.Windows.Media.Color primaryColor, System.Windows.Media.Color secondaryColor, bool gradientEnabled)
    {
        if (gradientEnabled)
        {
            CurrentBrush = new System.Windows.Media.LinearGradientBrush(primaryColor, secondaryColor, 90.0);
        }
        else
        {
            CurrentBrush = new System.Windows.Media.SolidColorBrush(primaryColor);
        }
    }
}
