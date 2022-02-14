namespace Chklstr.UI.Core.Utils;

public class SelectFileRequest
{
    public string? BaseFolder { get; set; }
    
    public string FormatName { get; set; }
    public string FileExtension { get; set; }
    public Action<string?> FileSelectedCallback { get; set; }
}