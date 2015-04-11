using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chessThing
{
    abstract class Board
    {
        protected char[][] board;
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
                        if (selectedMoves.Any(m => m.endX == cursorX && m.endY == cursorY))
                            movePeice(selX,selY,cursorX,cursorY);

                        selX = -1;
                        selY = -1;
                        selectedMoves = null;
                    }
                    break;
            }

            drawBoard();
        }

        protected void movePeice(int startX, int startY, int desX, int desY)
        {
            board[desX][desY] = board[startX][startY];
            board[startX][startY] = ' ';
        }

        protected char getLocation(int x, int y)
        {
            if(x >= 0 && y >= 0)
                return board[x][y];

            return '!';
        }

        public abstract List<Move> getPosibleMoves(int x,int y);

        #region drawing

        public void drawBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                int width = 0;
                int height = i*3;
                Console.SetCursorPosition(width, height);
                drawRow("     ",i);
                Console.SetCursorPosition(width, height+1);
                drawRow("",i);
                Console.SetCursorPosition(width, height + 2);
                drawRow("     ",i);
            }
        }

        private void drawRow(string contents,int i)
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

                Console.Write(contents == "" ? "  " + board[j][i] + "  " : contents);
            }
        }

        #endregion
    }
}
