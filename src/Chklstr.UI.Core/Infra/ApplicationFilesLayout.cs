namespace Chklstr.UI.Core.Infra;

public class ApplicationFilesLayout
{
    public static ApplicationFilesLayout Default { get; } = new ApplicationFilesLayout(".");
    
    public string BaseDir { get; }
    public string ConfigFolder => Path.Combine(BaseDir, "etc");
    
    public ApplicationFilesLayout(string baseDir)
    {
        BaseDir = Path.GetFullPath(baseDir);
    }
}