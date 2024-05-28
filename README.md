# Fiplang

Fiplang is a really basic and simple programming language that I am creating for learning and fun. 
It is a dynamically typed language that is interpreted.
The language interpreter is written in C# using the Antlr4 library for parsing.

The grammar for the language is defined in the Fip.g4 file. The language is still in its very early development and is not yet complete (and may never be).

## Installation for development

### Prerequisites

- .NET 7.0 SDK (https://dotnet.microsoft.com/download/dotnet/7.0)
- Antlr4 (https://www.antlr.org/)
- Antlr4.Runtime.Standard (https://www.nuget.org/packages/Antlr4.Runtime.Standard)

### Steps

1. Clone the repository
2. Install the .NET 7.0 SDK
3. Install the Antlr4 tool
4. Install the Antlr4.Runtime.Standard package
5. Implement the grammar in the Fip.g4 file
6. Generate the lexer and parser using the following command:
```shell
java -jar antlr-4.9.2-complete.jar -Dlanguage=CSharp -visitor -o FipLang/Generated FipLang/Fip.g4
```
7. Implement the interpreter in the CustomFipVisitor.cs file
8. Run the interpreter
9. Have fun!

## Concepts

The language has the following concepts:

- Variables (int, double, string, bool) 
- Arithmetic operations (+, -, *, /)
- Comparison operations (==, !=, <, >, <=, >=)
- Utilities commands (print, mem, freemem)
- Assignment commands (set, mod)

It can run in two modes:

- Interactive mode: where you can write commands and see the output immediately
- File mode: where you can run a file with Fip code and see the output

## Example

Here is an example of a simple program written in Fip:

```shell
print @res;
mod @res = false;
print @res;
set int num = 1;
print @num;
set double num2 = 1.2 + 2.0;
print @num2;
mod @num2 = 2.2;
print @num2;
mem;
freemem;
mem;
```

which will output:

```
hello world
true
false
1
3.2
2.2
bool[5]: @res = false
int[1]: @num = 1
double[2]: @num2 = 2.2
```

## Usage

To run the interpreter, you can use the following command in the root directory of the project:

```shell
dotnet run
```

or

```shell
dotnet build
```

then

```shell
./bin/Debug/net7.0/FipLang
```

This will start the interpreter in interactive mode. 

```
Fip 0.0.0.0.1 - Fip Interactive Prompt
Usage: fip [filename]
Press Ctrl+C to exit

fip> 
```

You can also run a file with Fip code using the following command:

```shell
dotnet run filename.fip
```

or

```shell
dotnet build
```

then

```shell
./bin/Debug/net7.0/FipLang filename.fip
```

## Commands

The following commands are available in the language:

- `print`: print an expression, a variable or a string
- `set`: set a variable with a value
- `mod`: modify a variable with a new value
- `mem`: show the defined variables in memory
- `freemem`: free the memory of all the variables

## Types

The language supports the following types:

- `int`: integer numbers
- `double`: floating point numbers
- `string`: text
- `bool`: boolean values

For now, a variable can be redefined with a different type, but not updated with a different one.

## Author

Léo Paillard