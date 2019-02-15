using System;
using Console_Chess.ChessLayer;
using Console_Chess.GameBoard;
using Console_Chess.GameBoard.Exceptions;

namespace Console_Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ChessMatch match = new ChessMatch();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.CursorSize = 24;
                //ConsoleColor aux = Console.BackgroundColor;

                while (!match.bMatchFinished)
                {
                    try
                    {

                        Console.Clear();
                        //Console.BackgroundColor = aux;
                        //Screen.printBoard(match.Board);
                        //Console.BackgroundColor = aux;
                        //Console.WriteLine("\n");
                        //Console.WriteLine("Turno : " + match._round);
                        //Console.WriteLine("Aguardando jogada : " + match.activePlayer);

                        Screen.PrintPlay(match);

                        Console.Write("Origem: ");
                        Position origen = match.ReadMovement().toPosition();

                        match.ValidateOrigen(origen);

                        bool[,] moves = match.Board.Piece(origen).PossibleMovements();
                        Console.Clear();

                        Screen.printBoard(match.Board, moves);
                        Console.WriteLine("\n");
                        Console.Write("Destino: ");
                        Position destination = match.ReadMovement().toPosition();

                        match.ValidateDestination(origen, destination);

                        match.ExecutePlay(origen, destination);
                    }
                    catch (BoardException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
