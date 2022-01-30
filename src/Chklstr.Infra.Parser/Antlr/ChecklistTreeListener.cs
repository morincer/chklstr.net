using Antlr4.Runtime.Misc;
using Chklstr.Core.Model;
using Chklstr.Core.Utils;
using Chklstr.Infra.Parser.Antlr.Gen;

namespace Chklstr.Infra.Parser.Antlr
{
    public class ChecklistTreeListener : ChecklistParserBaseListener
    {
        public QuickReferenceHandbook? Handbook { get; private set; }

        private Checklist? _currentChecklist;
        private ChecklistItem? _currentItem;

        private HashSet<string> _currentContexts = new();


        public List<string> SyntaxErrors { get; }= new();

        private void SyntaxError(string message)
        {
            SyntaxErrors.Add(message);
        }


        public override void ExitCmd_header(ChecklistParser.Cmd_headerContext ctx)
        {
            var name = ctx.title().GetText().Trim();

            this.Handbook = new QuickReferenceHandbook(name);

            if (ctx.contexts() != null)
            {
                this.Handbook.DefaultContexts.AddAll(this._currentContexts);
                this._currentContexts.Clear();
            }
        }


        public override void ExitCmd_checklist(ChecklistParser.Cmd_checklistContext ctx)
        {
            var name = ctx.title().GetText().Trim();
            if (this.Handbook == null)
            {
                SyntaxError("Orphaned checklist " + name);
                return;
            }
            
            this._currentChecklist = this.Handbook.Add(name);
            this._currentItem = _currentChecklist;

            if (ctx.contexts() != null)
            {
                AddContexts();
            }
        }


        public override void ExitCmd_sublist_start(ChecklistParser.Cmd_sublist_startContext ctx)
        {
            var name = ctx.title().GetText().Trim();

            if (this._currentChecklist != null && this._currentChecklist.Parent != null)
            {
                this._currentChecklist = (Checklist) this._currentChecklist.Parent;
            }

            if (this._currentChecklist == null)
            {
                SyntaxError("Orphaned sublist: " + name);
                return;
            }
            
            Checklist list = this._currentChecklist.AddSubList(name);
            _currentItem = list;
            _currentChecklist = list;

            if (ctx.contexts() != null)
            {
                AddContexts();
            }
        }


        public override void ExitCmd_sublist_end(ChecklistParser.Cmd_sublist_endContext ctx)
        {
            if (this._currentChecklist == null)
            {
                SyntaxError("No sublist to close");
                return;
            }

            this._currentChecklist = this._currentChecklist.Parent;
            this._currentItem = this._currentChecklist;
        }


        public override void ExitCmd_docstring(ChecklistParser.Cmd_docstringContext ctx)
        {
            var docstring = ctx.GetText().Trim();
            if (docstring.StartsWith("```"))
            {
                docstring = docstring.Substring(3);
            }

            if (docstring.EndsWith("```"))
            {
                docstring = docstring.Substring(0, docstring.Length - 3);
            }

            if (this._currentItem == null)
            {
                SyntaxError("Orphaned docstring: " + docstring);
                return;
            }

            this._currentItem.Description = docstring.Trim();
        }


        public override void ExitCmd_separator(ChecklistParser.Cmd_separatorContext ctx)
        {
            if (this._currentChecklist == null)
            {
                SyntaxError("Orphaned separator");
                return;
            }
            
            this._currentItem = this._currentChecklist.AddSeparator();
        }

        private void AddContexts()
        {
            if (this._currentItem == null)
            {
                throw new Exception("Orphaned context");
            }

            this._currentItem.Contexts.AddAll(this._currentContexts);
        }


        public override void ExitCmd_checklist_item(ChecklistParser.Cmd_checklist_itemContext ctx)
        {
            var checkName = ctx.checklist_item_name().GetText().Trim();
            var checkValue = ctx.checklist_item_value().GetText().Trim();

            if (this._currentChecklist == null)
            {
                SyntaxError($"Orphaned checklist item: {checkName} ... {checkValue}");
                return;
            }

            if (checkName.Equals(""))
            {
                SyntaxError($"Check name is empty for {ctx.GetText().Trim()}");
                return;
            }

            if (checkValue.Equals(""))
            {
                SyntaxError($"Check value is empty for {ctx.GetText().Trim()}");
                return;
            }

            this._currentItem = this._currentChecklist.AddSingleItem(checkName, checkValue);
            if (ctx.contexts() != null)
            {
                AddContexts();
            }
        }


        public override void ExitContexts(ChecklistParser.ContextsContext ctx)
        {
            this._currentContexts = ctx.Ctx().Select(c => {
                var txt = c.GetText().Trim();

                if (txt.StartsWith("@<"))
                {
                    txt = txt.Substring(2).Trim();
                }

                if (txt.EndsWith(">"))
                {
                    txt = txt.Substring(0, txt.Length - 1).Trim();
                }

                return txt;
            }).ToHashSet();
        }
    }
}