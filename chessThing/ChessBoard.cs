using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chessThing
{
    class ChessBoard : Board
    {
        public ChessBoard()
        {
            board = new char[8][];
            /*
            string back = "rnbkqbnr";
            string pawns = "pppppppp";
            string blank = "        ";
            board[0] = back.ToCharArray();
            board[1] = pawns.ToCharArray();
            board[2] = blank.ToCharArray();
            board[3] = blank.ToCharArray();
            board[4] = blank.ToCharArray();
            board[5] = blank.ToCharArray();
            board[6] = pawns.ToUpper().ToCharArray();
            board[7] = back.ToUpper().ToCharArray();
            */
            board[0] = "rp    PR".ToCharArray();
            board[1] = "np    PN".ToCharArray();
            board[2] = "bp    PB".ToCharArray();
            board[3] = "kp    PK".ToCharArray();
            board[4] = "qp    PQ".ToCharArray();
            board[5] = "bp    PB".ToCharArray();
            board[6] = "np    PN".ToCharArray();
            board[7] = "rp    PR".ToCharArray();
        }

        public void isPosibleMove(int startX, int startY, int desX, int desY)
        {

        }

        public override List<Move> getPosibleMoves(int x, int y)
        {
            var moves = new List<Move>();

            switch (board[x][y])
            {
                case 'P':
                case 'p':
                    int dirI = getLocation(x, y) == 'p' ? 1 : -1;
                    List<Tuple<int, int>> dirs = new List<Tuple<int, int>>{
                        new Tuple<int,int>(0,1 * dirI)
                    };

                    if (getLocation(x, y) == 'p' && y == 1
                        || getLocation(x, y) == 'P' && y == 6)
                    {
                        dirs.Add(new Tuple<int, int>(0, 2 * dirI));
                    }

                    var l1 = getLocation(x + 1, y + 1 * dirI);
                    if (l1 != ' ' && l1 != '!')
                        dirs.Add(new Tuple<int, int>(x + 1, y + 1 * dirI));

                    var l2 = getLocation(x - 1,y + 1 * dirI);
                    if (l2 != ' ' && l2 != '!')
                        dirs.Add(new Tuple<int, int>(x - 1, y + 1 * dirI));

                    foreach (Tuple<int, int> dir in dirs)
                    {
                        if (getLocation(x + dir.Item1, y + dir.Item2) == ' ')
                        {
                            moves.Add(new Move(x, y, x + dir.Item1, y + dir.Item2));
                        }
                    }
                    break;
                case 'r':
                case 'R':
                    moves.AddRange(rookMoves(x, y));
                    break;
                case 'b':
                case 'B':
                    moves.AddRange(bishopMoves(x, y));
                    break;
                case 'q':
                case 'Q':
                    moves.AddRange(rookMoves(x, y));
                    moves.AddRange(bishopMoves(x, y));
                    break;
                case 'n':
                case 'N':

                    break;
            }

            return moves;
        }

        private List<Move> bishopMoves(int x, int y)
        {
            Tuple<int, int>[] t = {
                new Tuple<int,int>(1,1),
                new Tuple<int,int>(-1,-1),
                new Tuple<int,int>(-1,1),
                new Tuple<int,int>(1,-1)
            };

            return sideMove(x, y, t);
        }

        private List<Move> rookMoves(int x, int y)
        {
            Tuple<int, int>[] t = {
                new Tuple<int,int>(0,1),
                new Tuple<int,int>(0,-1),
                new Tuple<int,int>(1,0),
                new Tuple<int,int>(-1,0)
            };

            return sideMove(x, y, t);
        }

        private List<Move> sideMove(int x, int y, Tuple<int, int>[] t)
        {
            var moves = new List<Move>();

            foreach (Tuple<int, int> thing in t)
            {
                int i = 1;

                while (getLocation(x * thing.Item1, y * thing.Item2) == ' ')
                {
                    moves.Add(new Move(x, y, x * thing.Item1, y * thing.Item2));
                    i++;
                }
            }

            return moves;
        }
    }
}
