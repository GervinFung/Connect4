using System;

namespace Connect4Game.engine.piece
{
    public abstract class Piece: IEquatable<Piece>
    {
        private readonly League _league;
        private readonly int _index;
        private Piece(in League league, in int index)
        {
            _league = league;
            _index = index;
        }
        public int GetIndex => _index;
        public League GetLeague => _league;
        public bool Equals(Piece piece)
        {
            if (piece == null) { throw new ArgumentException("Piece cannot be null at Equals method"); }

            return piece.GetHashCode() == GetHashCode() && piece._index == _index;
        }
        
        public static bool operator == (Piece piece1, Piece piece2)
        {
            if (ReferenceEquals(piece1, piece2)) return true;
            if (ReferenceEquals(piece1, null)) return false;
            if (ReferenceEquals(piece2, null)) return false;
   
            return piece1.Equals(piece2);
        }

        public static bool operator != (Piece piece1, Piece piece2)
        {
            if (ReferenceEquals(piece1, piece2)) return false;
            if (ReferenceEquals(piece1, null)) return true;
            if (ReferenceEquals(piece2, null)) return true;

            return !piece1.Equals(piece2);
        }

        public sealed override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            return Equals(obj as Piece);
        }
        public sealed override int GetHashCode() { return (_league.GetHashCode() + _index) * 31; }

        public static Piece CreatePiece(in League league, in int index) { return LeagueExtensions.IsBlack(league) ? new BlackPiece(index) : new RedPiece(index); }

        private sealed class BlackPiece : Piece { public BlackPiece (in int index) : base(League.Black, index) {} }
        private sealed class RedPiece : Piece { public RedPiece (in int index) : base(League.Red, index) {} }
    }
}