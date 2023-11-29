grammar Fip;

file                : (commandline)* EOF;

commandline         :  command (NEWLINE | EOF);

command             : assignment                                
                    | print                                                    
                    | update
                    | mem
                    | freemem;  
                    
mem                 : MEM REFERENCE? SEMICOLON;     

freemem             : FREEMEM REFERENCE? SEMICOLON;     

print               : PRINT (expression CONCAT?)+ SEMICOLON;

update              : UPDATE expression ASSIGN expression SEMICOLON;

assignment          : SET VALUETYPE IDENTIFIER ASSIGN expression SEMICOLON;
                    
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
ASSIGN              : '=' ;
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
NEWLINE             : [\r\n]+;
WHITESPACE          : ' ' -> skip;