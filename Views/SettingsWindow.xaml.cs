using FloatingAnnotationTool.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace FloatingAnnotationTool.Views;

/// <summary>
/// SettingsWindow - Configuration dialog for application settings
/// </summary>
public partial class SettingsWindow : Window
{
    private readonly MainToolbarViewModel _toolbarViewModel;
    private readonly SettingsViewModel _viewModel;

    public SettingsWindow(MainToolbarViewModel toolbarViewModel)
    {
        _toolbarViewModel = toolbarViewModel;
        _viewModel = new SettingsViewModel();
        DataContext = _viewModel;

        InitializeComponent();

        // Load current settings
        _viewModel.LoadFromToolbarViewModel(_toolbarViewModel);

        // Bind to UI
        FadeDurationSlider.Value = _viewModel.MagicPenFadeDurationMs;
        PenWidthSlider.Value = _viewModel.PenWidth;
        EraserSizeSlider.Value = _viewModel.EraserSize;
        GradientEnabledCheckBox.IsChecked = _viewModel.GradientEnabled;

        UpdateColorBrushes();
    }

    private void UpdateColorBrushes()
    {
        if (PrimaryColorBtn.Template.FindName("PrimaryColorBrush", PrimaryColorBtn) is SolidColorBrush primaryBrush)
        {
            primaryBrush.Color = _viewModel.PrimaryColor;
        }

        if (SecondaryColorBtn.Template.FindName("SecondaryColorBrush", SecondaryColorBtn) is SolidColorBrush secondaryBrush)
        {
            secondaryBrush.Color = _viewModel.SecondaryColor;
        }
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
            UpdateColorBrushes();
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
            UpdateColorBrushes();
        }
    }

    private async void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        // Update ViewModel from UI
        _viewModel.MagicPenFadeDurationMs = (int)FadeDurationSlider.Value;
        _viewModel.PenWidth = PenWidthSlider.Value;
        _viewModel.EraserSize = EraserSizeSlider.Value;
        _viewModel.GradientEnabled = GradientEnabledCheckBox.IsChecked ?? false;

        // Apply to toolbar ViewModel
        _viewModel.ApplyToToolbarViewModel(_toolbarViewModel);

        // Save to file
        await _toolbarViewModel.SaveSettingsAsync();

        DialogResult = true;
        Close();
    }

    private void CancelBtn_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
