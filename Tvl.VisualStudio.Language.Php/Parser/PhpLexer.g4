lexer grammar PhpLexer;

@members
{
	protected const int EOF = Eof;
	protected const int HIDDEN = Hidden;
}

tokens {
	DOC_COMMENT_INVALID_TAG,
}

WS
	:	[ \t]+
		-> channel(HIDDEN)
	;

NEWLINE
	:	'\r'? '\n'
		-> channel(HIDDEN)
	;

HTML_START_CODE
	:	('<?php' | '<?=')
		-> pushMode(PhpCode)
	;

TEXT
	:	~[<\r\n]+
		-> channel(HIDDEN)
	;

ANYCHAR
	:	.
	;

mode PhpCode;

	PhpCode_NEWLINE : NEWLINE -> type(NEWLINE), channel(HIDDEN);
	PhpCode_WS : WS -> type(WS), channel(HIDDEN);

	// keywords
	KW___CLASS__ : '__CLASS__';
	KW___DIR__ : '__DIR__';
	KW___FILE__ : '__FILE__';
	KW___FUNCTION__ : '__FUNCTION__';
	KW___LINE__ : '__LINE__';
	KW___METHOD__ : '__METHOD__';
	KW___NAMESPACE__ : '__NAMESPACE__';
	KW_ABSTRACT : 'abstract';
	KW_AND : 'and';
	KW_AS : 'as';
	KW_BREAK : 'break';
	KW_CASE : 'case';
	KW_CATCH : 'catch';
	KW_CLASS : 'class';
	KW_CLONE : 'clone';
	KW_CONST : 'const';
	KW_CONTINUE : 'continue';
	KW_DECLARE : 'declare';
	KW_DEFAULT : 'default';
	KW_DO : 'do';
	KW_ELSE : 'else';
	KW_ELSEIF : 'elseif';
	KW_ENDDECLARE : 'enddeclare';
	KW_ENDFOR : 'endfor';
	KW_ENDFOREACH : 'endforeach';
	KW_ENDIF : 'endif';
	KW_ENDSWITCH : 'endswitch';
	KW_ENDWHILE : 'endwhile';
	KW_EXCEPTION : 'exception';
	KW_EXTENDS : 'extends';
	KW_FINAL : 'final';
	KW_FOR : 'for';
	KW_FOREACH : 'foreach';
	KW_FUNCTION : 'function';
	KW_GLOBAL : 'global';
	KW_IF : 'if';
	KW_IMPLEMENTS : 'implements';
	KW_INSTANCEOF : 'instanceof';
	KW_INTERFACE : 'interface';
	KW_NAMESPACE : 'namespace';
	KW_NEW : 'new';
	KW_OR : 'or';
	KW_PHP_USER_FILTER : 'php_user_filter';
	KW_PRIVATE : 'private';
	KW_PROTECTED : 'protected';
	KW_PUBLIC : 'public';
	KW_RETURN : 'return';
	KW_STATIC : 'static';
	KW_SWITCH : 'switch';
	KW_THIS : 'this';
	KW_THROW : 'throw';
	KW_TRY : 'try';
	KW_USE : 'use';
	KW_VAR : 'var';
	KW_WHILE : 'while';
	KW_XOR : 'xor';

	CLOSE_PHP_TAG
		:	'?>' -> popMode
		;

	LSHIFTEQ: '<<=';

	PHP_HEREDOC_START
		:	'<<<' PHP_IDENTIFIER {_input.La(1) == '\r' || _input.La(1) == '\n'}?
			-> pushMode(PhpHereDoc)
		;

	LSHIFT
		:	'<<'
		;

	ADDEQ	: '+=';
	SUBEQ	: '-=';
	MULEQ	: '*=';
	DIVEQ	: '/=';
	DOTEQ	: '.=';
	MODEQ	: '%=';
	ANDEQ	: '&=';
	OREQ	: '|=';
	XOREQ	: '^=';
	RSHIFTEQ: '>>=';
	ASSOC	: '=>';

	RSHIFT	: '>>';
	LE		: '<=';
	LT		: '<';
	GE		: '>=';
	GT		: '>';
	INC		: '++';
	DEC		: '--';
	TILDE	: '~';
	ARROW	: '->';
	SUB		: '-';
	AT		: '@';
	NOT		: '!';
	MUL		: '*';
	DOT		: '.';
	LTGT	: '<>';
	ANDAND	: '&&';
	AND		: '&';
	XOR		: '^';
	OROR	: '||';
	OR		: '|';
	QUESTION: '?';
	COLON	: ':';
	LBRACE	: '{';
	RBRACE	: '}';
	LBRACK	: '[';
	RBRACK	: ']';
	LPAREN	: '(';
	RPAREN	: ')';
	COMMA	: ',';
	NEQEQ	: '!==';
	NEQ		: '!=';
	EQEQEQ	: '===';
	EQEQ	: '==';
	EQ		: '=';

	PHP_IDENTIFIER
		:	[a-zA-Z_$] [a-zA-Z0-9_$]*
		;

	PHP_NUMBER
		:	(	'0'..'9'
			| 	'.' '0'..'9'
			)
			(PHP_IDENTIFIER PHP_NUMBER?)?
		;

	PHP_COMMENT
		:	'//' (~('\r' | '\n'))*
			-> channel(HIDDEN)
		;

	PHP_ML_COMMENT
		:	'/*' .*? ('*/' | EOF) -> channel(HIDDEN)
		;

	PHP_SINGLE_STRING_LITERAL
		:	'\''
			(	~['\\]
			|	'\\\''
			|	{_input.LA(1) != '\''}? '\\'
			)*
			'\''?
		;

	PHP_DOUBLE_STRING_LITERAL
		:	'"' -> pushMode(PhpDoubleString)
		;

	fragment
	HEXDIGIT
		:	[0-9a-fA-F]
		;

	PhpCode_ANYCHAR : ANYCHAR -> type(ANYCHAR);

mode BlockComment;

	BlockComment_NEWLINE : NEWLINE -> type(NEWLINE);

	BlockComment_TEXT
		:	(	~[\r\n*]
			|	'*' ~[\r\n/]
			)+
			-> type(PHP_ML_COMMENT)
		;
	BlockComment_STAR : '*' -> type(PHP_ML_COMMENT);

	END_BLOCK_COMMENT : '*/' -> type(PHP_ML_COMMENT), popMode;

mode PhpHereDoc;

	PhpHereDoc_NEWLINE : NEWLINE -> type(NEWLINE);

	PHP_HEREDOC_END
		:	{_input.La(-1) == '\n'}?
			PHP_IDENTIFIER ';'?
			{CheckHeredocEnd(_input.La(1), Text);}?
			-> popMode
		;

	PHP_HEREDOC_TEXT
		:	~[\r\n]+
		;

mode PhpDoubleString;

	PhpDoubleString_ARROW : ARROW -> type(ARROW);
	PhpDoubleString_LBRACK : LBRACK -> type(LBRACK);
	PhpDoubleString_RBRACK : RBRACK -> type(RBRACK);
	PhpDoubleString_PHP_IDENTIFIER : PHP_IDENTIFIER -> type(PHP_IDENTIFIER);
	PhpDoubleString_WS : WS -> type(WS);

	PhpDoubleString_LBRACE : LBRACE {_input.La(1) == '\$'}? -> type(LBRACE);
	PhpDoubleString_RBRACE : RBRACE {StringBraceLevel > 0}? -> type(RBRACE);

	DOUBLE_STRING_ESCAPE
		:	'\\'
			(	('n' | 'r' | 't' | 'v' | 'f' | '\\' | '$' | '"')
			|	'0'..'7' ('0'..'7' ('0'..'7')?)?
			|	'x'
				(	HEXDIGIT HEXDIGIT?
				)
			)
		;

	DOUBLE_STRING_INVALID_ESCAPE
		:	'\\' -> type(PHP_DOUBLE_STRING_LITERAL)
		;

	CONTINUE_DOUBLE_STRING
		:	(	~('\r' | '\n' | '"' | '\\' | '$' | '-' | '[' | ']' | '{' | '}' | ' ' | '\t')
			|	'-' {_input.La(1) != '>'}?
			|	'{' {_input.La(1) != '\$'}?
			|	'}' {StringBraceLevel == 0}?
			)+
			-> type(PHP_DOUBLE_STRING_LITERAL)
		;

	END_DOUBLE_STRING
		:	'"' -> type(PHP_DOUBLE_STRING_LITERAL), popMode
		;
