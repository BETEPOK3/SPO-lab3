using System;
using parser;
using lexer;

namespace СПО_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            Console.Write("Input: ");
            string strIn = Console.ReadLine();
            while (strIn.Length > 0)
            {
                try
                {
                    Console.WriteLine(parser.parse(Lexer.lex(strIn)));
                }
                catch (ApplicationException exc)
                {
                    Console.WriteLine(exc.Message);
                }
                Console.WriteLine();
                Console.Write("Input: ");
                strIn = Console.ReadLine();
            };
        }
    }
}
