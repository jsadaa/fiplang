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
            // Initialize data repository to store variables
            Repository dataRepository = new Repository();
            string? input;

            // If no arguments are passed, start interactive mode
            if (args.Length == 0)
            {
                Console.WriteLine("Fip 1.0.0");
                Console.WriteLine("Usage: fip [filename]");
                Console.WriteLine("Press Ctrl+C to exit");
                Console.WriteLine();
                
                while (true)
                {
                    Console.Write("fip> ");
                    input = Console.ReadLine();
                    if (input == null) continue;
                    InterpretAndExecute(input, dataRepository);
                }
            }
            
            // If a filename is passed, execute the file
            input = File.ReadAllText(args[0]);
            InterpretAndExecute(input, dataRepository);
          
        }

        private static void InterpretAndExecute(string input, Repository dataRepository)
        {
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
            }
        }
    }
}
