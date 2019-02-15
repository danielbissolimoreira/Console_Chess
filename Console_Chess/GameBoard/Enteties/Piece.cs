using System;
using Console_Chess.GameBoard.Enum;

namespace Console_Chess.GameBoard.Enteties
{
    abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int MovementsMade { get; protected set; }
        public Board Board { get; set; }

        public Piece(Color color, Board board)
        {
            Position = null;
            Color = color;
            Board = board;
            MovementsMade = 0;
        }

        public void IncrementMovementsMade()
        {
            MovementsMade++;
        }

        public void DecrementMovementsMade()
        {
            MovementsMade--;
        }

        public abstract bool[,] PossibleMovements();

        public bool AnyPossibleMovements()
        {
            bool[,] moves = PossibleMovements();

            for (int i = 0; i < Board.Lines; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    if (moves[i, j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CanMoveTo(Position pos)
        {
            return PossibleMovements()[pos.Line, pos.Column];
        }

    }
}
