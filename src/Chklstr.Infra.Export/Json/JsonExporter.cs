using System.Text;
using Chklstr.Core.Model;
using Chklstr.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chklstr.Infra.Export.Json;

public class JsonExporter
{
    public async Task Export(QuickReferenceHandbook book, string outputPath, params string[] contexts)
    {
        var result = ExportToString(book, contexts);
        await File.WriteAllTextAsync(outputPath, result);
    }

    public string ExportToString(QuickReferenceHandbook book, params string[] contexts)
    {
        var sb = new StringBuilder();
        var sw = new StringWriter();
        using var writer = new JsonTextWriter(sw);

        var root = new JObject
        {
            new JProperty("aircraftName", book.AircraftName),
            new JProperty("defaultContexts", new JArray(book.DefaultContexts.Select(c => new JValue(c)))),
            new JProperty("allAvailableContexts",
                new JArray(book.GetAllAvailableContexts().Select(c => new JValue(c)))),
            new JProperty("selectedContexts", new JArray(contexts.Select(c => new JValue(c)))),
            new JProperty("checklists", new JArray(book.Checklists.Select(cl => export(cl, contexts))))
        };

        var result = root.ToString();
        return result;
    }

    private JObject export(Checklist checklist, string[] contexts)
    {
        var checklistNode = new JObject(
            new JProperty("name", checklist.Name),
            new JProperty("type", checklist.Parent == null ? "checklist" : "section")
        );

        addDescription(checklist, checklistNode);
        addContexts(checklist, checklistNode, contexts);

        var itemsNode = new JArray();
        checklistNode["items"] = itemsNode;

        foreach (var item in checklist.Items)
        {
            if (item is Checklist checklistItem)
            {
                itemsNode.Add(export(checklistItem, contexts));
            }
            else
            {
                var itemNode = new JObject();
                itemsNode.Add(itemNode);
                addContexts(item, itemNode, contexts);

                switch (item)
                {
                    case SingleCheckItem singleCheckItem:
                        itemNode["type"] = "singlecheck";
                        itemNode["name"] = singleCheckItem.CheckName;
                        itemNode["value"] = singleCheckItem.Value;
                        break;
                    case Separator:
                        itemNode["type"] = "separator";
                        break;
                    default:
                        throw new NotImplementedException();
                }

                addDescription(item, itemNode);
            }
        }

        return checklistNode;
    }

    private void addContexts(ChecklistItem item, JObject node, string[] currentContexts)
    {
        if (!item.Contexts.IsEmpty())
        {
            var contextsNode = new JArray(
                item.Contexts.Select(c => new JValue(c))
            );

            node["contexts"] = contextsNode;
        }

        node["availableInCurrentContexts"] = item.IsAvailableInContext(currentContexts);
    }

    private void addDescription(ChecklistItem item, JObject itemNode)
    {
        if (string.IsNullOrEmpty(item.Description)) return;

        itemNode["description"] = new JObject(
            new JProperty("markdown", item.Description),
            new JProperty("html", item.DescriptionAsHtml())
        );
    }
}