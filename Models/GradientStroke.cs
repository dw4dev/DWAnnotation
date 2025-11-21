using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace FloatingAnnotationTool.Models;

public class GradientStroke : Stroke
{
    public System.Windows.Media.Brush Brush { get; set; }

    public GradientStroke(StylusPointCollection stylusPoints, DrawingAttributes drawingAttributes, System.Windows.Media.Brush brush)
        : base(stylusPoints, drawingAttributes)
    {
        Brush = brush;
    }

    protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
    {
        if (Brush == null)
        {
            base.DrawCore(drawingContext, drawingAttributes);
            return;
        }

        // Create the geometry for the stroke
        var geometry = GetGeometry(drawingAttributes);
        
        // Draw with the gradient brush
        drawingContext.DrawGeometry(Brush, null, geometry);
    }
    
    // Clone method is important for InkCanvas operations
    public override Stroke Clone()
    {
        var clone = new GradientStroke(this.StylusPoints.Clone(), this.DrawingAttributes.Clone(), this.Brush.Clone());
        return clone;
    }
}
