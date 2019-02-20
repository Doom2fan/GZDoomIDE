grammar ZScript;

/*
 * Parser Rules
 */

compileUnit
	:   'version' StringLiteral (translationUnit|EOF)
    |   translationUnit EOF
	|	EOF
	;

// ----- Optional bits -----
optSemicolon : (';'|) ;
optComma     : (','|) ;
optExpr      : (expr|) ;

// ----- Global scope -----
translationUnit : translationUnit externalDeclaration
				| externalDeclaration
				;

externalDeclaration
	: classDef
	/*| structDef
	| enumDef
	| constDef*/
	| includeDef
	;

includeDef : '#include' stringConstant ;

// ----- Dottable Identifier -----
// This can be either a single identifier or two identifiers connected by a '.'
dottableId
	: Identifier
	| dottableId '.' Identifier
//	| dottableId '.' 'default'
//	| dottableId '.' 'color'
	;

// ----- Class Definition -----
// Can only occur at global scope.
classDef : classHead classBody ;
  
classHead
	: 'class' Identifier classAncestry? classFlags
	| 'extend' 'class' Identifier
	;

classAncestry : ':' dottableId ;

classFlags
	: classFlags 'abstract'
	| classFlags 'native'
	| classFlags 'ui'
	| classFlags 'play'
	| classFlags 'replaces' dottableId
	| classFlags 'version' '(' StringLiteral ')'
	|
	;

// ----- Class Body -----
// Body is a list of:
//  * variable definitions
//  * function definitions
//  * enum definitions
//  * struct definitions
//  * state definitions
//  * constants
//  * defaults
classBody : '{' classInnards '}'
		  | ';' classInnards EOF
		  ;

classInnards : classInnards classMember
			 |
			 ;

classMember : declarator
			| enumDef
			| structDef
			| statesDef
			| defaultDef
			| constDef
			| propertyDef
			| staticArrayStatement
			;

propertyDef : 'property' Identifier ':' identifierList ';' ;

identifierList : Identifier
			   | statesOpt ',' Identifier
			   ;

// ----- Struct Definition -----
// Structs can define variables and enums.
structDef : 'struct' Identifier structFlags '{' structBody? '}' optSemicolon ;

structFlags
	: structFlags 'ui'
	| structFlags 'play'
	| structFlags 'clearscope'
	| structFlags 'native'
	| structFlags 'version' '(' StringLiteral ')'
	|
	;

structBody : structBody structMember
		   | structMember
		   ;

structMember : declarator
			 | enumDef
			 | constDef
			 ;

// ----- Constant Definition -----
// Like UnrealScript, a constant's type is implied by its value's type.
constDef : 'const' Identifier '=' expr ';' ;

// ----- Enum Definition -----
// Enumerators are lists of named integers.
enumDef : 'enum' Identifier enumType? '{' enumList '}' optSemicolon ;

enumType : ':' intType ;

enumList : enumList ',' enumerator
		 | enumerator
		 ;

enumerator : Identifier '=' expr
		   | Identifier
		   ;

// ----- States -----
statesDef : 'states' statesOpt? '{' statesBody '}' ;

statesOpts : '(' statesOpt ')' ;

statesOpt : statesOpt ',' Identifier
		  | Identifier
		  ;

statesBody : /*statesBody stateLine
		   |*/ statesBody stateFlow
		   | statesBody stateLabel
		   |
		   ;

// DO STATES
//stateLine : expr stateOpts stateAction ;
stateOpts : stateOpts 'bright'
		  | stateOpts 'fast'
		  | stateOpts 'slow'
		  | stateOpts 'nodelay'
		  | stateOpts 'canraise'
		  | stateOpts 'offset' '(' expr ',' expr ')'
		  | stateOpts 'light' '(' lightList ')'
		  |
		  ;

lightList : lightList ',' StringLiteral
		  | StringLiteral
		  ;

stateFlow : stateFlowType ';' ;

stateFlowType : 'stop'
			  | 'wait'
			  | 'fail'
			  | 'loop'
			  | 'goto' dottableId stateGotoOffset?
			  | 'goto' Identifier '::' dottableId stateGotoOffset?
			  | 'goto' 'super' '::' dottableId stateGotoOffset?
			  ;

stateGotoOffset : '+' expr ;

stateLabel : Identifier ':' ;

// A state action can be either a compound statement or a single action function call.
stateAction : stateCall? ';'
			| '{' statementList '}'
			| '{' '}'
			;

stateCall : Identifier stateCallParams? ;

stateCallParams : '(' funcExprList ')' ;

// Definition of a default class instance.
defaultDef : 'default' '{' defaultStatementList? '}' ;

defaultStatementList : defaultStatementList defaultStatement
					 | defaultStatement
					 ;

defaultStatement : assignStatement ';'
				 | propertyStatement
				 | flagStatement
				 | ';'
				 ;

propertyStatement : dottableId exprList ';'
				  | dottableId ';'
				  ;

flagStatement : '+' dottableId
			  | '-' dottableId
			  ;

