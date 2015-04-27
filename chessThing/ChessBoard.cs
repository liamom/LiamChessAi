using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices; 

namespace chessThing
{
    class ChessBoard : Board
    {
        public ChessBoard()
        {
            peices = new ulong[2][];


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
                peices[i] = new ulong[6];

                for (int j = 0; j < 6; j++)
                {
                    peices[i][j] = Convert.ToUInt64(peicesStr[j], 2);

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
            return peices[team][0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getRooks(int team)
        {
            return peices[team][1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getBishops(int team)
        {
            return peices[team][2];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getKnights(int team)
        {
            return peices[team][3];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getQueen(int team)
        {
            return peices[team][4];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getKing(int team)
        {
            return peices[team][5];
        }

        #endregion

        public ulong getBlanks()
        {
            ulong blanks = 0;

            foreach (ulong[] team in peices)
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

        public override List<Move> getPosibleMoves(int x, int y)
        {

            return null;
        }
    }
}
