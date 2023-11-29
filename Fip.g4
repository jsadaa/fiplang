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
                    | expression (ASTERISK|SLASH) expression    #mulDivExp
                    | expression (PLUS|MINUS) expression        #addSubExp
                    | STRING                                    #stringAtomExp
                    | IDENTIFIER                                #identifierAtomExp
                    | REFERENCE                                 #referenceAtomExp
                    | DOUBLE                                    #doubleAtomExp
                    | INTEGER                                   #integerAtomExp
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
ASSIGN              : 'to' ;
VALUETYPE           : 'int' | 'double' | 'string' ;
SET                 : 'set' ;
PRINT               : 'print' ;
UPDATE              : 'update' ;
MEM                 : 'mem' ;
FREEMEM             : 'freemem' ;
STRING              : '"' .*? '"' ;
IDENTIFIER          : LETTER (LETTER | DIGIT)* ;
REFERENCE           : AT LETTER (LETTER | DIGIT)* ;
DOUBLE              : DIGIT+ '.' DIGIT+;
INTEGER             : DIGIT+;
NEWLINE             : [\r\n]+;
WHITESPACE          : ' ' -> skip;