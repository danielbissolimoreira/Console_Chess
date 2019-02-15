using System;
using Console_Chess.GameBoard;
using Console_Chess.GameBoard.Enteties;
using Console_Chess.GameBoard.Enum;


namespace Console_Chess.ChessLayer
{
    class King : Piece
    {
        private ChessMatch _match;

        public King(Color color, Board board, ChessMatch match) : base(color, board)
        {
            _match = match;
        }

        private bool CanMove(Position position)
        {
            Piece piece = Board.Piece(position);
            return (piece == null || piece.Color != Color);
        }

        private bool ElegibleForCastling(Position pos)
        {
            Piece piece = Board.Piece(pos);
            return piece != null && piece is Rook && piece.MovementsMade == 0 && piece.Color == Color;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] moves = new bool[Board.Lines, Board.Columns];
            Position p = new Position(0, 0);

            // Verificar todos os movimentos possíveis do Rei

            // Acima
            p.DefinePosition(Position.Line - 1, Position.Column);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            //Abaixo
            p.DefinePosition(Position.Line + 1, Position.Column);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Direita
            p.DefinePosition(Position.Line, Position.Column + 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Esquerda
            p.DefinePosition(Position.Line, Position.Column - 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Nordeste
            p.DefinePosition(Position.Line - 1, Position.Column + 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Noroeste
            p.DefinePosition(Position.Line - 1, Position.Column - 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Sudeste
            p.DefinePosition(Position.Line + 1, Position.Column + 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;

            // Sudoeste
            p.DefinePosition(Position.Line + 1, Position.Column - 1);
            if (Board.IsValidPosition(p) && CanMove(p))
                moves[p.Line, p.Column] = true;


            // #Special Move
            if (MovementsMade == 0 && !_match.InCheck)
            {
                // Short Castling
                Position posKingShort = new Position(Position.Line, Position.Column + 3);

                if (ElegibleForCastling(posKingShort))
                {
                    Position p1 = new Position(Position.Line, Position.Column + 1);
                    Position p2 = new Position(Position.Line, Position.Column + 2);
                    if (Board.Piece(p1) == null && Board.Piece(p2) == null)
                    {
                        moves[Position.Line, Position.Column + 2] = true;
                    }
                }

                // Long Castling
                Position posKingLong = new Position(Position.Line, Position.Column - 4);

                if (ElegibleForCastling(posKingLong))
                {
                    Position p1 = new Position(Position.Line, Position.Column - 1);
                    Position p2 = new Position(Position.Line, Position.Column - 2);
                    Position p3 = new Position(Position.Line, Position.Column - 3);
                    if (Board.Piece(p1) == null && Board.Piece(p2) == null && Board.Piece(p3) == null)
                    {
                        moves[Position.Line, Position.Column - 2] = true;
                    }
                }
            }

            return moves;
        }

        public override string ToString()
        {
            return "K";
        }
    }
}
