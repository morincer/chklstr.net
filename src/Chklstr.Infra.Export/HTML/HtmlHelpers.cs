using System.Reflection;
using Chklstr.Core.Model;

namespace Chklstr.Infra.Export.HTML;

public static class HtmlHelpers
{
    public static string ReadResource(string name)
    {
        // Determine path
        var assembly = Assembly.GetExecutingAssembly();
        
        var resourcePath = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(name));

        using (var stream = assembly.GetManifestResourceStream(resourcePath))
        using (var reader = new StreamReader(stream!))
        {
            return reader.ReadToEnd();
        }
    }

    public static string ContextsToClasses(ChecklistItem item, Dictionary<string, string> classesMap)
    {
        return string.Join(" ", item.Contexts.Select(classesMap.GetValueOrDefault).Where(c => c != null));
    }
}