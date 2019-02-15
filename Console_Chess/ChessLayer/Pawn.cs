using Console_Chess.GameBoard;
using Console_Chess.GameBoard.Enteties;
using Console_Chess.GameBoard.Enum;

namespace Console_Chess.ChessLayer
{
    class Pawn : Piece
    {
        private ChessMatch _match;

        public Pawn(Color color, Board board, ChessMatch match) : base(color, board)
        {
            _match = match;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool CanMove(Position position)
        {
            Piece piece = Board.Piece(position);
            return (piece == null || piece.Color != Color);
        }

        private bool HasEnemy(Position position)
        {
            Piece piece = Board.Piece(position);
            return (piece != null && piece.Color != Color);
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] moves = new bool[Board.Lines, Board.Columns];
            Position p = new Position(0, 0);

            // Verificar todos os movimentos possíveis do Peão

            if (this.Color == Color.Black)
            {
                // Acima
                p.DefinePosition(Position.Line + 1, Position.Column);
                if (Board.IsValidPosition(p) && CanMove(p))
                    moves[p.Line, p.Column] = true;

                //Acima 2
                if (MovementsMade == 0)
                {
                    p.DefinePosition(Position.Line + 2, Position.Column);
                    if (Board.IsValidPosition(p) && !HasEnemy(p))
                        moves[p.Line, p.Column] = true;
                }

                // Nordeste
                p.DefinePosition(Position.Line + 1, Position.Column + 1);
                if (Board.IsValidPosition(p) && HasEnemy(p))
                    moves[p.Line, p.Column] = true;

                // Noroeste
                p.DefinePosition(Position.Line + 1, Position.Column - 1);
                if (Board.IsValidPosition(p) && HasEnemy(p))
                    moves[p.Line, p.Column] = true;

                // #Special Move - en Passant
                if (Position.Line == 4)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if(Board.IsValidPosition(left) &&
                        HasEnemy(left) &&
                        Board.Piece(left) == _match.EnPassantState)
                    {
                        moves[left.Line+1, left.Column] = true;
                    }

                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.IsValidPosition(right) &&
                        HasEnemy(right) &&
                        Board.Piece(right) == _match.EnPassantState)
                    {
                        moves[right.Line +1 , right.Column] = true;
                    }
                }
            }
            else
            {
                // Acima
                p.DefinePosition(Position.Line - 1, Position.Column);
                if (Board.IsValidPosition(p) && CanMove(p))
                    moves[p.Line, p.Column] = true;

                //Acima 2
                if (MovementsMade == 0)
                {
                    p.DefinePosition(Position.Line - 2, Position.Column);
                    if (Board.IsValidPosition(p) && !HasEnemy(p))
                        moves[p.Line, p.Column] = true;
                }

                // Nordeste
                p.DefinePosition(Position.Line - 1, Position.Column + 1);
                if (Board.IsValidPosition(p) && HasEnemy(p))
                    moves[p.Line, p.Column] = true;

                // Noroeste
                p.DefinePosition(Position.Line - 1, Position.Column - 1);
                if (Board.IsValidPosition(p) && HasEnemy(p))
                    moves[p.Line, p.Column] = true;

                // #Special Move - en Passant
                if (Position.Line == 3)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if (Board.IsValidPosition(left) &&
                        HasEnemy(left) &&
                        Board.Piece(left) == _match.EnPassantState)
                    {
                        moves[left.Line-1, left.Column] = true;
                    }

                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.IsValidPosition(right) &&
                        HasEnemy(right) &&
                        Board.Piece(right) == _match.EnPassantState)
                    {
                        moves[right.Line-1, right.Column] = true;
                    }
                }
            }

            return moves;
        }
    }
}
