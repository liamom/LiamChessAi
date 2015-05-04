using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Checkers
{
    class CheckersBoardState
    {
        #region data

        private ulong[][] peices;
        private ulong lastMove;
        private bool isBlackTurn;
        //private int heuristic = 0;

        public ulong[][] Peices
        {
            get { return peices; }
            set { peices = value; }
        }

        public ulong LastMove
        {
            get { return lastMove; }
            set { lastMove = value; }
        }


        //public int Heuristic
        //{
        //    get { return Heuristic; }
        //}

        public bool IsBlackTurn
        {
            get { return isBlackTurn; }
            set { isBlackTurn = value; }
        }

        public void EndTurn()
        {
            IsBlackTurn = !IsBlackTurn;
        }

        public ulong[] this [int key]
        {
            get { return Peices[key]; }
            set { Peices[key] = value; }
        }

        public CheckersBoardState(CheckersBoardState board)
        {
            IsBlackTurn = board.IsBlackTurn;
            Peices = new ulong[2][];

            for (int i = 0; i < 2; i++)
            {
                Peices[i] = new ulong[2];

                for (int j = 0; j < Peices[i].Length; j++)
                {
                    Peices[i][j] = board.Peices[i][j];
                }

            }
        }

        public CheckersBoardState(bool test = false)
        {
            Peices = new ulong[2][];


            #region peices

            string[] peicesStr = test == false ? getInitPos() : getTestPos();

            #endregion

            for (int i = 0; i < 2; i++)
            {
                Peices[i] = new ulong[2];

                for (int j = 0; j < peicesStr.Length; j++)
                {
                    Peices[i][j] = Convert.ToUInt64(peicesStr[j], 2);

                    var t = peicesStr[j].ToCharArray();
                    Array.Reverse(t);
                    peicesStr[j] = new string(t);
                }
            }
        }

        private static string[] getInitPos()
        {
            string[] peicesStr = new string[2];
            peicesStr[0] =
                    "01010101" +
                    "10101010" +
                    "01010101" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000";
            peicesStr[1] =
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000";
            return peicesStr;
        }

        private static string[] getTestPos()
        {
            string[] peicesStr = new string[2];
            peicesStr[0] =
                    "01010000" +
                    "10100010" +
                    "01010000" +
                    "00100000" +
                    "01000000" +
                    "00000000" +
                    "00000000" +
                    "00000000";
            peicesStr[1] =
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00000000" +
                    "00100000";
            return peicesStr;
        }

        #endregion

        #region methods

        public ulong GetBlanks()
        {
            ulong blanks = 0;
            ulong ignore = 6100500256496637097;

            foreach (ulong[] team in Peices)
            {
                foreach (ulong peice in team)
                {
                    blanks |= peice;
                }
            }

            return ~(blanks & ignore);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong GetPeices(int team)
        {
            return Peices[team][0] | Peices[team][1];
        }

        public ulong GetPeices(bool team)
        {
            int turn = Convert.ToInt32(team);
            return Peices[turn][0] | Peices[turn][1];
        }

        public ulong GetDoubleBits(bool team)
        {
            int turn = Convert.ToInt32(team);
            return Peices[turn][1];
        }

        public bool MovePeice(ulong peice, ulong destination)
        {
            int team = Convert.ToInt32(isBlackTurn);
            int otherTeam = Convert.ToInt32(!isBlackTurn);
            bool capture = false;
            LastMove = destination;

            ulong test = (peices[team][0] & peice);

            if ((destination & 18374686479671623680UL) != 0 || (destination & 255UL) != 0 || (peices[team][0] & peice) == 0)
            {
                peices[team][1] |= destination;
            }
            else
            {
                peices[team][0] |= destination;

            }

            peices[team][0] &= ~peice;
            peices[team][1] &= ~peice;

            if (peice << 14 == destination)
            {
                peices[otherTeam][0] &= ~(peice << 7);
                peices[otherTeam][1] &= ~(peice << 7);
                capture = true;
            }
            else if (peice << 18 == destination)
            {
                peices[otherTeam][0] &= ~(peice << 9);
                peices[otherTeam][1] &= ~(peice << 9);
                capture = true;
            }
            else if (peice >> 14 == destination)
            {
                peices[otherTeam][0] &= ~(peice >> 7);
                peices[otherTeam][1] &= ~(peice >> 7);
                capture = true;
            }
            else if (peice >> 18 == destination)
            {
                peices[otherTeam][0] &= ~(peice >> 9);
                peices[otherTeam][1] &= ~(peice >> 9);
                capture = true;
            }

            return capture;
        }

        public bool HasWon()
        {
            return (peices[0][0] == 0 && peices[0][1] == 0) || (peices[1][0] == 0 && peices[1][1] == 0);
        }

        public int GetHeuristic()
        {
            int player = Convert.ToInt32(IsBlackTurn);
            int otherPlayer = Convert.ToInt32(!IsBlackTurn);

            ulong playerPeices = NumberOfSetBits(Peices[player][0]) + NumberOfSetBits(Peices[player][1]);
            ulong otherPlayerPeices = NumberOfSetBits(Peices[otherPlayer][0]) + NumberOfSetBits(Peices[otherPlayer][1]);

            return Convert.ToInt32(playerPeices) - Convert.ToInt32(otherPlayerPeices);
        }

        public bool IsPeiceTaken(CheckersBoardState otherState)
        {
            int thisTotal = Convert.ToInt32(NumberOfSetBits(GetPeices(true)) + NumberOfSetBits(GetPeices(false)));
            int otherTotal = Convert.ToInt32(NumberOfSetBits(otherState.GetPeices(true)) + NumberOfSetBits(otherState.GetPeices(false)));
            
            return thisTotal != otherTotal;
        }

        private ulong NumberOfSetBits(ulong i)
        {
            i = i - ((i >> 1) & 0x5555555555555555);
            i = (i & 0x3333333333333333) + ((i >> 2) & 0x3333333333333333);
            return (((i + (i >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56;
        }

        public static CheckersBoardState Max(CheckersBoardState a, CheckersBoardState b)
        {
            return a.GetHeuristic() > b.GetHeuristic() ? a : b;
        }

        public static CheckersBoardState Min(CheckersBoardState a, CheckersBoardState b)
        {
            return a.GetHeuristic() < b.GetHeuristic() ? a : b;
        }

        #endregion
    }
}
