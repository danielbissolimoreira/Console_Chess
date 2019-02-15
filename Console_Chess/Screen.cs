using System;
using Console_Chess.ChessLayer;
using Console_Chess.GameBoard;
using Console_Chess.GameBoard.Enteties;
using Console_Chess.GameBoard.Enum;

namespace Console_Chess
{
    class Screen
    {
        public static void printBoard(Board board)
        {
            Console.WriteLine();
            for (int i = 0; i < board.Lines; i++)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(board.Lines - i);
                for (int j = 0; j < board.Columns; j++)
                {

                    Position p = new Position(i, j);

                    if ((i + j) % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }


                    if (!board.HasPiece(p))
                    {
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write(" ");
                        PrintPiece(board.Piece(p));
                        Console.Write(" ");
                    }

                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(" ");
            for (char letter = 'A'; letter < ('A' + board.Columns); letter++)
            {
                Console.Write(" " + letter + " ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void printBoard(Board board, bool[,] moves)
        {
            Console.WriteLine();
            for (int i = 0; i < board.Lines; i++)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(board.Lines - i);
                for (int j = 0; j < board.Columns; j++)
                {

                    Position p = new Position(i, j);

                    if (moves[i, j])
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        if ((i + j) % 2 == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                        }
                    }


                    if (!board.HasPiece(p))
                    {
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write(" ");
                        PrintPiece(board.Piece(p));
                        Console.Write(" ");
                    }

                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(" ");
            for (char letter = 'A'; letter < ('A' + board.Columns); letter++)
            {
                Console.Write(" " + letter + " ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void PrintPiece(Piece piece)
        {
            ConsoleColor aux = Console.ForegroundColor;
            if (piece.Color == Color.Black)
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else if (piece.Color == Color.White)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.Write(piece.ToString());
            Console.ForegroundColor = aux;
        }

        public static void PrintPlay(ChessMatch match)
        {
            ConsoleColor aux = Console.BackgroundColor;
            Console.BackgroundColor = aux;
            Screen.printBoard(match.Board);
            Console.BackgroundColor = aux;
            Console.WriteLine();
            PrintCapturedPieces(match);
            Console.WriteLine("\n");
            Console.WriteLine("Turno : " + match._round);

            if (!match.bMatchFinished)
            {
                Console.WriteLine("Aguardando jogada : " + match.activePlayer);

                if (match.InCheck)
                {
                    Console.WriteLine("XEQUE!");
                }
            }
            else
            {
                Console.WriteLine("XEQUEMATE!");
                Console.WriteLine("Vencedor : " + match.activePlayer);
            }
        }

        private static void PrintCapturedPieces(ChessMatch match)
        {
            Console.WriteLine("Peças Capturadas: ");
            Console.Write("Brancas: ");
            PrintPieceSet(match, Color.White);
            Console.Write("\nPretas: ");
            PrintPieceSet(match, Color.Black);
        }

        private static void PrintPieceSet(ChessMatch match, Color color)
        {
            Console.Write("[");
            foreach (Piece p in match.getCapturedPieces(color))
            {
                Console.Write(p.ToString() + " ");
            }
            Console.Write("]");
        }
    }
}