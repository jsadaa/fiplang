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
                string input = File.ReadAllText("./test.fip");

                ICharStream inputStream = CharStreams.fromString(input);
                FipLexer fipLexer = new FipLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(fipLexer);
                FipParser fipParser = new FipParser(commonTokenStream);

                FipParser.FileContext fileContext = fipParser.file();
                Repository dataRepository = new Repository();
                CustomFipVisitor visitor = new CustomFipVisitor(dataRepository);

                var resultContent = new StringBuilder();

                foreach (var commandline in fileContext.commandline())
                {
                    var data = visitor.Visit(commandline);
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