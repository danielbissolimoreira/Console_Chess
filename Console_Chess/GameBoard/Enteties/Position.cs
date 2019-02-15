using System;

namespace Console_Chess.GameBoard
{
    class Position
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public Position(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public void DefinePosition(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return Line + ", " + Column;
        }
    }
}
