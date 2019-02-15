using System;
using Console_Chess.GameBoard;
using Console_Chess.GameBoard.Enteties;
using Console_Chess.GameBoard.Enum;


namespace Console_Chess.ChessLayer
{
    class Bishop : Piece
    {
        public Bishop( Color color, Board board) : base( color, board)
        {
        }

        public override string ToString()
        {
            return "B";
        }

        private bool CanMove(Position position)
        {
            Piece piece = Board.Piece(position);
            return (piece == null || piece.Color != Color);
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] moves = new bool[Board.Lines, Board.Columns];
            Position p = new Position(0, 0);

            // Verificar todos os movimentos possíveis do Bispo

            // NO
            p.DefinePosition(Position.Line - 1, Position.Column - 1);
            while (Board.IsValidPosition(p) && CanMove(p))
            {
                moves[p.Line, p.Column] = true;
                if (Board.Piece(p) != null && Board.Piece(p).Color != Color) break;

                p.Line -= 1;
                p.Column -= 1;
            }

            //NE
            p.DefinePosition(Position.Line - 1, Position.Column + 1);
            while (Board.IsValidPosition(p) && CanMove(p))
            {
                moves[p.Line, p.Column] = true;
                if (Board.Piece(p) != null && Board.Piece(p).Color != Color) break;

                p.Line -= 1;
                p.Column += 1;
            }

            // SO
            p.DefinePosition(Position.Line + 1, Position.Column - 1);
            while (Board.IsValidPosition(p) && CanMove(p))
            {
                moves[p.Line, p.Column] = true;
                if (Board.Piece(p) != null && Board.Piece(p).Color != Color) break;

                p.Line += 1;
                p.Column -= 1;
            }

            // SE
            p.DefinePosition(Position.Line + 1, Position.Column + 1);
            while (Board.IsValidPosition(p) && CanMove(p))
            {
                moves[p.Line, p.Column] = true;
                if (Board.Piece(p) != null && Board.Piece(p).Color != Color) break;

                p.Line += 1;
                p.Column += 1;
            }

            return moves;
        }
    }
}