// ----- Type Names -----
intType : 'int'
		| 'uint'
		| 'short'
		| 'ushort'
		| 'byte'
		| 'ubyte'
		;

typeName1 : 'bool'
		  | intType
		  | 'float'
		  | 'double'
		  | 'Vector2'
		  | 'Vector3'
		  | 'let'
		  ;

typeName : typeName1
		 | Identifier
		 | '@' Identifier
		 | 'readonly' '<' Identifier '>'
		 | 'readonly' '<' '@' Identifier '>'
		 | '.' dottableId
		 ;

aggregateType : 'map' '<' typeOrArray ',' typeOrArray '>' // Hash table. !!!NOT IMPLEMENTED!!!
			  | 'array' '<' typeOrArray '>'
			  | 'class' classRestrictor?
			  ;

classRestrictor : '<' dottableId '>' ;

type : typeName
	 | aggregateType
	 ;

typeOrArray : type
			| type arraySize
			;

typeList : typeList ',' typeOrArray
		 | typeOrArray
		 ;

typeListOrVoid : typeList
			   | 'void'
			   ;

arraySizeExpr : '[' optExpr ']' ;

arraySize : arraySize arraySizeExpr
		  | arraySizeExpr
		  ;

// Multiple type names are only valid for functions.
declarator : declFlags typeListOrVoid variablesOrFunction ;

variablesOrFunction : variableDef
					| funcDef
					| ';'
					;

// ----- Variable Names -----
variableDef : variableList ';' ;

variableName : Identifier
			 | Identifier arraySize
			 ;

variableList : variableList ',' variableName
			 | variableName
			 ;

declFlags
	: declFlags declFlag
	| declFlags 'action' statesOpts
	| declFlags 'deprecated' '(' StringLiteral ')'
	| declFlags 'version' '(' StringLiteral ')'
	|
	;

declFlag
     : 'native'
     | 'static'
     | 'private'
     | 'protected'
     | 'latent'
     | 'final'
     | 'meta'
     | 'transient'
     | 'readonly'
     | 'virtual'
     | 'override'
     | 'vararg'
     | 'ui'
     | 'play'
     | 'clearscope'
     | 'virtualscope'
	 ;

// ----- Function Definition -----
funcDef : Identifier '(' funcParams ')' 'const'? functionBody? ;

funcParams : funcParamList
		   | funcParamList ',' '...'
		   | 'void'
		   |
		   ;

funcParamList : funcParamList ',' funcParam
			  | funcParam
			  ;

funcParam : funcParamFlags type Identifier
		  | funcParamFlags type Identifier '=' expr
		  ;

funcParamFlags : funcParamFlags 'in'
			   | funcParamFlags 'out'
			   | funcParamFlags 'optional'
			   |
			   ;

primary : primary '++' // Postfix++
		| primary '--' // Postfix--
		| primary '.' Identifier // Member access
		| primary '(' funcExprList ')' // Function call
		| primary '[' expr ']' // Array access
		| '(' 'class' '<' Identifier '>' ')' '(' funcExprList ')' // Class type cast
		| '(' expr ',' expr ',' expr ')' // Vector3 intializer
		| '(' expr ',' expr ')' // Vector2 initializer
		| '(' expr ')'
		| '(' ')'
		| Identifier
		| 'super'
		| constant
		| typeName1
		;

// ----- Binary Expressions -----
expr : condExpr '=' expr
	 | condExpr '+=' expr
	 | condExpr '-=' expr
	 | condExpr '*=' expr
	 | condExpr '/=' expr
	 | condExpr '%=' expr
	 | condExpr '<<=' expr
	 | condExpr '>>=' expr
	 | condExpr '>>>=' expr
	 | condExpr '&=' expr
	 | condExpr '|=' expr
	 | condExpr '^=' expr
	 | condExpr
	 ;

condExpr : orExpr '?' orExpr ':' condExpr
		 | orExpr
		 ;

orExpr : orExpr '||' andExpr
	   | andExpr
	   ;

andExpr : andExpr '&&' equalExpr
		| equalExpr
	    ;

equalExpr : equalExpr '==' compExpr
		  | equalExpr '!=' compExpr
		  | equalExpr '~==' compExpr
		  | compExpr
	      ;

compExpr : compExpr '<' concatExpr
		 | compExpr '>' concatExpr
		 | compExpr '<=' concatExpr
		 | compExpr '>=' concatExpr
		 | compExpr '<>=' concatExpr
		 | compExpr 'is' concatExpr
		 | concatExpr
		 ;

concatExpr : concatExpr '..' bitOrExpr
		   | bitOrExpr
		   ;

bitOrExpr : bitOrExpr '|' xorExpr
		  | xorExpr
		  ;

xorExpr : xorExpr '^' bitAndExpr
		| bitAndExpr
		;

bitAndExpr : bitAndExpr '&' bitshiftExpr
		   | bitshiftExpr
		   ;

