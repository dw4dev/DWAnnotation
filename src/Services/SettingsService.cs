using DWAnnotation.Models;
using System.IO;
using System.Text.Json;

namespace DWAnnotation.Services;

/// <summary>
/// Service for managing application settings persistence
/// </summary>
public sealed class SettingsService
{
    private readonly string _settingsPath;
    private AppSettings? _cachedSettings;

    public SettingsService()
    {
        // Store settings in the same directory as the executable
        var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _settingsPath = Path.Combine(appDirectory, "settings.json");
    }

    public async Task<AppSettings> LoadSettingsAsync()
    {
        if (_cachedSettings is not null)
            return _cachedSettings;

        if (!File.Exists(_settingsPath))
        {
            _cachedSettings = AppSettings.CreateDefault();
            await SaveSettingsAsync(_cachedSettings);
            return _cachedSettings;
        }

        try
        {
            await using var stream = File.OpenRead(_settingsPath);
            _cachedSettings = await JsonSerializer.DeserializeAsync(
                stream,
                AppSettingsJsonContext.Default.AppSettings
            ) ?? AppSettings.CreateDefault();
            return _cachedSettings;
        }
        catch
        {
            _cachedSettings = AppSettings.CreateDefault();
            return _cachedSettings;
        }
    }

    public async Task SaveSettingsAsync(AppSettings settings)
    {
        _cachedSettings = settings;
        await using var stream = File.Create(_settingsPath);
        await JsonSerializer.SerializeAsync(
            stream,
            settings,
            AppSettingsJsonContext.Default.AppSettings
        );
    }
}
