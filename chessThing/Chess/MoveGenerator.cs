using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class MoveGenerator
    {
        public static void getWhitePawnMoves(ChessBoard board, bool isBlackMove)
        {
            int turn = Convert.ToInt32(isBlackMove);

            ulong blankSpaces = board.getBlanks();

            ulong posibleMoves = board.getPawns(turn) << 8 & blankSpaces;

            var t = Convert.ToString((Int64)Convert.ToUInt64(posibleMoves), 2).PadLeft(64,'0');
        }
    }
}
