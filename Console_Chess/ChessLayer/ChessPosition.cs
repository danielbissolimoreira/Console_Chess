using Console_Chess.GameBoard;
using Console_Chess.GameBoard.Enteties;


namespace Console_Chess.ChessLayer
{
    class ChessPosition
    {

        public int Line { get; set; }
        public char Column { get; set; }
        public Board Board { get; set; }

        public ChessPosition(int line, char column, Board board)
        {
            Line = line;
            Column = column;
            Board = board;
        }

        public Position toPosition()
        {
            return new Position(Board.Lines - Line, Column - 'A');
        }

        public override string ToString()
        {
            return "" + Column + Line;
        }

    }
}
