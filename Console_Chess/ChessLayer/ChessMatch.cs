using System;
using System.Collections.Generic;
using Console_Chess.GameBoard;
using Console_Chess.GameBoard.Enteties;
using Console_Chess.GameBoard.Enum;
using Console_Chess.GameBoard.Exceptions;

namespace Console_Chess.ChessLayer
{
    class ChessMatch
    {
        public Board Board { get; private set; }
        public int _round { get; private set; }
        public Color activePlayer { get; private set; }
        public bool bMatchFinished;
        public bool InCheck { get; private set; }
        public HashSet<Piece> Pieces;
        public HashSet<Piece> CapturedPieces;
        public Piece EnPassantState { get; private set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            _round = 1;
            activePlayer = Color.White;
            InCheck = false;
            EnPassantState = null;
            Pieces = new HashSet<Piece>();
            CapturedPieces = new HashSet<Piece>();
            InitializeMatch();
        }

        public void ExecutePlay(Position origin, Position destination)
        {
            Piece capturedPiece = ExecuteMovement(origin, destination);

            if (IsKingInCheck(activePlayer))
            {
                UndoExecuteMovement(origin, destination, capturedPiece);
                throw new BoardException("Você não pode se colocar em cheque!");
            }

            Piece p = Board.Piece(destination);

            // #Special Move - Promotion
            if (p is Pawn)
            {
                if (p.Color == Color.White && destination.Line == 0 || p.Color == Color.Black && destination.Line == 7)
                {
                    p = Board.RemovePiece(destination);
                    Pieces.Remove(p);
                    bool correctChoice = true;
                    Piece newPiece = null;
                    do
                    {
                        Console.Write("Escolha uma Peça (Q, B, R, L): ");
                        string choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "Q":
                                newPiece = new Queen(p.Color, Board);
                                break;
                            case "B":
                                newPiece = new Bishop(p.Color, Board);
                                break;
                            case "R":
                                newPiece = new Rook(p.Color, Board);
                                break;
                            case "L":
                                newPiece = new Knight(p.Color, Board);
                                break;
                            default:
                                correctChoice = false;
                                break;
                        }
                        if (!correctChoice)
                        {
                            Console.Write("Escolha inválida!");
                        }
                        else
                        {
                            Board.PutPiece(newPiece, destination);
                            Pieces.Add(newPiece);
                        }
                    } while (!correctChoice);
                }
            }

            InCheck = IsKingInCheck(Adversary(activePlayer));

            if (TestCheckMate(Adversary(activePlayer)))
            {
                bMatchFinished = true;
            }
            else
            {
                _round++;
                SwitchPlayer();
            }

            // #Special Move - en Passant
            if (p is Pawn && (destination.Line == origin.Line + 2 || destination.Line == origin.Line - 2))
            {
                EnPassantState = p;
            }
            else
            {
                EnPassantState = null;
            }

        }

        private Piece ExecuteMovement(Position origin, Position destination)
        {
            Piece piece = Board.RemovePiece(origin);
            piece.IncrementMovementsMade();

            Piece removedPiece = Board.RemovePiece(destination);
            Board.PutPiece(piece, destination);
            if (removedPiece != null)
            {
                CapturedPieces.Add(removedPiece);
            }

            // #Special Move - Short Casteling
            if (piece is King && destination.Column == origin.Column + 2)
            {
                Position posOriRook = new Position(origin.Line, origin.Column + 3);
                Position posDestRook = new Position(origin.Line, origin.Column + 1);
                Piece rook = Board.RemovePiece(posOriRook);
                rook.IncrementMovementsMade();
                Board.PutPiece(rook, posDestRook);
            }

            // #Special Move - Long Casteling
            if (piece is King && destination.Column == origin.Column - 2)
            {
                Position posOriRook = new Position(origin.Line, origin.Column - 4);
                Position posDestRook = new Position(origin.Line, origin.Column - 1);
                Piece rook = Board.RemovePiece(posOriRook);
                rook.IncrementMovementsMade();
                Board.PutPiece(rook, posDestRook);
            }

            // #Special Move - en Passant
            if (piece is Pawn && origin.Column != destination.Column && removedPiece == null)
            {
                Position capturePos;
                if (piece.Color == Color.White)
                {
                    capturePos = new Position(destination.Line + 1, destination.Column);
                }
                else
                {
                    capturePos = new Position(destination.Line - 1, destination.Column);
                }

                removedPiece = Board.RemovePiece(capturePos);
                CapturedPieces.Add(removedPiece);
            }


            return removedPiece;
        }

