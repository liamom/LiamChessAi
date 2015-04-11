using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chessThing
{
    class Program
    {
        static void Main(string[] args)
        {
            Board b = new ChessBoard();

            b.drawBoard();

            ConsoleKey key;

            do
            {
                while (!Console.KeyAvailable)
                {
                    // Do something
                }

                key = Console.ReadKey(true).Key;

                b.moveCursor(key);


            } while (key != ConsoleKey.Escape);

            Console.ReadLine();
        }
    }
}
