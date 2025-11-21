using FloatingAnnotationTool.ViewModels;
using FloatingAnnotationTool.Models;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FloatingAnnotationTool.Views;

/// <summary>
/// MainToolbarWindow - Floating toolbar with draggable support
/// </summary>
public partial class MainToolbarWindow : Window
{
    private readonly MainToolbarViewModel _viewModel;
    private OverlayWindow? _overlayWindow;

    public MainToolbarWindow(MainToolbarViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;
        
        InitializeComponent();
        
        // Subscribe to ViewModel events
        _viewModel.UndoRequested += OnUndoRequested;
        _viewModel.ClearRequested += OnClearRequested;
        _viewModel.SettingsRequested += OnSettingsRequested;
        _viewModel.ExitRequested += OnExitRequested;
        
        // Update color brushes
        UpdateColorBrushes();
        
        // Initialize Overlay immediately
        InitializeOverlay();
        
        // Update tool button states to reflect default selection
        UpdateToolButtonStates();
        
        // Subscribe to property changes
        _viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(MainToolbarViewModel.PrimaryColor) ||
                e.PropertyName == nameof(MainToolbarViewModel.SecondaryColor))
            {
                UpdateColorBrushes();
            }
        };
    }

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void InitializeOverlay()
    {
        if (_overlayWindow == null)
        {
            _overlayWindow = new OverlayWindow(_viewModel);
            
            // Ensure overlay closes when toolbar closes
            _overlayWindow.Closed += (s, e) => _overlayWindow = null;
            
            // Subscribe to exit annotation mode event
            _overlayWindow.ExitAnnotationModeRequested += (s, e) => 
            {
                SetDrawingMode(false);
                _viewModel.CurrentTool = DrawingTool.None;
            };
            
            _overlayWindow.Show();
            
            // Default to View Mode (Cursor)
            SetDrawingMode(false);
        }
    }

    private void SetDrawingMode(bool isDrawing)
    {
        if (_overlayWindow == null) InitializeOverlay();

        _overlayWindow!.SetInputMode(isDrawing);

        // Always ensure Toolbar is owned by Overlay so it stays on top
        if (this.Owner != _overlayWindow)
        {
            this.Owner = _overlayWindow;
        }

        if (isDrawing)
        {
            // Edit Mode
            this.Activate();
            
            // Uncheck Cursor button
            CursorBtn.IsChecked = false;
        }
        else
        {
            // View Mode
            // Check Cursor button
            CursorBtn.IsChecked = true;
            
            // Uncheck drawing tools visually
            PenBtn.IsChecked = false;
            LineBtn.IsChecked = false;
            RectangleBtn.IsChecked = false;
            EraserPointBtn.IsChecked = false;
            EraserObjectBtn.IsChecked = false;
        }
    }

    private void CursorBtn_Click(object sender, RoutedEventArgs e)
    {
        SetDrawingMode(false);
        _viewModel.CurrentTool = DrawingTool.None;
    }

    private void PenBtn_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.SelectPenCommand.Execute(null);
        SetDrawingMode(true);
        UpdateToolSelection(PenBtn);
        _overlayWindow?.UpdateTool();
    }

    private void LineBtn_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.SelectLineCommand.Execute(null);
        SetDrawingMode(true);
        UpdateToolSelection(LineBtn);
        _overlayWindow?.UpdateTool();
    }

    private void RectangleBtn_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.SelectRectangleCommand.Execute(null);
        SetDrawingMode(true);
        UpdateToolSelection(RectangleBtn);
        _overlayWindow?.UpdateTool();
    }

    private void EraserPointBtn_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.CurrentTool = Models.DrawingTool.EraserPoint;
        SetDrawingMode(true);
        UpdateToolSelection(EraserPointBtn);
        _overlayWindow?.UpdateTool();
    }

    private void EraserObjectBtn_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.CurrentTool = Models.DrawingTool.EraserObject;
        SetDrawingMode(true);
        UpdateToolSelection(EraserObjectBtn);
        _overlayWindow?.UpdateTool();
    }

    private void UpdateToolSelection(System.Windows.Controls.Primitives.ToggleButton selectedButton)
    {
        PenBtn.IsChecked = false;
        LineBtn.IsChecked = false;
        RectangleBtn.IsChecked = false;
        EraserPointBtn.IsChecked = false;
        EraserObjectBtn.IsChecked = false;
        selectedButton.IsChecked = true;
    }

    private void UpdateToolButtonStates()
    {
        PenBtn.IsChecked = _viewModel.CurrentTool == Models.DrawingTool.Pen;
        LineBtn.IsChecked = _viewModel.CurrentTool == Models.DrawingTool.Line;
        RectangleBtn.IsChecked = _viewModel.CurrentTool == Models.DrawingTool.Rectangle;
        EraserPointBtn.IsChecked = _viewModel.CurrentTool == Models.DrawingTool.EraserPoint;
        EraserObjectBtn.IsChecked = _viewModel.CurrentTool == Models.DrawingTool.EraserObject;
    }

    private void PrimaryColorBtn_Click(object sender, RoutedEventArgs e)
    {
        var colorDialog = new System.Windows.Forms.ColorDialog
        {
            Color = System.Drawing.Color.FromArgb(
                _viewModel.PrimaryColor.A,
                _viewModel.PrimaryColor.R,
                _viewModel.PrimaryColor.G,
                _viewModel.PrimaryColor.B)
        };

        if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            _viewModel.PrimaryColor = System.Windows.Media.Color.FromArgb(
                colorDialog.Color.A,
                colorDialog.Color.R,
                colorDialog.Color.G,
                colorDialog.Color.B);
            _overlayWindow?.UpdateBrush();
        }
    }

    private void SecondaryColorBtn_Click(object sender, RoutedEventArgs e)
    {
        var colorDialog = new System.Windows.Forms.ColorDialog
        {
            Color = System.Drawing.Color.FromArgb(
                _viewModel.SecondaryColor.A,
                _viewModel.SecondaryColor.R,
                _viewModel.SecondaryColor.G,
                _viewModel.SecondaryColor.B)
        };

        if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            _viewModel.SecondaryColor = System.Windows.Media.Color.FromArgb(
                colorDialog.Color.A,
                colorDialog.Color.R,
                colorDialog.Color.G,
                colorDialog.Color.B);
            _overlayWindow?.UpdateBrush();
        }
    }

    private void GradientBtn_Click(object sender, RoutedEventArgs e)
    {
        // ViewModel is updated via Binding
        _overlayWindow?.UpdateBrush();
    }

    private void MagicPenBtn_Click(object sender, RoutedEventArgs e)
    {
        // ViewModel is updated via Binding
        _overlayWindow?.UpdateMagicPenMode();
    }

    private void UndoBtn_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.UndoCommand.Execute(null);
    }

    private void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        _overlayWindow?.PerformSave();
    }

    private void CopyBtn_Click(object sender, RoutedEventArgs e)
    {
        _overlayWindow?.PerformCopy();
    }

    private void ClearBtn_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.ClearCommand.Execute(null);
    }

    private void SettingsBtn_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.OpenSettingsCommand.Execute(null);
    }

    private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
    {
        // Minimize to system tray
        Hide();
    }

    private void ExitBtn_Click(object sender, RoutedEventArgs e)
    {
        // Exit the application
        ((App)System.Windows.Application.Current).ExitApplication();
    }

    private void UpdateColorBrushes()
    {
        PrimaryColorBrush.Color = _viewModel.PrimaryColor;
        SecondaryColorBrush.Color = _viewModel.SecondaryColor;
    }

    private void OnUndoRequested(object? sender, EventArgs e)
    {
        _overlayWindow?.PerformUndo();
    }

    private void OnClearRequested(object? sender, EventArgs e)
    {
        _overlayWindow?.PerformClear();
    }

    private void OnSettingsRequested(object? sender, EventArgs e)
    {
        var settingsWindow = new SettingsWindow(_viewModel);
        settingsWindow.Owner = this;
        settingsWindow.ShowDialog();
    }

    private void OnExitRequested(object? sender, EventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    protected override void OnClosed(EventArgs e)
    {
        _overlayWindow?.Close();
        base.OnClosed(e);
    }
}
