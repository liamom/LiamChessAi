using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

using chessThing;

namespace Chess
{
    class ChessBoard : Board
    {
        public ChessBoard()
        {
            Peices = new ulong[2][];

            #region peices
            string [] peicesStr = new string[6];
            peicesStr[0] =
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "11111111"+
                    "00000000";
            peicesStr[1] = 
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "10000001";
            peicesStr[2] = 
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00100100";
            peicesStr[3] = 
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "01000010";
            peicesStr[4] = 
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00001000";
            peicesStr[5] = 
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00000000"+
                    "00010000";
            #endregion

            for (int i = 0; i < 2; i++)
            {
                Peices[i] = new ulong[6];

                for (int j = 0; j < 6; j++)
                {
                    Peices[i][j] = Convert.ToUInt64(peicesStr[j], 2);

                    var t = peicesStr[j].ToCharArray();
                    Array.Reverse(t);
                    peicesStr[j] = new string(t);
                }
            }
        }

        #region peice getters

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getPawns(int team)
        {
            return Peices[team][0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getRooks(int team)
        {
            return Peices[team][1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getBishops(int team)
        {
            return Peices[team][2];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getKnights(int team)
        {
            return Peices[team][3];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getQueen(int team)
        {
            return Peices[team][4];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getKing(int team)
        {
            return Peices[team][5];
        }

        #endregion

        public ulong getBlanks()
        {
            ulong blanks = 0;

            foreach (ulong[] team in Peices)
            {
                foreach (ulong peice in team)
                {
                    blanks &= peice;
                }
            }

            return ~blanks;
        }

        public void isPosibleMove(int startX, int startY, int desX, int desY)
        {

        }

        public override List<Move> GetPosibleMoves(int x, int y)
        {

            return null;
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

            char[] bits = { 'p', 'r', 'b', 'n', 'q', 'k', 'P', 'R', 'B', 'N', 'Q', 'K' };

            foreach (char p in bits)
            {
                int rowCounter = 0;
                string board = Convert.ToString((Int64)Convert.ToUInt64(Peices[tCounter][pCounter]), 2).PadLeft(64, '0');
                foreach (char c in board)
                {
                    if (c == '1')
                        view[rowCounter / 8][rowCounter % 8] = p;
                    rowCounter++;
                }

                pCounter++;

                if (p == 'k')
                {
                    tCounter++;
                    pCounter = 0;
                }
            }

            return view;
        }

        #region view

        

        #endregion
    }
}
