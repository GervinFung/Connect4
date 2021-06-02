using Connect4Game.engine.piece;

namespace Connect4Game.engine.board
{
    public abstract class Tile
    {
        private readonly int _index;
        private Tile(in int index) { _index = index; }
        public int Index => _index;
        public static Tile CreateTile(in Piece piece, in int index) { return piece == null ? new EmptyTile(index) : new OccupiedTile(piece, index); }
        public abstract bool IsTileOccupied();
        public abstract Piece GetPiece();

        private sealed class OccupiedTile : Tile
        {
            private readonly Piece _piece;
            public OccupiedTile(in Piece piece, in int index) : base(index) { _piece = piece; }
            public override bool IsTileOccupied() { return true; }
            public override Piece GetPiece() { return _piece; }
            public override string ToString() { return LeagueExtensions.IsBlack(_piece.GetLeague) ? "B" : "R"; }
        }

        private sealed class EmptyTile : Tile
        {
            public EmptyTile(in int index) : base(index) { }
            public override bool IsTileOccupied() { return false; }
            public override Piece GetPiece() { return null; }
            public override string ToString() { return "-"; }
        }
    }
}