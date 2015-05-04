using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using chessThing;

namespace Checkers
{
    class CheckersBoard : Board
    {

        public CheckersBoard(bool testing = false)
        {
            State = new CheckersBoardState(testing);
        }

        public void IsPosibleMove(int startX, int startY, int desX, int desY)
        {

        }

        public override ulong GetPosibleMoves(int x, int y)
        {
            y = Math.Abs(y - 7);
            x = Math.Abs(x - 7);

            int num = y * 8 + x;

            ulong peice = Convert.ToUInt64(1UL << num);

            return MoveGenerator.combineBits(MoveGenerator.getMoves(State, peice));
        }

        protected override char[][] GenerateBoardView()
        {
            char[][] view = new char[8][];

            for (int i = 0; i < 8; i++)
            {
                view[i] = new char[8];
            }

            int tCounter = 0;
            int pCounter = 0;

            char[] bits = { 'r','R','b','B' };

            foreach (char p in bits)
            {
                int rowCounter = 0;
                string board = Convert.ToString((Int64)Convert.ToUInt64(State[tCounter][pCounter]), 2).PadLeft(64, '0');
                foreach (char c in board)
                {
                    if (c == '1')
                        view[rowCounter / 8][rowCounter % 8] = p;
                    rowCounter++;
                }

                pCounter++;

                if (p == 'R')
                {
                    tCounter++;
                    pCounter = 0;
                }
            }

            return view;
        }
    }
}
