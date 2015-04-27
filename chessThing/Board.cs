using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chessThing
{
    abstract class Board
    {
        public ulong[][] peices;

        int cursorY = 0, cursorX = 0;
        int selX = -1, selY = -1;
        List<Move> selectedMoves;

        public Board(){

        }

        public void moveCursor(ConsoleKey key)
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
                        selectedMoves = getPosibleMoves(selX, selY);
                    }
                    else
                    {
                        //if (selectedMoves.Any(m => m.endX == cursorX && m.endY == cursorY))
                        //    movePeice(selX,selY,cursorX,cursorY);

                        selX = -1;
                        selY = -1;
                        selectedMoves = null;
                    }
                    break;
            }

            drawBoard();
        }

        public abstract List<Move> getPosibleMoves(int x,int y);

        #region drawing

        public void drawBoard()
        {
            char[][] board = getBoardView();

            for (int i = 0; i < 8; i++)
            {
                int width = 0;
                int height = i*3;
                Console.SetCursorPosition(width, height);
                drawRow("     ", i, board);
                Console.SetCursorPosition(width, height+1);
                drawRow("", i, board);
                Console.SetCursorPosition(width, height + 2);
                drawRow("     ", i, board);
            }
        }

        private void drawRow(string contents, int i, char[][] board)
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
                else if (selectedMoves != null && selectedMoves.Any(m => m.endX == j && m.endY == i))
                {
                    Console.BackgroundColor = ConsoleColor.Green;
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

        private char[][] getBoardView()
        {
            char[][] view = new char[8][];

            for (int i = 0; i < 8; i++)
            {
                view[i] = new char[8];
            }
            
            int tCounter = 0;
            int pCounter = 0;

            char[] bits = {'p','r','b','n','q','k','P','R','B','N','Q','K'};

            foreach (char p in bits)
            {
                int rowCounter = 0;
                string board = Convert.ToString((Int64)Convert.ToUInt64(peices[tCounter][pCounter]), 2).PadLeft(64,'0');
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

        #endregion
    }
}
