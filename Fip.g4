grammar Fip;

file                : (commandline)* EOF;

commandline         :  command (NEWLINE | EOF);

command             : assignment                                
                    | print                                                    
                    | update;             

print               : PRINT (expression CONCAT?)+ SEMICOLON;

update              : UPDATE expression ASSIGN expression SEMICOLON;

assignment          : SET VALUETYPE IDENTIFIER ASSIGN expression SEMICOLON;
                    
expression          : '(' expression ')'                        #parenthesisExp
                    | expression (ASTERISK|SLASH) expression    #mulDivExp
                    | expression (PLUS|MINUS) expression        #addSubExp
                    | STRING                                    #stringAtomExp
                    | IDENTIFIER                                #identifierAtomExp
                    | REFERENCE                                 #referenceAtomExp
                    | NUMBER                                    #numericAtomExp
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
VALUETYPE           : 'int' | 'float' | 'string' ;
SET                 : 'set' ;
PRINT               : 'print' ;
UPDATE              : 'update' ;
MEM                 : 'mem' ;
FREEMEM             : 'freemem' ;
STRING              : '"' .*? '"' ;
IDENTIFIER          : LETTER (LETTER | DIGIT)* ;
REFERENCE           : AT LETTER (LETTER | DIGIT)* ;
NUMBER              : DIGIT+ ('.' DIGIT+)? ;
NEWLINE             : [\r\n]+;
WHITESPACE          : ' ' -> skip;