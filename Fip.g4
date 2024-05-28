grammar Fip;

file                : (commandline)* EOF;

commandline         : statement                 
                    ;
                    
statement           : IF expression NEWLINE THEN NEWLINE (command)* (ELSE NEWLINE (command)*)? ENDIF  #ifStatement
                    | (command)*                                     #commandStatement
                    ;

command             : assignment NEWLINE                                
                    | print NEWLINE                                                 
                    | update NEWLINE
                    | mem NEWLINE
                    | freemem NEWLINE; 
                    
mem                 : MEM REFERENCE?;     

freemem             : FREEMEM REFERENCE?;     

print               : PRINT (expression CONCAT?)+;

update              : UPDATE expression ASSIGN expression;

assignment          : SET VALUETYPE IDENTIFIER ASSIGN expression;
                    
expression          : '(' expression ')'                        #parenthesisExp
                    | expression (EQUALS|NOTEQUALS|LESS|GREATER|LESSEQUALS|GREATEREQUALS) expression #comparisonExp
                    | expression (ASTERISK|SLASH) expression    #mulDivExp
                    | expression (PLUS|MINUS) expression        #addSubExp
                    | IDENTIFIER                                #identifierAtomExp
                    | REFERENCE                                 #referenceAtomExp
                    | STRING                                    #stringAtomExp
                    | DOUBLE                                    #doubleAtomExp
                    | INTEGER                                   #integerAtomExp
                    | BOOL                                      #boolAtomExp
                    ;
                                      
fragment LETTER     : [a-zA-Z] ;
fragment DIGIT      : [0-9] ;
fragment AT         : '@' ;

COMMENT             : '//' .*? '\r'? '\n' -> skip ;
MULTILINE_COMMENT   : '/*' .*? '*/' -> skip ;
CONCAT              : '.' ;
SEMICOLON           : ';' ;
ASTERISK            : '*' ;
SLASH               : '/' ;
PLUS                : '+' ;
MINUS               : '-' ;
EQUALS              : '==' ;
NOTEQUALS           : '!=' ;
LESS                : '<' ;
GREATER             : '>' ;
LESSEQUALS          : '<=' ;
GREATEREQUALS       : '>=' ;
IF                  : 'if' ;
THEN                : 'then' ;
ELSE                : 'else' ;
ENDIF               : 'endif' ;
ASSIGN              : 'to' ;
VALUETYPE           : 'int' | 'double' | 'string' | 'bool' ;
SET                 : 'set' ;
PRINT               : 'print' ;
UPDATE              : 'mod' ;
MEM                 : 'mem' ;
FREEMEM             : 'freemem' ;
BOOL                : 'true' | 'false' ;
STRING              : '"' .*? '"' ;
IDENTIFIER          : LETTER (LETTER | DIGIT)* ;
REFERENCE           : AT LETTER (LETTER | DIGIT)* ;
DOUBLE              : DIGIT+ '.' DIGIT+;
INTEGER             : DIGIT+;
NEWLINE             : ('\r'? '\n' | '\r')+ ;
WHITESPACE          : ' ' -> skip;