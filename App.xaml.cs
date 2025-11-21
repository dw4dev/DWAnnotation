using FloatingAnnotationTool.Services;
using FloatingAnnotationTool.ViewModels;
using FloatingAnnotationTool.Views;

namespace FloatingAnnotationTool;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private System.Windows.Forms.NotifyIcon? _notifyIcon;
    private MainToolbarWindow? _mainWindow;
    private readonly SettingsService _settingsService = new();

    private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
    {
        // Create system tray icon
        CreateNotifyIcon();

        // Create and show main toolbar window
        var viewModel = new MainToolbarViewModel(_settingsService);
        _mainWindow = new MainToolbarWindow(viewModel);
        _mainWindow.Closing += MainWindow_Closing;
        _mainWindow.Show();
    }

    private void CreateNotifyIcon()
    {
        var iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Icons", "AppIcon.ico");
        System.Drawing.Icon appIcon;

        try
        {
            if (System.IO.File.Exists(iconPath))
            {
                appIcon = new System.Drawing.Icon(iconPath);
            }
            else
            {
                appIcon = System.Drawing.SystemIcons.Application;
            }
        }
        catch
        {
            appIcon = System.Drawing.SystemIcons.Application;
        }

        _notifyIcon = new System.Windows.Forms.NotifyIcon
        {
            Icon = appIcon,
            Visible = true,
            Text = "浮動標註工具"
        };

        _notifyIcon.DoubleClick += (s, e) =>
        {
            ShowMainWindow();
        };

        var contextMenu = new System.Windows.Forms.ContextMenuStrip();
        
        var openItem = new System.Windows.Forms.ToolStripMenuItem("開啟工具列");
        openItem.Click += (s, e) => ShowMainWindow();
        contextMenu.Items.Add(openItem);

        var settingsItem = new System.Windows.Forms.ToolStripMenuItem("開啟設定");
        settingsItem.Click += (s, e) =>
        {
            ShowMainWindow();
            if (_mainWindow?.DataContext is MainToolbarViewModel vm)
            {
                vm.OpenSettingsCommand.Execute(null);
            }
        };
        contextMenu.Items.Add(settingsItem);

        contextMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());

        var exitItem = new System.Windows.Forms.ToolStripMenuItem("結束程式");
        exitItem.Click += (s, e) => Shutdown();
        contextMenu.Items.Add(exitItem);

        _notifyIcon.ContextMenuStrip = contextMenu;
    }

    private void ShowMainWindow()
    {
        if (_mainWindow != null)
        {
            _mainWindow.Show();
            _mainWindow.WindowState = System.Windows.WindowState.Normal;
            _mainWindow.Activate();
        }
    }

    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // Minimize to tray instead of closing
        if (_mainWindow != null)
        {
            e.Cancel = true;
            _mainWindow.Hide();
            _notifyIcon?.ShowBalloonTip(2000, "浮動標註工具", "程式已最小化到系統匣", System.Windows.Forms.ToolTipIcon.Info);
        }
    }

    private void Application_Exit(object sender, System.Windows.ExitEventArgs e)
    {
        _notifyIcon?.Dispose();
    }
}