        public void UndoExecuteMovement(Position origin, Position destination, Piece capturedPiece)
        {
            Piece piece = Board.RemovePiece(destination);
            piece.DecrementMovementsMade();

            if (capturedPiece != null)
            {
                Board.PutPiece(capturedPiece, destination);
                CapturedPieces.Remove(capturedPiece);
            }
            Board.PutPiece(piece, origin);

            // #Special Move - Short Casteling
            if (piece is King && destination.Column == origin.Column + 2)
            {
                Position posOriRook = new Position(origin.Line, origin.Column + 3);
                Position posDestRook = new Position(origin.Line, origin.Column + 1);
                Piece rook = Board.Piece(posDestRook);
                rook.IncrementMovementsMade();
                Board.PutPiece(rook, posOriRook);
            }

            // #Special Move - Long Casteling
            if (piece is King && destination.Column == origin.Column - 2)
            {
                Position posOriRook = new Position(origin.Line, origin.Column - 4);
                Position posDestRook = new Position(origin.Line, origin.Column - 1);
                Piece rook = Board.Piece(posDestRook);
                rook.IncrementMovementsMade();
                Board.PutPiece(rook, posOriRook);
            }

            // #Special Move - en Passant
            if (piece is Pawn && origin.Column != destination.Column && capturedPiece == EnPassantState)
            {
                Piece p = Board.Piece(destination);
                Position capturePos;
                if (piece.Color == Color.White)
                {
                    capturePos = new Position(3, destination.Column);
                }
                else
                {
                    capturePos = new Position(4, destination.Column);
                }

                Board.PutPiece(p, capturePos);
            }

        }

        private void SwitchPlayer()
        {
            activePlayer = (activePlayer == Color.White) ? Color.Black : Color.White;
        }

        public ChessPosition ReadMovement()
        {
            string sPos = Console.ReadLine();
            return new ChessPosition(int.Parse("" + sPos[1]), sPos[0], Board);
        }

        public void ValidateOrigen(Position pos)
        {
            if (Board.Piece(pos) == null)
            {
                throw new BoardException("Não existe peça na posição de origem excluída!");
            }

            if (activePlayer != Board.Piece(pos).Color)
            {
                throw new BoardException("A peça de origem escolhida não é sua!");
            }

            if (!Board.Piece(pos).AnyPossibleMovements())
            {
                throw new BoardException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidateDestination(Position origen, Position destination)
        {
            if (!Board.Piece(origen).CanMoveTo(destination))
            {
                throw new BoardException("Posição de destino inválida!");
            }
        }

        public HashSet<Piece> getCapturedPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in CapturedPieces)
            {
                if (p.Color == color)
                {
                    aux.Add(p);
                }
            }
            return aux;
        }

        public HashSet<Piece> PiecesInGame(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in Pieces)
            {
                if (p.Color == color)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(getCapturedPieces(color));
            return aux;
        }

        private Color Adversary(Color c)
        {
            return (c == Color.White ? Color.Black : Color.White);
        }

        private Piece GetKing(Color color)
        {
            foreach (Piece x in PiecesInGame(color))
            {
                if (x is King)
                {
                    return x;
                }
            }
            return null;
        }

        private bool IsKingInCheck(Color color)
        {
            Piece king = GetKing(color);
            if (king == null)
            {
                throw new BoardException("Não existe Rei da cor " + color + " no tabuleiro!");
            }

            foreach (Piece p in PiecesInGame(Adversary(color)))
            {
                bool[,] moves = p.PossibleMovements();
                if (moves[king.Position.Line, king.Position.Column])
                {
                    return true;
                }
            }

            return false;
        }

        private bool TestCheckMate(Color color)
        {
            if (!IsKingInCheck(color))
            {
                return false;
            }

            foreach (Piece p in Pieces)
            {
                bool[,] moves = p.PossibleMovements();
                for (int i = 0; i < Board.Lines; i++)
                {
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        Position orig = p.Position;
                        Position dest = new Position(i, j);
                        Piece capturedPiece = ExecuteMovement(orig, dest);
                        bool testCheck = IsKingInCheck(color);
                        UndoExecuteMovement(orig, dest, capturedPiece);
                        if (testCheck)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void PlacePiece(Piece piece, Position position)
        {
            Board.PutPiece(piece, position);
            Pieces.Add(piece);
        }

        private void InitializeMatch()
        {
            PlacePiece(new Rook(Color.Black, Board), new Position(0, 0));
            PlacePiece(new Rook(Color.Black, Board), new Position(0, 7));
            PlacePiece(new Rook(Color.White, Board), new Position(7, 0));
            PlacePiece(new Rook(Color.White, Board), new Position(7, 7));

            PlacePiece(new Knight(Color.Black, Board), new Position(0, 1));
            PlacePiece(new Knight(Color.Black, Board), new Position(0, 6));
            PlacePiece(new Knight(Color.White, Board), new Position(7, 1));
            PlacePiece(new Knight(Color.White, Board), new Position(7, 6));

            PlacePiece(new Bishop(Color.Black, Board), new Position(0, 2));
            PlacePiece(new Bishop(Color.Black, Board), new Position(0, 5));
            PlacePiece(new Bishop(Color.White, Board), new Position(7, 2));
            PlacePiece(new Bishop(Color.White, Board), new Position(7, 5));

            PlacePiece(new Queen(Color.Black, Board), new Position(0, 3));
            PlacePiece(new Queen(Color.White, Board), new Position(7, 3));

            PlacePiece(new King(Color.Black, Board, this), new Position(0, 4));
            PlacePiece(new King(Color.White, Board, this), new Position(7, 4));

            for (int i = 0; i < Board.Columns; i++)
            {
                PlacePiece(new Pawn(Color.Black, Board, this), new Position(1, i));
                PlacePiece(new Pawn(Color.White, Board, this), new Position(6, i));
            }
        }
    }
}
