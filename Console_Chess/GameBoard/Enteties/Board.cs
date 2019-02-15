using Console_Chess.GameBoard.Exceptions;

namespace Console_Chess.GameBoard.Enteties
{
    class Board
    {

        public int Lines { get; set; }
        public int Columns { get; set; }
        private Piece[,] Pieces;

        public Board(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;
            Pieces = new Piece[lines, columns];
        }

        public Piece Piece (Position position)
        {
            return Pieces[position.Line, position.Column];
        }

        public bool HasPiece(Position position)
        {
            ValidatePostion(position);
            return Piece(position) != null;
        }

        public void PutPiece(Piece piece, Position position)
        {
            if (HasPiece(position))
            {
                throw new BoardException("Já existe uma peça nessa posição!");
            }

            Pieces[position.Line, position.Column] = piece;
            piece.Position = position;
        }

        public Piece RemovePiece(Position position)
        {
            if (Piece(position) == null)
            {
                return null;
            }
            else
            {
                Piece pAux = Piece(position);
                pAux.Position = null;
                Pieces[position.Line, position.Column] = null;
                return pAux;
            }
        }

        public bool IsValidPosition(Position position)
        {
            return !(position.Line < 0 || position.Line >= Lines || position.Column < 0 || position.Column >= Columns);
        }

        public void ValidatePostion(Position position)
        {
            if (!IsValidPosition(position)) {
                throw new BoardException("Posição Inválida!");
            }
        }

    }
}
