using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Checkers;

namespace chessThing
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckersBoard b = new CheckersBoard(true);

            b.DrawBoard();

            //b.Highlighted = MoveGenerator.getMoves((CheckersBoard)b, false);

            //b.State.IsBlackTurn = true;
            //var movablePieces = MoveGenerator.getMovablePieces(b.State);

            //b.Highlighted = MoveGenerator.combineBits(movablePieces);

            //var test = AI.AlphaBetaInit(b.State, 8, int.MinValue, int.MaxValue, true);

            ConsoleKey key;

            do
            {
                while (!Console.KeyAvailable)
                {
                    // Do something
                }

                key = Console.ReadKey(true).Key;

                b.MoveCursor(key);


            } while (key != ConsoleKey.Escape);

            Console.ReadLine();
        }
    }
}
