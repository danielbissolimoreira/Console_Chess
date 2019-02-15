using System;
using Console_Chess.GameBoard;
using Console_Chess.GameBoard.Enteties;
using Console_Chess.GameBoard.Enum;

namespace Console_Chess.ChessLayer
{
    class Knight : Piece
    {
        public Knight( Color color, Board board) : base( color, board)
        {
        }

        public override string ToString()
        {
            return "L";
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

            // Verificar todos os movimentos possíveis do Rei

            // Acima
            p.DefinePosition(Position.Line - 2, Position.Column + 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            //Abaixo
            p.DefinePosition(Position.Line - 1, Position.Column + 2);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Direita
            p.DefinePosition(Position.Line - 2, Position.Column - 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Esquerda
            p.DefinePosition(Position.Line - 1, Position.Column - 2);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Nordeste
            p.DefinePosition(Position.Line + 2, Position.Column - 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Noroeste
            p.DefinePosition(Position.Line + 1, Position.Column - 2);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Sudeste
            p.DefinePosition(Position.Line + 2, Position.Column + 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Sudoeste
            p.DefinePosition(Position.Line + 1, Position.Column + 2);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            return moves;
        }
    }
}
