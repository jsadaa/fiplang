using System.Text;
using Antlr4.Runtime;
using FipLang.Data;
using FipLang.Type;

namespace FipLang
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Fip 1.0.0");
                // usage is fip with an optional filename argument
                Console.WriteLine("Usage: fip [filename]");
                Console.WriteLine("Press Ctrl+C to exit");
                Console.WriteLine();
                
                // create the repository to store the data
                Repository dataRepository = new Repository();
                
                while (true)
                {
                    Console.Write("fip> ");
                    string? input = Console.ReadLine();
                
                    try
                    {
                        // create the lexer, parser
                        ICharStream inputStream = CharStreams.fromString(input);
                        FipLexer fipLexer = new FipLexer(inputStream);
                        CommonTokenStream commonTokenStream = new CommonTokenStream(fipLexer);
                        FipParser fipParser = new FipParser(commonTokenStream);
                        

                        // parse the file and create the visitor
                        FipParser.FileContext fileContext = fipParser.file();
                        CustomFipVisitor visitor = new CustomFipVisitor(dataRepository);
                
                        var resultContent = new StringBuilder();

                        // visit each context
                        foreach (var commandline in fileContext.commandline())
                        {
                            var data = visitor.Visit(commandline);
                    
                            // if the commandline data return is not void, append the data to the result content
                            if (data.Type != Integrated.Void)
                                resultContent.AppendLine(data.Value.ToString());
                        }

                        Console.Write(resultContent);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }
            }
            
            try
            {
                // read the file
                string input = File.ReadAllText(args[0]);

                // create the lexer, parser
                ICharStream inputStream = CharStreams.fromString(input);
                FipLexer fipLexer = new FipLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(fipLexer);
                FipParser fipParser = new FipParser(commonTokenStream);
                
                // create the repository to store the data
                Repository dataRepository = new Repository();

                // parse the file and create the visitor
                FipParser.FileContext fileContext = fipParser.file();
                CustomFipVisitor visitor = new CustomFipVisitor(dataRepository);
                
                var resultContent = new StringBuilder();

                // visit each context
                foreach (var commandline in fileContext.commandline())
                {
                    var data = visitor.Visit(commandline);
                    
                    // if the commandline data return is not void, append the data to the result content
                    if (data.Type != Integrated.Void)
                        resultContent.AppendLine(data.Value.ToString());
                }

                Console.WriteLine(resultContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}