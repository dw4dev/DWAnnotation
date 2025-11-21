using FloatingAnnotationTool.Models;
using FloatingAnnotationTool.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FloatingAnnotationTool.Views;

/// <summary>
/// OverlayWindow - Full-screen transparent annotation overlay
/// </summary>
public partial class OverlayWindow : Window
{
    private readonly MainToolbarViewModel _toolbarViewModel;
    private readonly OverlayViewModel _viewModel;
    private System.Windows.Point _startPoint;
    private Stroke? _currentStroke;
    private bool _isDrawingShape;

    public OverlayWindow(MainToolbarViewModel toolbarViewModel)
    {
        _toolbarViewModel = toolbarViewModel;
        _viewModel = new OverlayViewModel();
        DataContext = _viewModel;

        InitializeComponent();

        // Multi-monitor support: Cover the entire virtual screen
        this.Left = SystemParameters.VirtualScreenLeft;
        this.Top = SystemParameters.VirtualScreenTop;
        this.Width = SystemParameters.VirtualScreenWidth;
        this.Height = SystemParameters.VirtualScreenHeight;

        // Register InkCanvas stroke collected event
        DrawingCanvas.StrokeCollected += DrawingCanvas_StrokeCollected;

        UpdateTool();
        UpdateBrush();
    }

    // Event to notify MainToolbarWindow to exit annotation mode
    public event EventHandler? ExitAnnotationModeRequested;

    private void ExitAnnotationMode()
    {
        ExitAnnotationModeRequested?.Invoke(this, EventArgs.Empty);
    }


