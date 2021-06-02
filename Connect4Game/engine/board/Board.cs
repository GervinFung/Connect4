using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Connect4Game.engine.piece;
using Connect4Game.engine.player;

namespace Connect4Game.engine.board
{
    public sealed class Board
    {
        public static readonly int DEFAULT_NUM_TILES = 42, DEFAULT_WIN_NUM_TILES = 4;
        private static readonly int DEFAULT_ROW = 6, DEFAULT_COL = 7;
        private readonly int _numTiles, _numCol;
        private readonly ImmutableList<Tile> _tileList;

        private readonly RedPlayer _redPlayer;
        private readonly BlackPlayer _blackPlayer;
        private readonly Player _currentPlayer;

        public ImmutableList<Tile> TileList => _tileList;
        public int NumTiles => _numTiles;
        public int NumCol => _numCol;

        private Board(in BoardBuilder builder)
        {
            _numCol = DEFAULT_COL;
            _numTiles = _numCol * DEFAULT_ROW;
            _tileList = builder.BoardConfiguration;
            _blackPlayer = new BlackPlayer(this);
            _redPlayer = new RedPlayer(this);
            _currentPlayer = builder.ChoosePlayer(_redPlayer, _blackPlayer);
        }

        public Tile GetTileAt(in int index) { return TileList[index]; }

        public static Board CreateStandardBoard()
        {
            BoardBuilder builder = new BoardBuilder(League.Black);
            for (int i = 0; i < DEFAULT_NUM_TILES; i++) { builder.SetTile(Tile.CreateTile(null, i)); }
            return builder.BuildBoard();
        }

        public RedPlayer GetRedPlayer => _redPlayer;
        public BlackPlayer GetBlackPlayer => _blackPlayer;
        public Player GetCurrentPlayer => _currentPlayer;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int separator = NumCol;
            for (int i = 0; i < _numTiles; i++)
            {
                if (i % separator == 0) { sb.Append("\n"); }
                sb.Append(_tileList[i] + "(" + _tileList[i].Index + ")" + "\t");
            }
            return sb.ToString();
        }

        public sealed class BoardBuilder
        {
            private readonly List<Tile> _boardConfiguration;
            private readonly League _league;

            public BoardBuilder(League league)
            {
                _league = league;
                _boardConfiguration = new List<Tile>(Board.DEFAULT_NUM_TILES);
            }

            public ImmutableList<Tile> BoardConfiguration => _boardConfiguration.ToImmutableList();

            public BoardBuilder SetTile(in Tile tile)
            {
                _boardConfiguration.Add(tile);
                return this;
            }

            public BoardBuilder AppendTile(in Tile tile)
            {
                _boardConfiguration[tile.Index] = tile;
                return this;
            }

            public Player ChoosePlayer(in RedPlayer redPlayer, in BlackPlayer blackPlayer) { return LeagueExtensions.IsBlack(_league) ? blackPlayer : redPlayer; }

            public Board BuildBoard() { return new Board(this); }
        }
    }
}