bitshiftExpr : bitshiftExpr '<<' subAddExpr
		     | bitshiftExpr '>>' subAddExpr
		     | bitshiftExpr '>>>' subAddExpr
		     | subAddExpr
             ;

subAddExpr : subAddExpr '-' multExpr
		   | subAddExpr '+' multExpr
		   | multExpr
           ;

multExpr : multExpr '*' powExpr
		 | multExpr '/' powExpr
		 | multExpr '%' powExpr
		 | multExpr 'cross' powExpr
		 | multExpr 'dot' powExpr
		 | powExpr
         ;

powExpr : powExpr '**' unaryExpr
		| unaryExpr
        ;

// ----- Unary Expressions -----

unaryExpr : '++' unaryExpr // ++Prefix
		  | '--' unaryExpr // --Prefix
		  | '-' unaryExpr
		  | '+' unaryExpr
		  | '~' unaryExpr // Bitwise negation
		  | '!' unaryExpr // Negation
		  | 'sizeof' unaryExpr
		  | 'alignof' unaryExpr
		  | primary
		  ;

exprList : exprList ',' expr
		 | expr
		 ;

funcExprList : funcExprList ',' funcExprItem
			 | funcExprItem
			 ;

funcExprItem : namedExpr
			 |
			 ;

namedExpr : Identifier ':' expr
		  | expr
		  ;

// ----- Constants -----
// Allow C-like concatenation of adjacent string constants.
stringConstant : stringConstant StringLiteral
			   | StringLiteral
			   ;

constant : stringConstant
		 | IntLiteral
		 | FloatLiteral
		 | NameLiteral
		 | 'false'
		 | 'true'
		 | 'nullptr'
		 ;

// ----- Statements -----
functionBody : compoundStatement ;

statement : labeledStatement
		  | compoundStatement
		  | expressionStatement ';'
		  | selectionStatement
		  | iterationStatement
		  | jumpStatement
		  | assignStatement ';'
		  | localVar ';'
		  | staticArrayStatement
		  | ';'
		  ;

// ----- Static Array Statements -----
staticArrayStatement : 'static const' type Identifier '[' ']' '=' '{' exprList '}' ';'
                     | 'static const' type '[' ']' Identifier '=' '{' exprList '}' ';' ;

// ----- Jump Statements -----
jumpStatement : 'continue' ';'
			  | 'break' ';'
			  | 'return' ';'
			  | 'return' exprList ';'
			  ;

// ----- Compound Statements -----
compoundStatement : '{' '}'
				  | '{' statementList '}'
				  ;

statementList : statementList statement
			  | statement
			  ;

// ----- Expression Statements -----
expressionStatement : expr ;

// ----- Iteration Statements -----
iterationStatement : whileOrUntil '(' expr ')' statement
				   | 'do' statement whileOrUntil '(' expr ')'
				   | 'for' '(' forInit ';' optExpr ';' forBump ')' statement
				   ;

whileOrUntil : 'while'
             | 'until'
             ;

forInit : localVar
        | forBump
        ;

forBump : forBump ',' expressionStatement
		| expressionStatement
		;

// ----- If Statements -----
selectionStatement : 'if' '(' expr ')' statement
				   | 'if' '(' expr ')' 'else' statement
				   | switchStatement
				   ;

// ----- Switch Statements -----
switchStatement : 'switch' '(' expr ')' statement ;

// ----- Case label Statements -----
labeledStatement : 'case' expr ':'
				 | 'default' ':'
				 ;

// ----- Assignment Statements -----
assignStatement : '[' exprList ']' '=' expr ;

// ----- Local Variable Definition "Statements" -----
localVar : type variableListWithInit ;

varInit : Identifier
		| Identifier '=' expr
		| Identifier '=' '{' exprList '}'
		| Identifier '=' '{' '}'
		| Identifier arraySize
		| Identifier arraySize '=' '{' exprList '}'
		;

variableListWithInit : variableListWithInit ',' varInit
					 | varInit
					 ;

// ----- ??? -----
/*
 * Lexer Rules
 */

fragment LETTER       : [a-zA-Z]    ;
fragment ALPHANUMERIC : [a-zA-Z0-9] ;
fragment DIGIT        : [0-9]       ;
fragment HEX_DIGIT    : [0-9a-fA-F] ;

fragment ID_HEAD : (LETTER|'_') ;
fragment ID_TAIL : (ALPHANUMERIC|'_') ;

Identifier : ID_HEAD ID_TAIL+ ;

IntLiteral     : (DecLiteral|HexLiteral)      ;
FloatLiteral   : (RealLiteral|RealLiteralAlt) ;

DecLiteral : DIGIT+            [Uu]? ;
HexLiteral : '0'[xX]HEX_DIGIT+ [Uu]? ;

RealLiteral    : DIGIT*'.'DIGIT+ ;
RealLiteralAlt : '.'DIGIT+       ;

StringLiteral : '"' .*? '"' ;
NameLiteral   : '\'' .*? '\'' ;

WS
	:	' ' -> channel(HIDDEN)
	;