    private void OverlayWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            // Exit annotation mode instead of closing window
            ExitAnnotationMode();
            e.Handled = true;
        }
        else if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            PerformUndo();
            e.Handled = true;
        }
        else if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            PerformUndo();
            e.Handled = true;
        }
        else if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            SaveToFile();
            e.Handled = true;
        }
        else if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            CopyToClipboard();
            e.Handled = true;
        }
        else if (e.Key == Key.G && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            _toolbarViewModel.ToggleGradientCommand.Execute(null);
            UpdateBrush();
            e.Handled = true;
        }
        else if (e.Key == Key.M && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            _toolbarViewModel.ToggleMagicPenCommand.Execute(null);
            UpdateMagicPenMode();
            e.Handled = true;
        }
    }

    public void UpdateTool()
    {
        _viewModel.CurrentTool = _toolbarViewModel.CurrentTool;
        _viewModel.PenWidth = _toolbarViewModel.PenWidth;

        // Default to custom cursor
        DrawingCanvas.UseCustomCursor = true;

        switch (_viewModel.CurrentTool)
        {
            case DrawingTool.Pen:
                DrawingCanvas.EditingMode = InkCanvasEditingMode.Ink;
                DrawingCanvas.IsHitTestVisible = true;
                
                this.Cursor = System.Windows.Input.Cursors.Pen;
                DrawingCanvas.Cursor = System.Windows.Input.Cursors.Pen;
                break;

            case DrawingTool.EraserPoint:
                DrawingCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                DrawingCanvas.IsHitTestVisible = true;
                
                // Use native InkCanvas cursor for Point Eraser to show size
                DrawingCanvas.UseCustomCursor = false;
                
                // Set eraser size
                DrawingCanvas.EraserShape = new EllipseStylusShape(
                    _toolbarViewModel.EraserSize, 
                    _toolbarViewModel.EraserSize);
                
                // We still set the Window cursor, but InkCanvas will override it when over the canvas
                this.Cursor = System.Windows.Input.Cursors.None; 
                break;

            case DrawingTool.EraserObject:
                DrawingCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                DrawingCanvas.IsHitTestVisible = true;
                
                // Load custom cursor from file
                try
                {
                    string cursorPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Cursors", "DelObj.cur");
                    
                    if (System.IO.File.Exists(cursorPath))
                    {
                        var customCursor = new System.Windows.Input.Cursor(cursorPath);
                        this.Cursor = customCursor;
                        DrawingCanvas.Cursor = customCursor;
                    }
                    else
                    {
                        this.Cursor = System.Windows.Input.Cursors.ScrollNW;
                        DrawingCanvas.Cursor = System.Windows.Input.Cursors.ScrollNW;
                    }
                }
                catch
                {
                    this.Cursor = System.Windows.Input.Cursors.ScrollNW;
                    DrawingCanvas.Cursor = System.Windows.Input.Cursors.ScrollNW;
                }
                break;

            case DrawingTool.Line:
            case DrawingTool.Rectangle:
                // We handle drawing manually for shapes
                DrawingCanvas.EditingMode = InkCanvasEditingMode.None;
                DrawingCanvas.IsHitTestVisible = true;
                
                this.Cursor = System.Windows.Input.Cursors.Cross;
                DrawingCanvas.Cursor = System.Windows.Input.Cursors.Cross;
                break;

            default:
                DrawingCanvas.EditingMode = InkCanvasEditingMode.None;
                DrawingCanvas.IsHitTestVisible = false;
                
                this.Cursor = System.Windows.Input.Cursors.Arrow;
                DrawingCanvas.Cursor = System.Windows.Input.Cursors.Arrow;
                break;
        }
    }

    public void UpdateBrush()
    {
        _viewModel.UpdateBrush(
            _toolbarViewModel.PrimaryColor,
            _toolbarViewModel.SecondaryColor,
            _toolbarViewModel.GradientEnabled
        );

        // Update InkCanvas drawing attributes
        var drawingAttributes = new DrawingAttributes
        {
            Width = _toolbarViewModel.PenWidth,
            Height = _toolbarViewModel.PenWidth,
            Color = _toolbarViewModel.PrimaryColor,
            StylusTip = StylusTip.Ellipse,
            IgnorePressure = false,
            FitToCurve = false // Better for geometric shapes
        };

        DrawingCanvas.DefaultDrawingAttributes = drawingAttributes;
        
        // Update Eraser Size if current tool is EraserPoint
        if (_viewModel.CurrentTool == DrawingTool.EraserPoint)
        {
            DrawingCanvas.EraserShape = new EllipseStylusShape(
                _toolbarViewModel.EraserSize, 
                _toolbarViewModel.EraserSize);
        }
    }

    public void UpdateMagicPenMode()
    {
        _viewModel.MagicPenEnabled = _toolbarViewModel.MagicPenEnabled;
        _viewModel.MagicPenFadeDurationMs = _toolbarViewModel.MagicPenFadeDurationMs;
    }

    private void DrawingCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
    {
        // Only handle Pen strokes here. Shapes are handled in MouseUp.
        if (_viewModel.CurrentTool == DrawingTool.Pen)
        {
            var stroke = e.Stroke;

            // If gradient is enabled, replace the stroke with a GradientStroke
            if (_toolbarViewModel.GradientEnabled)
            {
                var gradientBrush = new System.Windows.Media.LinearGradientBrush(
                    _toolbarViewModel.PrimaryColor,
                    _toolbarViewModel.SecondaryColor,
                    90.0);

                var gradientStroke = new GradientStroke(
                    stroke.StylusPoints,
                    stroke.DrawingAttributes,
                    gradientBrush
                );

                // Replace the original stroke
                DrawingCanvas.Strokes.Remove(stroke);
                DrawingCanvas.Strokes.Add(gradientStroke);
                stroke = gradientStroke; // Update reference for Undo/MagicPen
            }

            _viewModel.AddToUndoStack(stroke);

            if (_viewModel.MagicPenEnabled)
            {
                _ = FadeOutStrokeAsync(stroke);
            }
        }
    }

    private void DrawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Handle shape drawing initiation
        if (_viewModel.CurrentTool == DrawingTool.Line || 
            _viewModel.CurrentTool == DrawingTool.Rectangle)
        {
            _startPoint = e.GetPosition(DrawingCanvas);
            _isDrawingShape = true;
            
            // Capture mouse to ensure we get MouseUp even if outside window
            DrawingCanvas.CaptureMouse();

            // Create initial stroke (point)
            var points = new StylusPointCollection { new StylusPoint(_startPoint.X, _startPoint.Y) };
            var drawingAttributes = DrawingCanvas.DefaultDrawingAttributes.Clone();

            if (_toolbarViewModel.GradientEnabled)
            {
                var gradientBrush = new System.Windows.Media.LinearGradientBrush(
                    _toolbarViewModel.PrimaryColor,
                    _toolbarViewModel.SecondaryColor,
                    90.0);
                
                _currentStroke = new GradientStroke(points, drawingAttributes, gradientBrush);
            }
            else
            {
                _currentStroke = new Stroke(points, drawingAttributes);
            }
            
            DrawingCanvas.Strokes.Add(_currentStroke);
        }
    }

    private void DrawingCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (_isDrawingShape && _currentStroke != null)
        {
            var currentPoint = e.GetPosition(DrawingCanvas);
            StylusPointCollection newPoints;

            if (_viewModel.CurrentTool == DrawingTool.Line)
            {
                newPoints = GenerateLinePoints(_startPoint, currentPoint);
            }
            else // Rectangle
            {
                newPoints = GenerateRectanglePoints(_startPoint, currentPoint);
            }

            // Update stroke points
            _currentStroke.StylusPoints = newPoints;
        }
    }

    private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_isDrawingShape && _currentStroke != null)
        {
            _isDrawingShape = false;
            DrawingCanvas.ReleaseMouseCapture();

            // Finalize shape
            _viewModel.AddToUndoStack(_currentStroke);

            if (_viewModel.MagicPenEnabled)
            {
                _ = FadeOutStrokeAsync(_currentStroke);
            }

            _currentStroke = null;
        }
    }

    private StylusPointCollection GenerateLinePoints(System.Windows.Point start, System.Windows.Point end)
    {
        // Simple 2-point line is enough for Stroke to render a straight line
        return new StylusPointCollection
        {
            new StylusPoint(start.X, start.Y),
            new StylusPoint(end.X, end.Y)
        };
    }

    private StylusPointCollection GenerateRectanglePoints(System.Windows.Point start, System.Windows.Point end)
    {
        var points = new StylusPointCollection();
        
        var left = Math.Min(start.X, end.X);
        var top = Math.Min(start.Y, end.Y);
        var right = Math.Max(start.X, end.X);
        var bottom = Math.Max(start.Y, end.Y);

        // Top-Left -> Top-Right
        points.Add(new StylusPoint(left, top));
        points.Add(new StylusPoint(right, top));
        
        // Top-Right -> Bottom-Right
        points.Add(new StylusPoint(right, bottom));
        
        // Bottom-Right -> Bottom-Left
        points.Add(new StylusPoint(left, bottom));
        
        // Bottom-Left -> Top-Left (Close the loop)
        points.Add(new StylusPoint(left, top));

        return points;
    }

    private async Task FadeOutStrokeAsync(Stroke stroke)
    {
        var fadeDuration = _viewModel.MagicPenFadeDurationMs;
        var steps = 20;
        var stepDelay = fadeDuration / steps;

        for (int i = 0; i < steps; i++)
        {
            await Task.Delay(stepDelay);
            
            if (!DrawingCanvas.Strokes.Contains(stroke))
                return;

            var opacity = 1.0 - ((double)(i + 1) / steps);
            var newColor = System.Windows.Media.Color.FromArgb(
                (byte)(opacity * 255),
                stroke.DrawingAttributes.Color.R,
                stroke.DrawingAttributes.Color.G,
                stroke.DrawingAttributes.Color.B
            );
            
            // Must clone attributes to trigger update
            var newAttr = stroke.DrawingAttributes.Clone();
            newAttr.Color = newColor;
            stroke.DrawingAttributes = newAttr;
        }

        DrawingCanvas.Strokes.Remove(stroke);
    }

    public void PerformUndo()
    {
        var item = _viewModel.PopFromUndoStack();
        if (item is Stroke stroke)
        {
            DrawingCanvas.Strokes.Remove(stroke);
        }
        // No need to handle UIElement anymore since everything is a Stroke
    }

    public void PerformClear()
    {
        DrawingCanvas.Strokes.Clear();
        _viewModel.ClearUndoStack();
    }

    public void PerformSave()
    {
        SaveToFile();
    }

    public void PerformCopy()
    {
        CopyToClipboard();
    }

    // Win32 API for click-through support
    private const int WS_EX_TRANSPARENT = 0x00000020;
    private const int GWL_EXSTYLE = -20;

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hwnd, int index);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    public void SetInputMode(bool isEditable)
    {
        var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

        if (isEditable)
        {
            // Enable drawing
            // Remove WS_EX_TRANSPARENT to capture mouse events
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT);

            // Use almost transparent color to capture mouse events
            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(1, 0, 0, 0)); 
            this.IsHitTestVisible = true;
            
            // Ensure window can receive keyboard input
            this.Focusable = true;
            this.Focus();
            this.Activate();
            
            // Sync Magic Pen state when entering edit mode
            UpdateMagicPenMode();
            
            // Sync Brush settings (Gradient, Colors, Width)
            UpdateBrush();
        }
        else
        {
            // Disable drawing, allow click-through (View Mode)
            // Add WS_EX_TRANSPARENT to let mouse events pass through to desktop
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);

            this.Background = System.Windows.Media.Brushes.Transparent;
            this.IsHitTestVisible = false;
        }
    }

    private void SaveToFile()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "PNG Image|*.png",
            DefaultExt = ".png",
            FileName = $"Annotation_{DateTime.Now:yyyyMMdd_HHmmss}.png"
        };

        if (dialog.ShowDialog() == true)
        {
            SaveToPng(dialog.FileName);
        }
    }

    private void SaveToPng(string filePath)
    {
        try
        {
            var screenshot = CaptureScreenWithAnnotations();
            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(screenshot));

            using var stream = System.IO.File.Create(filePath);
            encoder.Save(stream);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"儲存失敗: {ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CopyToClipboard()
    {
        try
        {
            var screenshot = CaptureScreenWithAnnotations();
            System.Windows.Clipboard.SetImage(screenshot);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"複製失敗: {ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private System.Windows.Media.Imaging.BitmapSource CaptureScreenWithAnnotations()
    {
        int screenLeft = (int)SystemParameters.VirtualScreenLeft;
        int screenTop = (int)SystemParameters.VirtualScreenTop;
        int screenWidth = (int)SystemParameters.VirtualScreenWidth;
        int screenHeight = (int)SystemParameters.VirtualScreenHeight;

        // Capture desktop screenshot
        using var desktopBitmap = new System.Drawing.Bitmap(screenWidth, screenHeight);
        using (var g = System.Drawing.Graphics.FromImage(desktopBitmap))
        {
            g.CopyFromScreen(screenLeft, screenTop, 0, 0, desktopBitmap.Size);
        }

        // Convert System.Drawing.Bitmap to BitmapSource
        var desktopBitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            desktopBitmap.GetHbitmap(),
            IntPtr.Zero,
            Int32Rect.Empty,
            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

        // Create a DrawingVisual to combine desktop and annotations
        var drawingVisual = new DrawingVisual();
        using (var drawingContext = drawingVisual.RenderOpen())
        {
            // Draw desktop screenshot as background
            drawingContext.DrawImage(desktopBitmapSource, new Rect(0, 0, screenWidth, screenHeight));

            // Draw annotations on top
            var visualBrush = new VisualBrush(DrawingCanvas);
            drawingContext.DrawRectangle(visualBrush, null, new Rect(0, 0, screenWidth, screenHeight));
        }

        // Render to bitmap
        var renderTarget = new System.Windows.Media.Imaging.RenderTargetBitmap(
            screenWidth,
            screenHeight,
            96, 96,
            PixelFormats.Pbgra32);

        renderTarget.Render(drawingVisual);

        return renderTarget;
    }
}
