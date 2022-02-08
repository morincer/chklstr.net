﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Chklstr.Infra.Export;

public static class MainDocumentExtensions
{
    public static readonly string NS_WORD12 = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
    public static readonly string W_NAMESPACE_DECLARATION = "xmlns:w=\"" + NS_WORD12 + "\"";

    public static ILogger Log { get; set; } = NullLogger.Instance;

    public static Table CreateTable(int rows, int cols, string? styleId)
    {
        if (styleId == null) styleId = "TableGrid";

        var table = new Table();
        var tblPr = new TableProperties();
        table.AppendChild(tblPr);

        tblPr.TableStyle = new TableStyle
        {
            Val = styleId
        };

        var tblW = new TableWidth
        {
            Type = TableWidthUnitValues.Auto,
            Width = "0"
        };

        tblPr.AppendChild(tblW);

        var tblLook = new TableLook()
        {
            Val = "04A0"
        };
        tblPr.AppendChild(tblLook);


        var tblGrid = new TableGrid();
        table.AppendChild(tblGrid);

        for (var row = 0; row < rows; row++)
        {
            var tr = new TableRow();
            table.AppendChild(tr);
            for (var cell = 0; cell < cols; cell++)
            {
                var tc = new TableCell();
                tr.AppendChild(tc);

                tc.AppendChild(new Run(new Text("")));
            }
        }

        return table;
    }

    public static void SetCellBorder(this TableCell tc, CellBorderType borderType, BorderValues? border)
    {
        var tcPr = tc.TableCellProperties;

        if (tcPr == null)
        {
            tcPr = new TableCellProperties();
            tc.TableCellProperties = tcPr;
        }

        var tcBorders = tcPr.TableCellBorders;
        if (tcBorders == null)
        {
            tcBorders = new TableCellBorders();
            tcPr.TableCellBorders = tcBorders;
        }

        switch (borderType)
        {
            case CellBorderType.Left:
                tcBorders.LeftBorder = new LeftBorder() {Val = border};
                break;
            case CellBorderType.Right:
                tcBorders.RightBorder = new RightBorder() {Val = border};
                break;
            case CellBorderType.Top:
                tcBorders.TopBorder = new TopBorder() {Val = border};
                break;
            case CellBorderType.Bottom:
                tcBorders.BottomBorder = new BottomBorder() {Val = border};
                break;
        }
    }


    public static void AddHorizontalSpan(this TableRow row, int span)
    {
        var firstCell = row.Elements<TableCell>().FirstOrDefault();

        if (firstCell == null) return;

        var tcPr = firstCell.GetOrCreateTableCellProperties();

        tcPr.GridSpan ??= new GridSpan();
        tcPr.GridSpan.Val = span;
    }

    public static TableRow AddRow(this Table table, bool isHeader, params string[] values)
    {
        return AddRow(table, isHeader, values, null);
    }

    public static TableRow AddRow(this Table table, bool isHeader, string[] values, string[]? styles)
    {
        var row = new TableRow();
        table.AppendChild(row);
        if (isHeader)
        {
            var trPr = new TableRowProperties();
            row.TableRowProperties = trPr;

            var tblHeader = new TableHeader();
            trPr.AppendChild(tblHeader);
        }

        for (var i = 0; i < values.Length; i++)
        {
            var val = values[i];
            var tc = new TableCell();
            row.AppendChild(tc);

            var text = CreateParagraph(val);
            tc.AppendChild(text);

            if (styles != null && i < styles.Length && !string.IsNullOrEmpty(styles[i]))
            {
                SetStyle(text, styles[i]);
            }
        }

        return row;
    }

    public static Paragraph CreateParagraph(String text)
    {
        var p = new Paragraph(new Run(new Text(text)));
        return p;
    }

    public static void SetStyle(this Paragraph? para, String styleId)
    {
        if (para == null) return;

        var pPr = para.ParagraphProperties;
        if (pPr == null)
        {
            pPr = new ParagraphProperties();
            para.ParagraphProperties = pPr;
        }

        var style = new ParagraphStyleId() {Val = styleId};
        pPr.ParagraphStyleId = style;
    }

    public static Paragraph AddStyledParagraphOfText(this MainDocumentPart mainDocumentPart, string styleId,
        string text)
    {
        var p = CreateParagraphOfText(text);

        mainDocumentPart.Document.Body ??= new Body();
        mainDocumentPart.Document.Body!.AppendChild(p);

        mainDocumentPart.Document.ApplyStyleToParagraph(styleId, p);

        return p;
    }

    public static Paragraph CreateParagraphOfText(string? simpleText)
    {
        var para = new Paragraph();

        if (simpleText == null) return para;

        var splits = simpleText.Replace("\r", "").Split("\n");

        var afterNewline = false;
        var run = new Run();
        foreach (var s in splits)
        {
            if (afterNewline)
            {
                var br = new Break();
                run.AppendChild(br);
            }


            Text t = new Text(s);

            if (s.StartsWith(" ") || s.EndsWith(" "))
            {
                t.Space = SpaceProcessingModeValues.Preserve;
            }

            run.AppendChild(t); // ContentAccessor					

            afterNewline = true;
        }

        para.AppendChild(run); // ContentAccessor

        return para;
    }

