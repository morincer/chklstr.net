namespace Chklstr.Infra.Export.HTML;

public class Layout
{
    public int ViewPortWidth { get; set; } = 900;
    public bool MultipleColumns { get; set; } = true;
    public int ColumnsCount { get; set; } = 2;
    public bool ShowDescription { get; set; } = true;
    public int FontSize { get; set; } = 14;
}