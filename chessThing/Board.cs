using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Checkers;

namespace chessThing
{
    abstract class Board
    {
        public ulong Highlighted;
        protected char[][] boardView;

        int cursorY = 0, cursorX = 0;
        int selX = -1, selY = -1;
        ulong selectedMoves;

        private CheckersBoardState state;

        public CheckersBoardState State
        {
            get { return state; }
            set { state = value; boardView = null; }
        }

        public Board(){

        }

        public void MoveCursor(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    cursorX--;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    cursorX++;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    cursorY++;
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    cursorY--;
                    break;
                case ConsoleKey.Spacebar:
                    if (selX == -1)
                    {
                        selX = cursorX;
                        selY = cursorY;
                        selectedMoves = GetPosibleMoves(selX, selY);
                    }
                    else
                    {
                    
                        int num = Math.Abs(cursorY - 7) * 8 + Math.Abs(cursorX - 7);
                        ulong destination = Convert.ToUInt64(1UL << num);

                        if((destination & selectedMoves) != 0UL)
                        {
                            int num2 = Math.Abs(selY - 7) * 8 + Math.Abs(selX - 7);
                            ulong peice = Convert.ToUInt64(1UL << num2);
                            bool hasCaptured = state.MovePeice(peice, destination);

                            DrawBoard();

                            if (!hasCaptured)
                            {
                                State.EndTurn();
                                CheckersBoardState newState;
                                CheckersBoardState oldState;

                                do
                                {
                                    oldState = state;
                                    newState = AI.AlphaBetaInit(State, 8, int.MinValue, int.MaxValue, true);
                                    state = newState;
                                    DrawBoard();
                                }
                                while (newState.IsPeiceTaken(oldState));
                            }
                        }


                        selX = -1;
                        selY = -1;
                        selectedMoves = 0;
                    }
                    break;
            }

            DrawBoard();
        }

        public abstract ulong GetPosibleMoves(int x,int y);

        protected abstract char[][] GenerateBoardView();

        public char[][] GetBoardView()
        {
            if(boardView == null)
                return GenerateBoardView();
            else
                return boardView;
        }

        protected char[][] UlongToCharArray(ulong val)
        {
            char[][] output = new char[8][];

            for (int i = 0; i < 8; i++)
                output[i] = new char[8];

            int rowCounter = 0;
            string board = Convert.ToString((Int64)Convert.ToUInt64(val), 2).PadLeft(64, '0');
            foreach (char c in board)
            {
                if (c == '1')
                    output[rowCounter / 8][rowCounter % 8] = 'h';

                rowCounter++;
            }

            return output;
        }

        #region drawing

        public void DrawBoard()
        {
            char[][] board = GetBoardView();

            char[][] highlighted = UlongToCharArray(Highlighted);

            char[][] selected = UlongToCharArray(selectedMoves);

            for (int i = 0; i < 8; i++)
            {
                int width = 0;
                int height = i*3;
                Console.SetCursorPosition(width, height);
                DrawRow("     ", i, board, highlighted, selected);
                Console.SetCursorPosition(width, height+1);
                DrawRow("", i, board, highlighted, selected);
                Console.SetCursorPosition(width, height + 2);
                DrawRow("     ", i, board, highlighted, selected);
            }
        }

        private void DrawRow(string contents, int i, char[][] board, char[][] highlighted, char[][] selected)
        {
            //for each row
            for (int j = 0; j < 8; j++)
            {
                if (j == cursorX && i == cursorY)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (j == selX && i == selY)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (selected[i][j] == 'h')
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (highlighted[i][j] == 'h')
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if ((j + i) % 2 == 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(contents == "" ? "  " + board[i][j] + "  " : contents);
            }
        }

        #endregion
    }
}
