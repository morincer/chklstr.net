namespace Chklstr.UI.Core.Utils;

public class SelectFileRequest
{
    public String? BaseFolder { get; set; }
    public String FileExtension { get; set; }
    public Action<string?> FileSelectedCallback { get; set; }
}