using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class AI
    {
        //public static void Max(CheckersBoardState board)
        //{
        //    ulong moveAblePeices = MoveGenerator.getMovablePieces(board);

        //    Stack<ulong> peices = MoveGenerator.seperateBits(moveAblePeices);
        //    while (peices.Count > 0)
        //    {
        //        ulong value = peices.Pop();
        //    }
        //}

        public static KeyValuePair<ulong,ulong>? AlphaBetaInit(CheckersBoardState node, int depth, int alpha, int beta, bool maximizingPlayer, bool isSecondMove)
        {
            //CheckersBoardState maxMove = null;
            KeyValuePair<ulong, ulong>? maxMove = null;

            int v;
            if (maximizingPlayer)
            {
                v = int.MinValue;

                //for each child of node
                Stack<ulong> moveAblePeices = MoveGenerator.getMovablePieces(node);
                while (moveAblePeices.Count > 0)
                {
                    ulong moveAblePeice = isSecondMove ? node.LastMove : moveAblePeices.Pop();
                    Stack<ulong> moveAblePositions = isSecondMove ? MoveGenerator.canJump(node, moveAblePeice) : MoveGenerator.getMoves(node, moveAblePeice);

                    while (moveAblePositions.Count > 0)
                    {
                        ulong position = moveAblePositions.Pop();
                        //create copy
                        CheckersBoardState newNode = new CheckersBoardState(node);
                        bool hasCaptured = newNode.MovePeice(moveAblePeice, position);
                        if(!hasCaptured || MoveGenerator.getMoves(newNode,position).Count == 0)
                            newNode.EndTurn();

                        int v2 = AlphaBeta(newNode, depth - 1, alpha, beta, hasCaptured/*false*/, isSecondMove);

                        if (v < v2 || maxMove == null)
                            maxMove = new KeyValuePair<ulong, ulong>(moveAblePeice,position);

                        v = Math.Max(v, v2);
                        alpha = Math.Max(alpha, v);

                        if (beta <= alpha)
                            break;//(* β cut-off *)
                    }

                    if (isSecondMove) break;
                }
            }
            else
            {
                v = int.MaxValue;
                //for each child of node
                Stack<ulong> moveAblePeices = MoveGenerator.getMovablePieces(node);
                while (moveAblePeices.Count > 0)
                {
                    ulong moveAblePeice = isSecondMove ? node.LastMove : moveAblePeices.Pop();
                    Stack<ulong> moveAblePositions = isSecondMove ? MoveGenerator.canJump(node, moveAblePeice) : MoveGenerator.getMoves(node, moveAblePeice);

                    while (moveAblePositions.Count > 0)
                    {
                        ulong position = moveAblePositions.Pop();
                        //create copy
                        CheckersBoardState newNode = new CheckersBoardState(node);
                        bool hasCaptured = newNode.MovePeice(moveAblePeice, position);
                        if (!hasCaptured || MoveGenerator.getMoves(newNode, position).Count == 0)
                            newNode.EndTurn();

                        int v2 = AlphaBeta(newNode, depth - 1, alpha, beta, !hasCaptured/*true*/, isSecondMove);
                        if (v > v2 || maxMove == null)
                            maxMove = new KeyValuePair<ulong, ulong>(moveAblePeice, position);

                        v = Math.Min(v, v2);
                        beta = Math.Min(beta, v);

                        if (beta <= alpha)
                            break;//(* α cut-off *)
                    }

                    if (isSecondMove) break;
                }
            }

            return maxMove;
        }

        public static int AlphaBeta(CheckersBoardState node, int depth, int alpha, int beta, bool maximizingPlayer, bool isSecondMove)
        {
            if (depth == 0 || node.HasWon())
                return node.GetHeuristic();

            int v;
            if (maximizingPlayer)
            {
                v = int.MinValue;

                //for each child of node
                Stack<ulong> moveAblePeices = MoveGenerator.getMovablePieces(node);
                while (moveAblePeices.Count > 0)
                {
                    ulong moveAblePeice = isSecondMove ? node.LastMove : moveAblePeices.Pop();
                    Stack<ulong> moveAblePositions = isSecondMove ? MoveGenerator.canJump(node, moveAblePeice) : MoveGenerator.getMoves(node, moveAblePeice);

                    while (moveAblePositions.Count > 0)
                    {
                        ulong position = moveAblePositions.Pop();
                        //create copy
                        CheckersBoardState newNode = new CheckersBoardState(node);
                        bool hasCaptured = newNode.MovePeice(moveAblePeice, position);
                        if (!hasCaptured || MoveGenerator.getMoves(newNode, position).Count == 0)
                            newNode.EndTurn();

                        v = Math.Max(v, AlphaBeta(newNode, depth - 1, alpha, beta, hasCaptured /*false*/, isSecondMove));
                        alpha = Math.Max(alpha, v);

                        if (beta <= alpha)
                            break;//(* β cut-off *)
                    }

                    if (isSecondMove) break;
                }
            }
            else
            {
                v = int.MaxValue;
                //for each child of node
                Stack<ulong> moveAblePeices = MoveGenerator.getMovablePieces(node);
                while (moveAblePeices.Count > 0)
                {
                    ulong moveAblePeice = isSecondMove ? node.LastMove : moveAblePeices.Pop();
                    Stack<ulong> moveAblePositions = isSecondMove ? MoveGenerator.canJump(node, moveAblePeice) : MoveGenerator.getMoves(node, moveAblePeice);

                    while (moveAblePositions.Count > 0)
                    {
                        ulong position = moveAblePositions.Pop();
                        //create copy
                        CheckersBoardState newNode = new CheckersBoardState(node);
                        bool hasCaptured = newNode.MovePeice(moveAblePeice, position);
                        if (!hasCaptured || MoveGenerator.getMoves(newNode, position).Count == 0)
                            newNode.EndTurn();

                        v = Math.Min(v, AlphaBeta(newNode, depth - 1, alpha, beta, !hasCaptured/*true*/, isSecondMove));
                        beta = Math.Min(beta, v);
                        if (beta <= alpha)
                            break;//(* α cut-off *)
                    }

                    if (isSecondMove) break;
                }
            }

            return v;
        }
    }
}
