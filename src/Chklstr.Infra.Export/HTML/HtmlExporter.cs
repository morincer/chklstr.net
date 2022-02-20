using System.Reflection;
using System.Text.RegularExpressions;
using Chklstr.Core.Model;
using Chklstr.Core.Utils;
using Chklstr.Infra.Export.Templates.Html;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using RazorLight;

namespace Chklstr.Infra.Export.HTML;

public class HtmlExporter
{
    public async Task Export(QuickReferenceHandbook book, string outputPath, Layout layout, params string[] contexts)
    {
        var engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(HtmlTemplatesRoot))
            .SetOperatingAssembly(typeof(HtmlExportModel).Assembly)
            .UseMemoryCachingProvider()
            .EnableDebugMode()
            .Build();

        var viewModel = new HtmlExportModel
        {
            Book = book,
            Layout = layout,
            Filler =  new string(new char[layout.ViewPortWidth / 2]).Replace('\0', '.')
        };

        foreach (var c in book.GetAllAvailableContexts())
        {
            var className = "ctx_" + Regex.Replace(c, "[^a-zA-Z0-9_]", "_");
            viewModel.ContextClasses[c] = className;
        }
        
        viewModel.SelectedContexts.AddAll(contexts);


        var result = await engine.CompileRenderAsync("index", viewModel);
        await File.WriteAllTextAsync(outputPath, result);
    }
}