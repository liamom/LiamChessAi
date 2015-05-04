using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers
{
    class MoveGenerator
    {
        public static readonly ulong blackSpots = 12273903644374837845;

        public static Stack<ulong> getMovablePieces(CheckersBoardState state)
        {
                ulong posibleMovesLeft;
                ulong rightJumps;
                ulong posibleMovesRight;
                ulong leftJumps;

                ulong dPosibleMovesLeft;
                ulong dRightJumps;
                ulong dPosibleMovesRight;
                ulong dLeftJumps;

                GetMovesUlong(state, out posibleMovesLeft, out rightJumps, out posibleMovesRight, out leftJumps, state.IsBlackTurn,
                    out dPosibleMovesLeft, out dRightJumps, out dPosibleMovesRight, out dLeftJumps);

                if (!state.IsBlackTurn)
                {
                    rightJumps <<= 14;
                    leftJumps <<= 18;
                    posibleMovesLeft <<= 7;
                    posibleMovesRight <<= 9;

                    
                    dRightJumps >>= 14;
                    dLeftJumps >>= 18;
                    dPosibleMovesLeft >>= 7;
                    dPosibleMovesRight >>= 9;
                }
                else
                {
                    rightJumps >>= 14;
                    leftJumps >>= 18;
                    posibleMovesLeft >>= 7;
                    posibleMovesRight >>= 9;

                    dRightJumps <<= 14;
                    dLeftJumps <<= 18;
                    dPosibleMovesLeft <<= 7;
                    dPosibleMovesRight <<= 9;
                    
                }

                ulong output = (posibleMovesLeft | posibleMovesRight | leftJumps | rightJumps);
                ulong dOutput = (dPosibleMovesLeft | dPosibleMovesRight | dLeftJumps | dRightJumps);
                return seperateBits((output | dOutput) & ~blackSpots);
        }

        private static void GetMovesUlong(CheckersBoardState state, out ulong posibleMovesLeft, out ulong rightJumps, out ulong posibleMovesRight, out ulong leftJumps, bool isBlackTurn, 
            out ulong dPosibleMovesLeft, out ulong dRightJumps, out ulong dPosibleMovesRight, out ulong dLeftJumps, ulong? peiceToMove = null)
        {
            ulong blankSpaces = state.GetBlanks();
            ulong yourPeices = state.GetPeices(state.IsBlackTurn);
            ulong peicesToMove = peiceToMove == null ? (ulong)yourPeices : (ulong)peiceToMove;
            ulong enemyPeices = state.GetPeices(!state.IsBlackTurn);

            posibleMovesLeft = isBlackTurn ? peicesToMove << 7 : peicesToMove >> 7;
            posibleMovesLeft &= ~yourPeices;
            posibleMovesLeft &= ~blackSpots;
            rightJumps = GetJumps(7, blankSpaces, yourPeices, enemyPeices, posibleMovesLeft, state.IsBlackTurn);
            posibleMovesLeft &= ~enemyPeices;

            posibleMovesRight = isBlackTurn ? peicesToMove << 9 : peicesToMove >> 9;
            posibleMovesRight &= ~yourPeices;
            posibleMovesRight &= ~blackSpots;
            leftJumps = GetJumps(9, blankSpaces, yourPeices, enemyPeices, posibleMovesRight, state.IsBlackTurn);
            posibleMovesRight &= ~enemyPeices;

            ulong doublePeicesToMove = peiceToMove == null ? state.GetDoubleBits(state.IsBlackTurn) : state.GetDoubleBits(state.IsBlackTurn) & (ulong)peiceToMove;

            dPosibleMovesLeft = !isBlackTurn ? doublePeicesToMove << 7 : doublePeicesToMove >> 7;
            dPosibleMovesLeft &= ~yourPeices;
            dPosibleMovesLeft &= ~blackSpots;
            dRightJumps = GetJumps(7, blankSpaces, yourPeices, enemyPeices, dPosibleMovesLeft, !state.IsBlackTurn);
            dPosibleMovesLeft &= ~enemyPeices;

            dPosibleMovesRight = !isBlackTurn ? doublePeicesToMove << 9 : doublePeicesToMove >> 9;
            dPosibleMovesRight &= ~yourPeices;
            dPosibleMovesRight &= ~blackSpots;
            dLeftJumps = GetJumps(9, blankSpaces, yourPeices, enemyPeices, dPosibleMovesRight, !state.IsBlackTurn);
            dPosibleMovesRight &= ~enemyPeices;
        }

        private static ulong GetJumps(int amount, ulong blankSpaces, ulong yourPeices, ulong enemyPeices, ulong posibleMoves, bool isBlackTurn)
        {
            ulong jumps = posibleMoves & enemyPeices;
            jumps &= ~blackSpots;
            jumps = isBlackTurn ? jumps << amount : jumps >> amount;
            jumps &= blankSpaces;
            jumps &= ~enemyPeices;
            jumps &= ~blackSpots;

            return jumps;
        }

        public static Stack<ulong> getMoves(CheckersBoardState state, ulong bits)
        {
            ulong posibleMovesLeft;
            ulong rightJumps;
            ulong posibleMovesRight;
            ulong leftJumps;

            ulong dPosibleMovesLeft;
            ulong dRightJumps;
            ulong dPosibleMovesRight;
            ulong dLeftJumps;

            GetMovesUlong(state, out posibleMovesLeft, out rightJumps, out posibleMovesRight, out leftJumps, state.IsBlackTurn,
                out dPosibleMovesLeft, out dRightJumps, out dPosibleMovesRight, out dLeftJumps, bits);

            return seperateBits((posibleMovesLeft | posibleMovesRight | leftJumps | rightJumps | dPosibleMovesLeft | dRightJumps | dPosibleMovesRight | dLeftJumps) & ~blackSpots);
        }

        public static Stack<ulong> canJump(CheckersBoardState state, ulong bits)
        {
            ulong posibleMovesLeft;
            ulong rightJumps;
            ulong posibleMovesRight;
            ulong leftJumps;

            ulong dPosibleMovesLeft;
            ulong dRightJumps;
            ulong dPosibleMovesRight;
            ulong dLeftJumps;

            GetMovesUlong(state, out posibleMovesLeft, out rightJumps, out posibleMovesRight, out leftJumps, state.IsBlackTurn,
                out dPosibleMovesLeft, out dRightJumps, out dPosibleMovesRight, out dLeftJumps, bits);

            return seperateBits((leftJumps | rightJumps | dRightJumps | dLeftJumps) & ~blackSpots);
        }

        public static Stack<ulong> seperateBits(ulong a)
        {
            Stack<ulong> output = new Stack<ulong>();

            for (int i = 0; i < 64; i++)
            {
                ulong bit = 1UL << i;

                if((bit & a) != 0)
                {
                    output.Push(bit);
                }
            }

            return output;
        }

        public static ulong combineBits(Stack<ulong> a)
        {
            ulong output = 0;

            while (a.Count > 0)
            {
                output |= a.Pop();
            }

            return output;
        }
    }
}
