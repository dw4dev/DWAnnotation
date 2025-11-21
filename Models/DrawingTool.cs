namespace DWAnnotation.Models;

/// <summary>
/// Drawing tool types
/// </summary>
public enum DrawingTool
{
    None,
    Pen,
    Line,
    Rectangle,
    EraserPoint,  // 軌跡擦除 (任意擦)
    EraserObject  // 物件擦除 (整塊擦)
}
