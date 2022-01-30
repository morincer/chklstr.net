parser grammar ChecklistParser;

options {   tokenVocab = ChecklistLexer; }

doc: cmd_header (EOL cmd)+ EOL?;

title: CHR+;
contexts: Ctx+;

cmd_header: Header_Cmd title contexts?;

cmd: cmd_checklist
        | cmd_checklist_item
        | cmd_sublist_start
        | cmd_sublist_end
        | cmd_docstring
        | cmd_separator
;

cmd_checklist: Checklist_Cmd title contexts?;

cmd_checklist_item: Checklist_Item_Cmd checklist_item_name Separator checklist_item_value contexts?;
checklist_item_name: CHR+;
checklist_item_value: CHR+;

cmd_sublist_start: Checklist_Sub_Cmd title contexts?;
cmd_sublist_end:   Checklist_Sub_End_Cmd;

cmd_docstring: DocString_Cmd;
cmd_separator: Separator_Cmd;