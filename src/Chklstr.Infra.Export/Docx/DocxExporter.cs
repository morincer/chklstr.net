using System.Diagnostics;
using System.Reflection;
using Chklstr.Core.Model;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Markdig;
using Markdig.Renderers.Docx;
using Microsoft.Extensions.Logging;

namespace Chklstr.Infra.Export;

public class DocxExporter
{
    private readonly ILogger<DocxExporter> _logger;
    private readonly ILogger<DocxDocumentRenderer> _markdownLogger;
    
    public static readonly String STYLE_ID_CHECKLIST_TABLE = "ChecklistTable";
    public static readonly String STYLE_ID_CHECK_VALUE = "CheckValue";
    public static readonly String STYLE_ID_CHECK_NAME = "CheckName";
    public static readonly String STYLE_ID_SECTION_HEADER = "SectionHeader";
    private readonly MarkdownPipeline _markdownPipeline;

    public DocxExporter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<DocxExporter>();
        _markdownLogger = loggerFactory.CreateLogger<DocxDocumentRenderer>();
        _markdownPipeline = new MarkdownPipelineBuilder().UseEmphasisExtras().Build();
    }

    public void Export(QuickReferenceHandbook book, string outputPath, Layout layout, params string[] contexts)
    {
        try
        {
            using var doc = WordprocessingDocument.CreateFromTemplate(@"./Templates/Docx/template.docx", false);
            DocxTemplateHelper.CleanContents(doc);

            if (doc.MainDocumentPart == null)
            {
                doc.AddMainDocumentPart();
            }

            doc.MainDocumentPart!.AddStyledParagraphOfText("Title", book.AircraftName);


            foreach (var cl in book.Checklists)
            {
                if (!cl.IsAvailableInContext(contexts)) continue;

                var table = MainDocumentExtensions.CreateTable(0, 2, STYLE_ID_CHECKLIST_TABLE);
                var titleRow = table.AddRow(true, cl.Name);
                titleRow.AddHorizontalSpan(2);

                doc.MainDocumentPart!.Document.Body!.AppendChild(table);
                doc.MainDocumentPart!.Document.Body!.AppendChild(MainDocumentExtensions.CreateParagraph(""));

                ExportItems(cl.Items, table, doc, layout, contexts);
            }

            doc.SaveAs(outputPath);
            doc.Close();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    private void ExportItems(IEnumerable<ChecklistItem> items, Table table, WordprocessingDocument doc, Layout layout,
        string[] contexts)
    {
        foreach (var item in items)
        {
            if (!item.IsAvailableInContext(contexts)) continue;

            if (item is Checklist cl)
            {
                var header = table.AddRow(
                    false,
                    new[] {cl.Name},
                    new[] {STYLE_ID_SECTION_HEADER});

                header.AddHorizontalSpan(2);

                ExportItems(cl.Items, table, doc, layout, contexts);
            }
            else if (item is SingleCheckItem check)
            {
                var row = table.AddRow(false,
                    new[] {check.CheckName, check.Value},
                    new[] {STYLE_ID_CHECK_NAME, STYLE_ID_CHECK_VALUE});

                var cells = row.ChildElements.OfType<TableCell>().ToList();

                cells[0].SetCellBorder(CellBorderType.Right, BorderValues.Nil);
                cells[1].SetCellBorder(CellBorderType.Left, BorderValues.Nil);
            }

            if (layout.ShowDescriptions) {
                ExportDescription(item, table, doc);
            }
        }
    }

    private void ExportDescription(ChecklistItem item, Table table, WordprocessingDocument doc)
    {
        if (string.IsNullOrEmpty(item.Description)) return;

        var row = table.AddRow(false, "");
        row.AddHorizontalSpan(2);
        var cell = row.GetFirstChild<TableCell>();
        
        Debug.Assert(cell != null, "cell != null");
        cell.SetCellMargin(0,0 , 200, 400);

        cell.RemoveAllChildren<Paragraph>();
        
        var renderer = new DocxDocumentRenderer(doc, new WordDocumentStyles(), _markdownLogger);
        renderer.Cursor.GoInto(cell);
        Markdown.Convert(item.Description, renderer, _markdownPipeline);

        /*

        Document doc = markdownParser.parse(item.getDescription());

        var row = Docx4jHelper.addRow(table, false, new String[]{null});
        Docx4jHelper.addHorizontalSpan(row, 2);

        var cell = Docx4jHelper.getFirstCell(row);
        Docx4jHelper.addCellMargin(cell, 400);

        var container = new TableCellContentContainer(cell, wordprocessingMLPackage, new boolean[]{true});

        markdownRenderer.render(doc, wordprocessingMLPackage, container);
        
        */

    }
}