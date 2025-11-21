using System.Text.Json.Serialization;

namespace FloatingAnnotationTool.Models;

/// <summary>
/// Application settings model with JSON source generation support
/// </summary>
public sealed class AppSettings
{
    [JsonPropertyName("magicPenFadeDurationMs")]
    public int MagicPenFadeDurationMs { get; set; } = 800;

    [JsonPropertyName("primaryColor")]
    public string PrimaryColor { get; set; } = "#FF0078D4";

    [JsonPropertyName("secondaryColor")]
    public string SecondaryColor { get; set; } = "#FFFF6B6B";

    [JsonPropertyName("penWidth")]
    public double PenWidth { get; set; } = 3.0;

    [JsonPropertyName("eraserSize")]
    public double EraserSize { get; set; } = 20.0;

    [JsonPropertyName("gradientEnabled")]
    public bool GradientEnabled { get; set; } = false;

    [JsonPropertyName("magicPenEnabled")]
    public bool MagicPenEnabled { get; set; } = false;

    public static AppSettings CreateDefault() => new()
    {
        MagicPenFadeDurationMs = 800,
        PrimaryColor = "#FF0078D4",
        SecondaryColor = "#FFFF6B6B",
        PenWidth = 3.0,
        EraserSize = 20.0,
        GradientEnabled = false,
        MagicPenEnabled = false
    };
}

/// <summary>
/// JSON source generator context for AppSettings
/// </summary>
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(AppSettings))]
internal partial class AppSettingsJsonContext : JsonSerializerContext
{
}
