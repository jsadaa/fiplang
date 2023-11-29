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
            try
            {
                // read the file
                string input = File.ReadAllText("./test.fip");

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

                // visit each commandline and get the data
                foreach (var commandline in fileContext.commandline())
                {
                    var data = visitor.Visit(commandline);
                    
                    // if the commandline is not void, append the data to the result content
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