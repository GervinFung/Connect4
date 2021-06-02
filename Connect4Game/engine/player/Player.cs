using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Connect4Game.engine.board;
using Connect4Game.engine.move;
using Connect4Game.engine.piece;

namespace Connect4Game.engine.player
{
    public abstract class Player
    {
        private readonly Board _board;
        private readonly ImmutableList<Move> _legalMoves;

        protected Player(in Board board)
        {
            _board = board;
            _legalMoves = GenerateLegalMoves();
        }
        public ImmutableList<Move> GetLegalMoves => _legalMoves;
        protected Board GetBoard => _board;

        public abstract League GetLeague();
        public abstract Player GetOpponent();

        public Board MakeMove(in Move move)
        {
            if (move == null)
            {
                throw new ArgumentException("Move cannot be null");
            }
            if (GetLegalMoves.Contains(move)) { return move.ExecuteMove(); }
            throw new InvalidDataException("Move passed here is invalid");
        }

        private ImmutableList<Tile> GenerateEmptyRows()
        {
            List<Tile> emptyRows = new List<Tile>();
            int begin = GetBoard.NumCol * 5, min = 0;
            for (int i = begin; i >= min; i -= GetBoard.NumCol) 
            {
                if (i == GetBoard.NumTiles) { break; }
                Tile tile = GetBoard.GetTileAt(i);
                if (!tile.IsTileOccupied())
                {
                    emptyRows.Add(tile);
                    min++;
                    begin++;
                    i = begin + GetBoard.NumCol;
                }
                else
                {
                    if (i == min)
                    {
                        min++;
                        begin++;
                        i = begin + GetBoard.NumCol;
                    }
                }
            }
            return emptyRows.ToImmutableList();
        }

        private ImmutableList<Move> GenerateLegalMoves() {
            List<Move> moves = new List<Move>();
            GenerateEmptyRows().ForEach(tile => { moves.Add(new Move(GetBoard, tile.Index, GetLeague())); });
            return moves.ToImmutableList();
        }

        private bool CheckHorizontalWin()
        {
            int numTileOccupied = 0;
            int begin = 0;
            for (int i = begin; i < _board.NumTiles; i++)
            {
                Tile tile = _board.GetTileAt(i);
                if (tile.IsTileOccupied())
                {
                    numTileOccupied = tile.GetPiece().GetLeague == GetOpponent().GetLeague() ? numTileOccupied + 1 : 0;
                    if (numTileOccupied == Board.DEFAULT_WIN_NUM_TILES) { return true; }
                }
                else { numTileOccupied = 0; }
                if (i - begin == 6)
                {
                    numTileOccupied = 0;
                    begin += _board.NumCol;
                }
            }
            return false;
        }

        private bool CheckVerticalWin()
        {
            int numTileOccupied = 0, begin = 0, max = _board.NumCol * 5;

            for (int i = begin; i <= max; i += _board.NumCol) 
            {
                Tile tile = _board.GetTileAt(i);
                if (tile.IsTileOccupied()) {
                    numTileOccupied = tile.GetPiece().GetLeague == GetOpponent().GetLeague() ? numTileOccupied + 1 : 0;
                    if (numTileOccupied == Board.DEFAULT_WIN_NUM_TILES) { return true; }
                }
                else { numTileOccupied = 0; }

                if (i == max && max < Board.DEFAULT_NUM_TILES - 1) {
                    begin++;
                    i = begin - _board.NumCol;
                    max++;
                    numTileOccupied = 0;
                }
            }
            return false;
        }

        private bool CheckRightToLeftDiagonalWin()
        {
            int increment = _board.NumCol - 1, begin = (_board.NumCol - 1) / 2;
            int max = 21, numTileOccupied = 0;
            bool goEdge = false;
            for (int i = begin; i <= max; i+=increment)
            {
                Tile tile = _board.GetTileAt(i);
                if (tile.IsTileOccupied()) {
                    numTileOccupied = tile.GetPiece().GetLeague == GetOpponent().GetLeague() ? numTileOccupied + 1 : 0;
                    if (numTileOccupied == Board.DEFAULT_WIN_NUM_TILES) { return true; }
                }
                else { numTileOccupied = 0; }
                if (i == max)
                {
                    if (begin == 20) { break; }
                    if (begin == increment && !goEdge) { goEdge = true; }
                    begin = goEdge ? begin + _board.NumCol : begin + 1;
                    max = max + _board.NumCol >= _board.NumTiles ? max + 1 : max + _board.NumCol;
                    i = begin - increment;
                    numTileOccupied = 0;
                }
            }
            return false;
        }

        private bool CheckLeftToRightDiagonalWin()
        {
            int increment = _board.NumCol + 1, begin = (_board.NumCol - 1) / 2;
            int max = 27, numTileOccupied = 0;
            bool goEdge = false;
            for (int i = begin; i <= max; i+=increment)
            {
                Tile tile = _board.GetTileAt(i);
                if (tile.IsTileOccupied()) {
                    numTileOccupied = tile.GetPiece().GetLeague == GetOpponent().GetLeague() ? numTileOccupied + 1 : 0;
                    if (numTileOccupied == Board.DEFAULT_WIN_NUM_TILES) { return true; }
                }
                else { numTileOccupied = 0; }
                if (i == max)
                {
                    if (begin == 14) { break; }
                    if (begin == 0 && !goEdge) { goEdge = true; }
                    begin = goEdge ? begin + _board.NumCol : begin - 1;
                    max = max + _board.NumCol >= _board.NumTiles ? max - 1 : max + _board.NumCol;
                    i = begin - increment;
                    numTileOccupied = 0;
                }
            }
            return false;
        }

        public bool IsInCheckmate()
        {
            return CheckVerticalWin() || CheckHorizontalWin() || CheckLeftToRightDiagonalWin() || CheckRightToLeftDiagonalWin();
        }

        public bool IsStalemate() { return _board.TileList.All(tile => tile.IsTileOccupied()); }
    }

    public sealed class RedPlayer : Player
    {
        public RedPlayer(in Board board) : base (board){}
        public override League GetLeague() { return League.Red; }
        public override BlackPlayer GetOpponent() { return GetBoard.GetBlackPlayer; }
        public override string ToString() { return "Player Red"; }
    }

    public sealed class BlackPlayer : Player
    {
        public BlackPlayer(in Board board) : base (board){}
        public override League GetLeague() { return League.Black; }
        public override RedPlayer GetOpponent() { return GetBoard.GetRedPlayer; }
        public override string ToString() { return "Player Black"; }
    }
}