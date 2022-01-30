//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:/Workspace/chklstr.net/src/Chklstr.Infra.Parser\ChecklistLexer.g4 by ANTLR 4.9.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Chklstr.Infra.Parser.Antlr.Gen {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.2")]
[System.CLSCompliant(false)]
public partial class ChecklistLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		Header_Cmd=1, Checklist_Cmd=2, Checklist_Sub_Cmd=3, Checklist_Sub_End_Cmd=4, 
		DocString_Cmd=5, Checklist_Item_Cmd=6, Separator_Cmd=7, CHECKLIST_SUB_END_=8, 
		HEADER_=9, CHECKLIST_=10, CHECKLIST_SUB_=11, CTX_START_=12, ITEM_START_=13, 
		EOL=14, WS=15, CHR=16, BLANK_LINE=17, ErrorChar=18, S_WS=19, Ctx=20, Separator=21, 
		NameErrorChar=22;
	public const int
		TITLE_CTX=1, CONTEXTS=2, ITEM_NAME=3, ITEM_VALUE=4;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE", "TITLE_CTX", "CONTEXTS", "ITEM_NAME", "ITEM_VALUE"
	};

	public static readonly string[] ruleNames = {
		"Header_Cmd", "Checklist_Cmd", "Checklist_Sub_Cmd", "Checklist_Sub_End_Cmd", 
		"DocString_Cmd", "Checklist_Item_Cmd", "Separator_Cmd", "CHECKLIST_SUB_END_", 
		"HEADER_", "CHECKLIST_", "CHECKLIST_SUB_", "CTX_START_", "ITEM_START_", 
		"EOL", "WS", "CHR", "BLANK_LINE", "ErrorChar", "TitleCtxChar", "TitleCtxMark", 
		"TitleCtxEnd", "S_WS", "Ctx", "CtxEnd", "ItemNameChar", "Separator", "NameErrorChar", 
		"ItemValueChar", "ItemCtxMark", "ItemCtxEnd"
	};


	public ChecklistLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public ChecklistLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, null, null, "'---//---'", "'>'", "'==='", 
		"'---'", "'@<'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "Header_Cmd", "Checklist_Cmd", "Checklist_Sub_Cmd", "Checklist_Sub_End_Cmd", 
		"DocString_Cmd", "Checklist_Item_Cmd", "Separator_Cmd", "CHECKLIST_SUB_END_", 
		"HEADER_", "CHECKLIST_", "CHECKLIST_SUB_", "CTX_START_", "ITEM_START_", 
		"EOL", "WS", "CHR", "BLANK_LINE", "ErrorChar", "S_WS", "Ctx", "Separator", 
		"NameErrorChar"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "ChecklistLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static ChecklistLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '\x18', '\x135', '\b', '\x1', '\b', '\x1', '\b', '\x1', 
		'\b', '\x1', '\b', '\x1', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', '\t', 
		'\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', '\x5', '\x4', '\x6', 
		'\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', '\t', '\b', '\x4', 
		'\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', '\t', '\v', '\x4', 
		'\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', '\t', '\xE', 
		'\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', '\x11', 
		'\t', '\x11', '\x4', '\x12', '\t', '\x12', '\x4', '\x13', '\t', '\x13', 
		'\x4', '\x14', '\t', '\x14', '\x4', '\x15', '\t', '\x15', '\x4', '\x16', 
		'\t', '\x16', '\x4', '\x17', '\t', '\x17', '\x4', '\x18', '\t', '\x18', 
		'\x4', '\x19', '\t', '\x19', '\x4', '\x1A', '\t', '\x1A', '\x4', '\x1B', 
		'\t', '\x1B', '\x4', '\x1C', '\t', '\x1C', '\x4', '\x1D', '\t', '\x1D', 
		'\x4', '\x1E', '\t', '\x1E', '\x4', '\x1F', '\t', '\x1F', '\x3', '\x2', 
		'\a', '\x2', '\x45', '\n', '\x2', '\f', '\x2', '\xE', '\x2', 'H', '\v', 
		'\x2', '\x3', '\x2', '\x3', '\x2', '\a', '\x2', 'L', '\n', '\x2', '\f', 
		'\x2', '\xE', '\x2', 'O', '\v', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', 
		'\x3', '\a', '\x3', 'T', '\n', '\x3', '\f', '\x3', '\xE', '\x3', 'W', 
		'\v', '\x3', '\x3', '\x3', '\x3', '\x3', '\a', '\x3', '[', '\n', '\x3', 
		'\f', '\x3', '\xE', '\x3', '^', '\v', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x4', '\a', '\x4', '\x63', '\n', '\x4', '\f', '\x4', '\xE', '\x4', 
		'\x66', '\v', '\x4', '\x3', '\x4', '\x3', '\x4', '\a', '\x4', 'j', '\n', 
		'\x4', '\f', '\x4', '\xE', '\x4', 'm', '\v', '\x4', '\x3', '\x4', '\x3', 
		'\x4', '\x3', '\x5', '\a', '\x5', 'r', '\n', '\x5', '\f', '\x5', '\xE', 
		'\x5', 'u', '\v', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', '\a', 
		'\x6', 'z', '\n', '\x6', '\f', '\x6', '\xE', '\x6', '}', '\v', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', 
		'\a', '\x6', '\x84', '\n', '\x6', '\f', '\x6', '\xE', '\x6', '\x87', '\v', 
		'\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', 
		'\x6', '\a', '\x6', '\x8E', '\n', '\x6', '\f', '\x6', '\xE', '\x6', '\x91', 
		'\v', '\x6', '\x3', '\a', '\a', '\a', '\x94', '\n', '\a', '\f', '\a', 
		'\xE', '\a', '\x97', '\v', '\a', '\x3', '\a', '\x3', '\a', '\a', '\a', 
		'\x9B', '\n', '\a', '\f', '\a', '\xE', '\a', '\x9E', '\v', '\a', '\x3', 
		'\a', '\x3', '\a', '\x3', '\b', '\a', '\b', '\xA3', '\n', '\b', '\f', 
		'\b', '\xE', '\b', '\xA6', '\v', '\b', '\x3', '\b', '\x3', '\b', '\x3', 
		'\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x6', '\b', '\xAE', '\n', 
		'\b', '\r', '\b', '\xE', '\b', '\xAF', '\x3', '\b', '\a', '\b', '\xB3', 
		'\n', '\b', '\f', '\b', '\xE', '\b', '\xB6', '\v', '\b', '\x3', '\t', 
		'\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', 
		'\t', '\x3', '\t', '\x3', '\t', '\x3', '\n', '\x3', '\n', '\x3', '\v', 
		'\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\f', '\x3', '\f', '\x3', 
		'\f', '\x3', '\f', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\xE', 
		'\x3', '\xE', '\a', '\xE', '\xD0', '\n', '\xE', '\f', '\xE', '\xE', '\xE', 
		'\xD3', '\v', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x6', 
		'\xF', '\xD8', '\n', '\xF', '\r', '\xF', '\xE', '\xF', '\xD9', '\x3', 
		'\x10', '\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x12', '\x3', 
		'\x12', '\x3', '\x12', '\x3', '\x12', '\x3', '\x12', '\x3', '\x13', '\x3', 
		'\x13', '\x3', '\x14', '\x3', '\x14', '\x3', '\x14', '\x3', '\x14', '\x3', 
		'\x15', '\x3', '\x15', '\x3', '\x15', '\x3', '\x15', '\x3', '\x15', '\x3', 
		'\x16', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', 
		'\x17', '\x6', '\x17', '\xF6', '\n', '\x17', '\r', '\x17', '\xE', '\x17', 
		'\xF7', '\x3', '\x17', '\x3', '\x17', '\x3', '\x18', '\x3', '\x18', '\x5', 
		'\x18', '\xFE', '\n', '\x18', '\x3', '\x18', '\x6', '\x18', '\x101', '\n', 
		'\x18', '\r', '\x18', '\xE', '\x18', '\x102', '\x3', '\x18', '\x3', '\x18', 
		'\x3', '\x19', '\x3', '\x19', '\x3', '\x19', '\x3', '\x19', '\x3', '\x19', 
		'\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1B', 
		'\a', '\x1B', '\x111', '\n', '\x1B', '\f', '\x1B', '\xE', '\x1B', '\x114', 
		'\v', '\x1B', '\x3', '\x1B', '\x3', '\x1B', '\x3', '\x1B', '\x3', '\x1B', 
		'\x6', '\x1B', '\x11A', '\n', '\x1B', '\r', '\x1B', '\xE', '\x1B', '\x11B', 
		'\x3', '\x1B', '\a', '\x1B', '\x11F', '\n', '\x1B', '\f', '\x1B', '\xE', 
		'\x1B', '\x122', '\v', '\x1B', '\x3', '\x1B', '\x3', '\x1B', '\x3', '\x1C', 
		'\x3', '\x1C', '\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1D', 
		'\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1E', 
		'\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', 
		'\x4', '\x85', '\x102', '\x2', ' ', '\a', '\x3', '\t', '\x4', '\v', '\x5', 
		'\r', '\x6', '\xF', '\a', '\x11', '\b', '\x13', '\t', '\x15', '\n', '\x17', 
		'\v', '\x19', '\f', '\x1B', '\r', '\x1D', '\xE', '\x1F', '\xF', '!', '\x10', 
		'#', '\x11', '%', '\x12', '\'', '\x13', ')', '\x14', '+', '\x2', '-', 
		'\x2', '/', '\x2', '\x31', '\x15', '\x33', '\x16', '\x35', '\x2', '\x37', 
		'\x2', '\x39', '\x17', ';', '\x18', '=', '\x2', '?', '\x2', '\x41', '\x2', 
		'\a', '\x2', '\x3', '\x4', '\x5', '\x6', '\x6', '\x5', '\x2', '\f', '\f', 
		'\xF', '\xF', '\x41', '\x41', '\x4', '\x2', '\v', '\v', '\"', '\"', '\x4', 
		'\x2', '\f', '\f', '\xF', '\xF', '\x5', '\x2', '\f', '\f', '\xF', '\xF', 
		'@', '@', '\x2', '\x147', '\x2', '\a', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\t', '\x3', '\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\r', '\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x2', '\x13', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x15', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x17', '\x3', '\x2', '\x2', '\x2', '\x2', '\x19', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1D', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'!', '\x3', '\x2', '\x2', '\x2', '\x2', '#', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '%', '\x3', '\x2', '\x2', '\x2', '\x2', '\'', '\x3', '\x2', '\x2', 
		'\x2', '\x2', ')', '\x3', '\x2', '\x2', '\x2', '\x3', '+', '\x3', '\x2', 
		'\x2', '\x2', '\x3', '-', '\x3', '\x2', '\x2', '\x2', '\x3', '/', '\x3', 
		'\x2', '\x2', '\x2', '\x4', '\x31', '\x3', '\x2', '\x2', '\x2', '\x4', 
		'\x33', '\x3', '\x2', '\x2', '\x2', '\x4', '\x35', '\x3', '\x2', '\x2', 
		'\x2', '\x5', '\x37', '\x3', '\x2', '\x2', '\x2', '\x5', '\x39', '\x3', 
		'\x2', '\x2', '\x2', '\x5', ';', '\x3', '\x2', '\x2', '\x2', '\x6', '=', 
		'\x3', '\x2', '\x2', '\x2', '\x6', '?', '\x3', '\x2', '\x2', '\x2', '\x6', 
		'\x41', '\x3', '\x2', '\x2', '\x2', '\a', '\x46', '\x3', '\x2', '\x2', 
		'\x2', '\t', 'U', '\x3', '\x2', '\x2', '\x2', '\v', '\x64', '\x3', '\x2', 
		'\x2', '\x2', '\r', 's', '\x3', '\x2', '\x2', '\x2', '\xF', '{', '\x3', 
		'\x2', '\x2', '\x2', '\x11', '\x95', '\x3', '\x2', '\x2', '\x2', '\x13', 
		'\xA4', '\x3', '\x2', '\x2', '\x2', '\x15', '\xB7', '\x3', '\x2', '\x2', 
		'\x2', '\x17', '\xC0', '\x3', '\x2', '\x2', '\x2', '\x19', '\xC2', '\x3', 
		'\x2', '\x2', '\x2', '\x1B', '\xC6', '\x3', '\x2', '\x2', '\x2', '\x1D', 
		'\xCA', '\x3', '\x2', '\x2', '\x2', '\x1F', '\xCD', '\x3', '\x2', '\x2', 
		'\x2', '!', '\xD7', '\x3', '\x2', '\x2', '\x2', '#', '\xDB', '\x3', '\x2', 
		'\x2', '\x2', '%', '\xDD', '\x3', '\x2', '\x2', '\x2', '\'', '\xDF', '\x3', 
		'\x2', '\x2', '\x2', ')', '\xE4', '\x3', '\x2', '\x2', '\x2', '+', '\xE6', 
		'\x3', '\x2', '\x2', '\x2', '-', '\xEA', '\x3', '\x2', '\x2', '\x2', '/', 
		'\xEF', '\x3', '\x2', '\x2', '\x2', '\x31', '\xF5', '\x3', '\x2', '\x2', 
		'\x2', '\x33', '\xFD', '\x3', '\x2', '\x2', '\x2', '\x35', '\x106', '\x3', 
		'\x2', '\x2', '\x2', '\x37', '\x10B', '\x3', '\x2', '\x2', '\x2', '\x39', 
		'\x112', '\x3', '\x2', '\x2', '\x2', ';', '\x125', '\x3', '\x2', '\x2', 
		'\x2', '=', '\x127', '\x3', '\x2', '\x2', '\x2', '?', '\x12B', '\x3', 
		'\x2', '\x2', '\x2', '\x41', '\x130', '\x3', '\x2', '\x2', '\x2', '\x43', 
		'\x45', '\x5', '#', '\x10', '\x2', '\x44', '\x43', '\x3', '\x2', '\x2', 
		'\x2', '\x45', 'H', '\x3', '\x2', '\x2', '\x2', '\x46', '\x44', '\x3', 
		'\x2', '\x2', '\x2', '\x46', 'G', '\x3', '\x2', '\x2', '\x2', 'G', 'I', 
		'\x3', '\x2', '\x2', '\x2', 'H', '\x46', '\x3', '\x2', '\x2', '\x2', 'I', 
		'M', '\x5', '\x17', '\n', '\x2', 'J', 'L', '\x5', '#', '\x10', '\x2', 
		'K', 'J', '\x3', '\x2', '\x2', '\x2', 'L', 'O', '\x3', '\x2', '\x2', '\x2', 
		'M', 'K', '\x3', '\x2', '\x2', '\x2', 'M', 'N', '\x3', '\x2', '\x2', '\x2', 
		'N', 'P', '\x3', '\x2', '\x2', '\x2', 'O', 'M', '\x3', '\x2', '\x2', '\x2', 
		'P', 'Q', '\b', '\x2', '\x2', '\x2', 'Q', '\b', '\x3', '\x2', '\x2', '\x2', 
		'R', 'T', '\x5', '#', '\x10', '\x2', 'S', 'R', '\x3', '\x2', '\x2', '\x2', 
		'T', 'W', '\x3', '\x2', '\x2', '\x2', 'U', 'S', '\x3', '\x2', '\x2', '\x2', 
		'U', 'V', '\x3', '\x2', '\x2', '\x2', 'V', 'X', '\x3', '\x2', '\x2', '\x2', 
		'W', 'U', '\x3', '\x2', '\x2', '\x2', 'X', '\\', '\x5', '\x19', '\v', 
		'\x2', 'Y', '[', '\x5', '#', '\x10', '\x2', 'Z', 'Y', '\x3', '\x2', '\x2', 
		'\x2', '[', '^', '\x3', '\x2', '\x2', '\x2', '\\', 'Z', '\x3', '\x2', 
		'\x2', '\x2', '\\', ']', '\x3', '\x2', '\x2', '\x2', ']', '_', '\x3', 
		'\x2', '\x2', '\x2', '^', '\\', '\x3', '\x2', '\x2', '\x2', '_', '`', 
		'\b', '\x3', '\x2', '\x2', '`', '\n', '\x3', '\x2', '\x2', '\x2', '\x61', 
		'\x63', '\x5', '#', '\x10', '\x2', '\x62', '\x61', '\x3', '\x2', '\x2', 
		'\x2', '\x63', '\x66', '\x3', '\x2', '\x2', '\x2', '\x64', '\x62', '\x3', 
		'\x2', '\x2', '\x2', '\x64', '\x65', '\x3', '\x2', '\x2', '\x2', '\x65', 
		'g', '\x3', '\x2', '\x2', '\x2', '\x66', '\x64', '\x3', '\x2', '\x2', 
		'\x2', 'g', 'k', '\x5', '\x1B', '\f', '\x2', 'h', 'j', '\x5', '#', '\x10', 
		'\x2', 'i', 'h', '\x3', '\x2', '\x2', '\x2', 'j', 'm', '\x3', '\x2', '\x2', 
		'\x2', 'k', 'i', '\x3', '\x2', '\x2', '\x2', 'k', 'l', '\x3', '\x2', '\x2', 
		'\x2', 'l', 'n', '\x3', '\x2', '\x2', '\x2', 'm', 'k', '\x3', '\x2', '\x2', 
		'\x2', 'n', 'o', '\b', '\x4', '\x2', '\x2', 'o', '\f', '\x3', '\x2', '\x2', 
		'\x2', 'p', 'r', '\x5', '#', '\x10', '\x2', 'q', 'p', '\x3', '\x2', '\x2', 
		'\x2', 'r', 'u', '\x3', '\x2', '\x2', '\x2', 's', 'q', '\x3', '\x2', '\x2', 
		'\x2', 's', 't', '\x3', '\x2', '\x2', '\x2', 't', 'v', '\x3', '\x2', '\x2', 
		'\x2', 'u', 's', '\x3', '\x2', '\x2', '\x2', 'v', 'w', '\x5', '\x15', 
		'\t', '\x2', 'w', '\xE', '\x3', '\x2', '\x2', '\x2', 'x', 'z', '\x5', 
		'#', '\x10', '\x2', 'y', 'x', '\x3', '\x2', '\x2', '\x2', 'z', '}', '\x3', 
		'\x2', '\x2', '\x2', '{', 'y', '\x3', '\x2', '\x2', '\x2', '{', '|', '\x3', 
		'\x2', '\x2', '\x2', '|', '~', '\x3', '\x2', '\x2', '\x2', '}', '{', '\x3', 
		'\x2', '\x2', '\x2', '~', '\x7F', '\a', '\x62', '\x2', '\x2', '\x7F', 
		'\x80', '\a', '\x62', '\x2', '\x2', '\x80', '\x81', '\a', '\x62', '\x2', 
		'\x2', '\x81', '\x85', '\x3', '\x2', '\x2', '\x2', '\x82', '\x84', '\v', 
		'\x2', '\x2', '\x2', '\x83', '\x82', '\x3', '\x2', '\x2', '\x2', '\x84', 
		'\x87', '\x3', '\x2', '\x2', '\x2', '\x85', '\x86', '\x3', '\x2', '\x2', 
		'\x2', '\x85', '\x83', '\x3', '\x2', '\x2', '\x2', '\x86', '\x88', '\x3', 
		'\x2', '\x2', '\x2', '\x87', '\x85', '\x3', '\x2', '\x2', '\x2', '\x88', 
		'\x89', '\a', '\x62', '\x2', '\x2', '\x89', '\x8A', '\a', '\x62', '\x2', 
		'\x2', '\x8A', '\x8B', '\a', '\x62', '\x2', '\x2', '\x8B', '\x8F', '\x3', 
		'\x2', '\x2', '\x2', '\x8C', '\x8E', '\x5', '#', '\x10', '\x2', '\x8D', 
		'\x8C', '\x3', '\x2', '\x2', '\x2', '\x8E', '\x91', '\x3', '\x2', '\x2', 
		'\x2', '\x8F', '\x8D', '\x3', '\x2', '\x2', '\x2', '\x8F', '\x90', '\x3', 
		'\x2', '\x2', '\x2', '\x90', '\x10', '\x3', '\x2', '\x2', '\x2', '\x91', 
		'\x8F', '\x3', '\x2', '\x2', '\x2', '\x92', '\x94', '\x5', '#', '\x10', 
		'\x2', '\x93', '\x92', '\x3', '\x2', '\x2', '\x2', '\x94', '\x97', '\x3', 
		'\x2', '\x2', '\x2', '\x95', '\x93', '\x3', '\x2', '\x2', '\x2', '\x95', 
		'\x96', '\x3', '\x2', '\x2', '\x2', '\x96', '\x98', '\x3', '\x2', '\x2', 
		'\x2', '\x97', '\x95', '\x3', '\x2', '\x2', '\x2', '\x98', '\x9C', '\x5', 
		'\x1F', '\xE', '\x2', '\x99', '\x9B', '\x5', '#', '\x10', '\x2', '\x9A', 
		'\x99', '\x3', '\x2', '\x2', '\x2', '\x9B', '\x9E', '\x3', '\x2', '\x2', 
		'\x2', '\x9C', '\x9A', '\x3', '\x2', '\x2', '\x2', '\x9C', '\x9D', '\x3', 
		'\x2', '\x2', '\x2', '\x9D', '\x9F', '\x3', '\x2', '\x2', '\x2', '\x9E', 
		'\x9C', '\x3', '\x2', '\x2', '\x2', '\x9F', '\xA0', '\b', '\a', '\x3', 
		'\x2', '\xA0', '\x12', '\x3', '\x2', '\x2', '\x2', '\xA1', '\xA3', '\x5', 
		'#', '\x10', '\x2', '\xA2', '\xA1', '\x3', '\x2', '\x2', '\x2', '\xA3', 
		'\xA6', '\x3', '\x2', '\x2', '\x2', '\xA4', '\xA2', '\x3', '\x2', '\x2', 
		'\x2', '\xA4', '\xA5', '\x3', '\x2', '\x2', '\x2', '\xA5', '\xA7', '\x3', 
		'\x2', '\x2', '\x2', '\xA6', '\xA4', '\x3', '\x2', '\x2', '\x2', '\xA7', 
		'\xA8', '\a', '/', '\x2', '\x2', '\xA8', '\xA9', '\a', '/', '\x2', '\x2', 
		'\xA9', '\xAA', '\a', '/', '\x2', '\x2', '\xAA', '\xAB', '\a', '/', '\x2', 
		'\x2', '\xAB', '\xAD', '\x3', '\x2', '\x2', '\x2', '\xAC', '\xAE', '\a', 
		'/', '\x2', '\x2', '\xAD', '\xAC', '\x3', '\x2', '\x2', '\x2', '\xAE', 
		'\xAF', '\x3', '\x2', '\x2', '\x2', '\xAF', '\xAD', '\x3', '\x2', '\x2', 
		'\x2', '\xAF', '\xB0', '\x3', '\x2', '\x2', '\x2', '\xB0', '\xB4', '\x3', 
		'\x2', '\x2', '\x2', '\xB1', '\xB3', '\x5', '#', '\x10', '\x2', '\xB2', 
		'\xB1', '\x3', '\x2', '\x2', '\x2', '\xB3', '\xB6', '\x3', '\x2', '\x2', 
		'\x2', '\xB4', '\xB2', '\x3', '\x2', '\x2', '\x2', '\xB4', '\xB5', '\x3', 
		'\x2', '\x2', '\x2', '\xB5', '\x14', '\x3', '\x2', '\x2', '\x2', '\xB6', 
		'\xB4', '\x3', '\x2', '\x2', '\x2', '\xB7', '\xB8', '\a', '/', '\x2', 
		'\x2', '\xB8', '\xB9', '\a', '/', '\x2', '\x2', '\xB9', '\xBA', '\a', 
		'/', '\x2', '\x2', '\xBA', '\xBB', '\a', '\x31', '\x2', '\x2', '\xBB', 
		'\xBC', '\a', '\x31', '\x2', '\x2', '\xBC', '\xBD', '\a', '/', '\x2', 
		'\x2', '\xBD', '\xBE', '\a', '/', '\x2', '\x2', '\xBE', '\xBF', '\a', 
		'/', '\x2', '\x2', '\xBF', '\x16', '\x3', '\x2', '\x2', '\x2', '\xC0', 
		'\xC1', '\a', '@', '\x2', '\x2', '\xC1', '\x18', '\x3', '\x2', '\x2', 
		'\x2', '\xC2', '\xC3', '\a', '?', '\x2', '\x2', '\xC3', '\xC4', '\a', 
		'?', '\x2', '\x2', '\xC4', '\xC5', '\a', '?', '\x2', '\x2', '\xC5', '\x1A', 
		'\x3', '\x2', '\x2', '\x2', '\xC6', '\xC7', '\a', '/', '\x2', '\x2', '\xC7', 
		'\xC8', '\a', '/', '\x2', '\x2', '\xC8', '\xC9', '\a', '/', '\x2', '\x2', 
		'\xC9', '\x1C', '\x3', '\x2', '\x2', '\x2', '\xCA', '\xCB', '\a', '\x42', 
		'\x2', '\x2', '\xCB', '\xCC', '\a', '>', '\x2', '\x2', '\xCC', '\x1E', 
		'\x3', '\x2', '\x2', '\x2', '\xCD', '\xD1', '\a', ']', '\x2', '\x2', '\xCE', 
		'\xD0', '\x5', '#', '\x10', '\x2', '\xCF', '\xCE', '\x3', '\x2', '\x2', 
		'\x2', '\xD0', '\xD3', '\x3', '\x2', '\x2', '\x2', '\xD1', '\xCF', '\x3', 
		'\x2', '\x2', '\x2', '\xD1', '\xD2', '\x3', '\x2', '\x2', '\x2', '\xD2', 
		'\xD4', '\x3', '\x2', '\x2', '\x2', '\xD3', '\xD1', '\x3', '\x2', '\x2', 
		'\x2', '\xD4', '\xD5', '\a', '_', '\x2', '\x2', '\xD5', ' ', '\x3', '\x2', 
		'\x2', '\x2', '\xD6', '\xD8', '\t', '\x2', '\x2', '\x2', '\xD7', '\xD6', 
		'\x3', '\x2', '\x2', '\x2', '\xD8', '\xD9', '\x3', '\x2', '\x2', '\x2', 
		'\xD9', '\xD7', '\x3', '\x2', '\x2', '\x2', '\xD9', '\xDA', '\x3', '\x2', 
		'\x2', '\x2', '\xDA', '\"', '\x3', '\x2', '\x2', '\x2', '\xDB', '\xDC', 
		'\t', '\x3', '\x2', '\x2', '\xDC', '$', '\x3', '\x2', '\x2', '\x2', '\xDD', 
		'\xDE', '\n', '\x2', '\x2', '\x2', '\xDE', '&', '\x3', '\x2', '\x2', '\x2', 
		'\xDF', '\xE0', '\x5', '#', '\x10', '\x2', '\xE0', '\xE1', '\x5', '!', 
		'\xF', '\x2', '\xE1', '\xE2', '\x3', '\x2', '\x2', '\x2', '\xE2', '\xE3', 
		'\b', '\x12', '\x4', '\x2', '\xE3', '(', '\x3', '\x2', '\x2', '\x2', '\xE4', 
		'\xE5', '\v', '\x2', '\x2', '\x2', '\xE5', '*', '\x3', '\x2', '\x2', '\x2', 
		'\xE6', '\xE7', '\n', '\x4', '\x2', '\x2', '\xE7', '\xE8', '\x3', '\x2', 
		'\x2', '\x2', '\xE8', '\xE9', '\b', '\x14', '\x5', '\x2', '\xE9', ',', 
		'\x3', '\x2', '\x2', '\x2', '\xEA', '\xEB', '\x5', '\x1D', '\r', '\x2', 
		'\xEB', '\xEC', '\x3', '\x2', '\x2', '\x2', '\xEC', '\xED', '\b', '\x15', 
		'\x6', '\x2', '\xED', '\xEE', '\b', '\x15', '\a', '\x2', '\xEE', '.', 
		'\x3', '\x2', '\x2', '\x2', '\xEF', '\xF0', '\x5', '!', '\xF', '\x2', 
		'\xF0', '\xF1', '\x3', '\x2', '\x2', '\x2', '\xF1', '\xF2', '\b', '\x16', 
		'\b', '\x2', '\xF2', '\xF3', '\b', '\x16', '\t', '\x2', '\xF3', '\x30', 
		'\x3', '\x2', '\x2', '\x2', '\xF4', '\xF6', '\t', '\x3', '\x2', '\x2', 
		'\xF5', '\xF4', '\x3', '\x2', '\x2', '\x2', '\xF6', '\xF7', '\x3', '\x2', 
		'\x2', '\x2', '\xF7', '\xF5', '\x3', '\x2', '\x2', '\x2', '\xF7', '\xF8', 
		'\x3', '\x2', '\x2', '\x2', '\xF8', '\xF9', '\x3', '\x2', '\x2', '\x2', 
		'\xF9', '\xFA', '\b', '\x17', '\x4', '\x2', '\xFA', '\x32', '\x3', '\x2', 
		'\x2', '\x2', '\xFB', '\xFC', '\a', '\x42', '\x2', '\x2', '\xFC', '\xFE', 
		'\a', '>', '\x2', '\x2', '\xFD', '\xFB', '\x3', '\x2', '\x2', '\x2', '\xFD', 
		'\xFE', '\x3', '\x2', '\x2', '\x2', '\xFE', '\x100', '\x3', '\x2', '\x2', 
		'\x2', '\xFF', '\x101', '\n', '\x5', '\x2', '\x2', '\x100', '\xFF', '\x3', 
		'\x2', '\x2', '\x2', '\x101', '\x102', '\x3', '\x2', '\x2', '\x2', '\x102', 
		'\x103', '\x3', '\x2', '\x2', '\x2', '\x102', '\x100', '\x3', '\x2', '\x2', 
		'\x2', '\x103', '\x104', '\x3', '\x2', '\x2', '\x2', '\x104', '\x105', 
		'\a', '@', '\x2', '\x2', '\x105', '\x34', '\x3', '\x2', '\x2', '\x2', 
		'\x106', '\x107', '\x5', '!', '\xF', '\x2', '\x107', '\x108', '\x3', '\x2', 
		'\x2', '\x2', '\x108', '\x109', '\b', '\x19', '\b', '\x2', '\x109', '\x10A', 
		'\b', '\x19', '\t', '\x2', '\x10A', '\x36', '\x3', '\x2', '\x2', '\x2', 
		'\x10B', '\x10C', '\x5', '%', '\x11', '\x2', '\x10C', '\x10D', '\x3', 
		'\x2', '\x2', '\x2', '\x10D', '\x10E', '\b', '\x1A', '\x5', '\x2', '\x10E', 
		'\x38', '\x3', '\x2', '\x2', '\x2', '\x10F', '\x111', '\x5', '#', '\x10', 
		'\x2', '\x110', '\x10F', '\x3', '\x2', '\x2', '\x2', '\x111', '\x114', 
		'\x3', '\x2', '\x2', '\x2', '\x112', '\x110', '\x3', '\x2', '\x2', '\x2', 
		'\x112', '\x113', '\x3', '\x2', '\x2', '\x2', '\x113', '\x115', '\x3', 
		'\x2', '\x2', '\x2', '\x114', '\x112', '\x3', '\x2', '\x2', '\x2', '\x115', 
		'\x116', '\a', '\x30', '\x2', '\x2', '\x116', '\x117', '\a', '\x30', '\x2', 
		'\x2', '\x117', '\x119', '\x3', '\x2', '\x2', '\x2', '\x118', '\x11A', 
		'\a', '\x30', '\x2', '\x2', '\x119', '\x118', '\x3', '\x2', '\x2', '\x2', 
		'\x11A', '\x11B', '\x3', '\x2', '\x2', '\x2', '\x11B', '\x119', '\x3', 
		'\x2', '\x2', '\x2', '\x11B', '\x11C', '\x3', '\x2', '\x2', '\x2', '\x11C', 
		'\x120', '\x3', '\x2', '\x2', '\x2', '\x11D', '\x11F', '\x5', '#', '\x10', 
		'\x2', '\x11E', '\x11D', '\x3', '\x2', '\x2', '\x2', '\x11F', '\x122', 
		'\x3', '\x2', '\x2', '\x2', '\x120', '\x11E', '\x3', '\x2', '\x2', '\x2', 
		'\x120', '\x121', '\x3', '\x2', '\x2', '\x2', '\x121', '\x123', '\x3', 
		'\x2', '\x2', '\x2', '\x122', '\x120', '\x3', '\x2', '\x2', '\x2', '\x123', 
		'\x124', '\b', '\x1B', '\n', '\x2', '\x124', ':', '\x3', '\x2', '\x2', 
		'\x2', '\x125', '\x126', '\v', '\x2', '\x2', '\x2', '\x126', '<', '\x3', 
		'\x2', '\x2', '\x2', '\x127', '\x128', '\x5', '%', '\x11', '\x2', '\x128', 
		'\x129', '\x3', '\x2', '\x2', '\x2', '\x129', '\x12A', '\b', '\x1D', '\x5', 
		'\x2', '\x12A', '>', '\x3', '\x2', '\x2', '\x2', '\x12B', '\x12C', '\x5', 
		'\x1D', '\r', '\x2', '\x12C', '\x12D', '\x3', '\x2', '\x2', '\x2', '\x12D', 
		'\x12E', '\b', '\x1E', '\x6', '\x2', '\x12E', '\x12F', '\b', '\x1E', '\a', 
		'\x2', '\x12F', '@', '\x3', '\x2', '\x2', '\x2', '\x130', '\x131', '\x5', 
		'!', '\xF', '\x2', '\x131', '\x132', '\x3', '\x2', '\x2', '\x2', '\x132', 
		'\x133', '\b', '\x1F', '\b', '\x2', '\x133', '\x134', '\b', '\x1F', '\t', 
		'\x2', '\x134', '\x42', '\x3', '\x2', '\x2', '\x2', '\x1E', '\x2', '\x3', 
		'\x4', '\x5', '\x6', '\x46', 'M', 'U', '\\', '\x64', 'k', 's', '{', '\x85', 
		'\x8F', '\x95', '\x9C', '\xA4', '\xAF', '\xB4', '\xD1', '\xD9', '\xF7', 
		'\xFD', '\x102', '\x112', '\x11B', '\x120', '\v', '\x4', '\x3', '\x2', 
		'\x4', '\x5', '\x2', '\b', '\x2', '\x2', '\t', '\x12', '\x2', '\x5', '\x2', 
		'\x2', '\x4', '\x4', '\x2', '\x4', '\x2', '\x2', '\t', '\x10', '\x2', 
		'\x4', '\x6', '\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace Chklstr.Infra.Parser.Antlr.Gen
