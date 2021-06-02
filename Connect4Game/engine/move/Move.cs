using System;
using Connect4Game.engine.board;
using Connect4Game.engine.piece;

namespace Connect4Game.engine.move
{
    public sealed class Move : IEquatable<Move>
    {
        private readonly Board _board;
        private readonly Piece _piece;
        public Move(in Board board, in int index, in League league)
        {
            _board = board;
            _piece = Piece.CreatePiece(league, index);
        }

        public Board ExecuteMove()
        {
            Board.BoardBuilder builder = new Board.BoardBuilder(_board.GetCurrentPlayer.GetOpponent().GetLeague());
            _board.TileList.ForEach(tile => { builder.SetTile(tile); });
            return builder.AppendTile(Tile.CreateTile(_piece, _piece.GetIndex)).BuildBoard();
        }

        public int GetIndex() { return _piece.GetIndex; }

        public override int GetHashCode() { return _piece.GetHashCode() * 31; }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            return Equals(obj as Move);
        }

        public override string ToString() { return _board.ToString(); }

        public bool Equals(Move move)
        {
            if (move == null) { throw new ArgumentException("Move cannot be null at Equals method"); }
            return move._piece.Equals(_piece);
        }

        public static bool operator == (Move move1, Move move2)
        {
            if (ReferenceEquals(move1, move2)) return true;
            if (ReferenceEquals(move1, null)) return false;
            if (ReferenceEquals(move2, null)) return false;
   
            return move1.Equals(move2);
        }

        public static bool operator != (Move move1, Move move2)
        {
            if (ReferenceEquals(move1, move2)) return false;
            if (ReferenceEquals(move1, null)) return true;
            if (ReferenceEquals(move2, null)) return true;

            return !move1.Equals(move2);
        }
    }
}