    // Apply a style to a paragraph.
    public static void ApplyStyleToParagraph(this Document doc,
        string styleid, Paragraph p)
    {
        // If the paragraph has no ParagraphProperties object, create one.
        if (!p.Elements<ParagraphProperties>().Any())
        {
            p.PrependChild<ParagraphProperties>(new ParagraphProperties());
        }

        // Get the paragraph properties element of the paragraph.
        var pPr = p.Elements<ParagraphProperties>().First();

        // Get the Styles part for this document.
        var part =
            doc.MainDocumentPart!.StyleDefinitionsPart;

        // If the Styles part does not exist, add it and then add the style.
        if (part == null)
        {
            part = AddStylesPartToPackage(doc);
            AddNewStyle(part, styleid, styleid);
        }
        else
        {
            // If the style is not in the document, add it.
            if (IsStyleIdInDocument(doc, styleid) != true)
            {
                // No match on styleid, so let's try style name.
                var styleidFromName = GetStyleIdFromStyleName(doc, styleid);
                if (styleidFromName == null)
                {
                    AddNewStyle(part, styleid, styleid);
                }
                else
                    styleid = styleidFromName;
            }
        }

        // Set the style of the paragraph.
        pPr.ParagraphStyleId = new ParagraphStyleId() {Val = styleid};
    }

    // Return true if the style id is in the document, false otherwise.
    public static bool IsStyleIdInDocument(Document doc,
        string styleid)
    {
        // Get access to the Styles element for this document.
        var s = doc.MainDocumentPart.StyleDefinitionsPart.Styles;

        // Check that there are styles and how many.
        var n = s.Elements<Style>().Count();
        if (n == 0)
            return false;

        // Look for a match on styleid.
        var style = s
            .Elements<Style>()
            .FirstOrDefault(st => st.Type != null && (st.StyleId == styleid) && st.Type == StyleValues.Paragraph);

        return style != null;
    }

    // Return styleid that matches the styleName, or null when there's no match.
    public static string? GetStyleIdFromStyleName(Document doc, string styleName)
    {
        var stylePart = doc.MainDocumentPart?.StyleDefinitionsPart;
        var styleId = stylePart?.Styles?.Descendants<StyleName>()
            .Where(s => styleName.Equals(s.Val?.Value)
                        && s.Parent is Style parent
                        && parent.Type != null
                        && parent.Type == StyleValues.Paragraph)
            .Select(n => n.Parent as Style)
            .Where(n => n != null)
            .Select(n => n!.StyleId).FirstOrDefault();

        return styleId;
    }

    // Create a new style with the specified styleid and stylename and add it to the specified
    // style definitions part.
    private static void AddNewStyle(StyleDefinitionsPart styleDefinitionsPart,
        string styleid, string stylename)
    {
        // Get access to the root element of the styles part.
        Styles styles = styleDefinitionsPart.Styles;

        // Create a new paragraph style and specify some of the properties.
        Style style = new Style()
        {
            Type = StyleValues.Paragraph,
            StyleId = styleid,
            CustomStyle = true
        };
        StyleName styleName1 = new StyleName() {Val = stylename};
        BasedOn basedOn1 = new BasedOn() {Val = "Normal"};
        NextParagraphStyle nextParagraphStyle1 = new NextParagraphStyle() {Val = "Normal"};
        style.Append(styleName1);
        style.Append(basedOn1);
        style.Append(nextParagraphStyle1);

        // Create the StyleRunProperties object and specify some of the run properties.
        StyleRunProperties styleRunProperties1 = new StyleRunProperties();
        Bold bold1 = new Bold();
        Color color1 = new Color() {ThemeColor = ThemeColorValues.Accent2};
        RunFonts font1 = new RunFonts() {Ascii = "Lucida Console"};
        Italic italic1 = new Italic();
        // Specify a 12 point size.
        FontSize fontSize1 = new FontSize() {Val = "24"};
        styleRunProperties1.Append(bold1);
        styleRunProperties1.Append(color1);
        styleRunProperties1.Append(font1);
        styleRunProperties1.Append(fontSize1);
        styleRunProperties1.Append(italic1);

        // Add the run properties to the style.
        style.Append(styleRunProperties1);

        // Add the style to the styles part.
        styles.Append(style);
    }

    // Add a StylesDefinitionsPart to the document.  Returns a reference to it.
    public static StyleDefinitionsPart AddStylesPartToPackage(Document doc)
    {
        StyleDefinitionsPart part;
        part = doc.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
        Styles root = new Styles();
        root.Save(part);
        return part;
    }
    
    private static TableCellProperties GetOrCreateTableCellProperties(this TableCell cell) {
        var result = cell.TableCellProperties;
        
        if (result == null) {
            result = new TableCellProperties();
            cell.TableCellProperties = result;
        }

        return result;
    }
    
    private static TableCellMargin GetOrCreateTableCellMargin(this TableCellProperties props)
    {
        var result = props.TableCellMargin;
        
        if (result == null) {
            result = new TableCellMargin();
            props.TableCellMargin = result;
        }

        return result;
    }

    
    public static void AddCellMargin(this TableCell cell, int val)
    {
        var tcPr = cell.GetOrCreateTableCellProperties();
        var mar = tcPr.GetOrCreateTableCellMargin();

        mar.LeftMargin = new LeftMargin {Width = val.ToString()};
    }

}

public enum CellBorderType
{
    Left,
    Right,
    Top,
    Bottom
}