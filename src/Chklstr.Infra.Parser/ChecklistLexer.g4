lexer grammar ChecklistLexer;

//Header_Cmd:         HEADER_ WS*             -> mode(TITLE_NO_CTX);
Header_Cmd:         WS* HEADER_ WS*             -> mode(TITLE_CTX);
Checklist_Cmd:      WS* CHECKLIST_ WS*          -> mode(TITLE_CTX);
Checklist_Sub_Cmd:  WS* CHECKLIST_SUB_ WS*      -> mode(TITLE_CTX);
Checklist_Sub_End_Cmd: WS* CHECKLIST_SUB_END_;
DocString_Cmd:      WS* '```' .*? '```' WS* ;
Checklist_Item_Cmd: WS* ITEM_START_ WS*         -> mode(ITEM_NAME);
Separator_Cmd:      WS* '----''-'+ WS* ;

CHECKLIST_SUB_END_: '---//---';
HEADER_:        '>';
CHECKLIST_:     '===';
CHECKLIST_SUB_: '---';
CTX_START_:     '@<';
ITEM_START_:    '[' WS* ']';

EOL:    [\r?\n]+;
WS:     [ \t];
CHR:    ~[\r?\n];

BLANK_LINE: WS EOL  -> skip;
ErrorChar : . ;

/*mode TITLE_NO_CTX;
TitleChar:      CHR                     -> type(CHR);
TitleEnd:       EOL                     -> mode(DEFAULT_MODE), type(EOL);*/

mode TITLE_CTX;
TitleCtxChar:   ~[\r\n]                 -> type(CHR);
TitleCtxMark:   CTX_START_              -> more, mode(CONTEXTS);
TitleCtxEnd:    EOL                     -> mode(DEFAULT_MODE), type(EOL);

mode CONTEXTS;
S_WS:           [ \t]+ -> skip;
Ctx:        '@<'?(~('>'|'\r'|'\n'))+?'>';
CtxEnd:    EOL                          -> mode(DEFAULT_MODE), type(EOL);

mode ITEM_NAME;
ItemNameChar:   CHR                     -> type(CHR);
Separator:      WS* '..''.'+ WS*        -> mode(ITEM_VALUE);
NameErrorChar : . ;

mode ITEM_VALUE;
ItemValueChar:  CHR                     -> type(CHR);
ItemCtxMark:    CTX_START_              -> more, mode(CONTEXTS);
ItemCtxEnd:     EOL                     -> mode(DEFAULT_MODE), type(EOL);
