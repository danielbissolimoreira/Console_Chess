using System;

namespace Console_Chess.GameBoard.Exceptions
{
    class BoardException: Exception
    {
        public BoardException(String message): base(message)
        {
        }
    }
}
