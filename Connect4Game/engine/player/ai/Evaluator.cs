using System.Collections.Generic;
using System.Collections.Immutable;
using Connect4Game.engine.board;
using Connect4Game.engine.piece;

namespace Connect4Game.engine.player.ai
{
    public abstract class Evaluator
    {
        public abstract int EvaluateBoard(in Board board, in League league);
    }
    
    public sealed class StandardEval : Evaluator
    {
        public override int EvaluateBoard(in Board board, in League league)
        {
            League currentLeague = LeagueExtensions.IsBlack(league)
                ? board.GetCurrentPlayer.GetOpponent().GetLeague()
                : board.GetCurrentPlayer.GetLeague();
            League enemyLeague = LeagueExtensions.IsBlack(league)
                ? board.GetCurrentPlayer.GetLeague()
                : board.GetCurrentPlayer.GetOpponent().GetLeague();
            return NumberOfConsecutivePieceEval(board, currentLeague, enemyLeague);
        }
        private static int NumberOfConsecutivePieceEval(in Board board, in League currentLeague, in League enemyLeague)
        {
            int score = 0;

            foreach (ImmutableList<League> eLeagues in GetAllVerticalColumn(board))
            {
                int currentTile = 0, emptyTile = 0, enemyTile = 0;
                int begin = 0, max = 3;
                int maxRun = 3, i = 0;
                for (int j = begin; j <= max; j++)
                {
                    if (eLeagues[j] == currentLeague) { currentTile++; }
                    else if (eLeagues[j] == enemyLeague) { enemyTile++; }
                    else if (eLeagues[j] == League.Empty) { emptyTile++; }
            
                    if (j == max)
                    {
                        j = begin;
                        i++;
                        score += ComputeScore(currentTile, enemyTile, emptyTile);
                        if (i == maxRun) { break; }
                        begin++;
                        max++;
                        currentTile = 0;
                        emptyTile = 0;
                        enemyTile = 0;
                    }
                }
            }

            
            foreach (ImmutableList<League> eLeagues in GetAllHorizontalRow(board))
            {
                int begin = 0, max = 3;
                int currentTile = 0;
                int emptyTile = 0;
                int enemyTile = 0;
                int maxRun = 4, i = 0;
                for (int j = begin; j <= max; j++)
                {
                    if (eLeagues[j] == currentLeague) { currentTile++; }
                    else if (eLeagues[j] == enemyLeague) { enemyTile++; }
                    else if (eLeagues[j] == League.Empty) { emptyTile++; }
                        
                    if (j == max)
                    {
                        j = begin;
                        i++;
                        score += ComputeScore(currentTile, enemyTile, emptyTile);
                        if (i == maxRun) { break; }
                        begin++;
                        max++;
                        currentTile = 0;
                        emptyTile = 0;
                        enemyTile = 0;
                    }
                }
            }
            
            
            foreach (ImmutableList<League> eLeagues in GetPositiveSlopeRow(board))
            {
                int begin = 0, max = 3;
                int currentTile = 0, emptyTile = 0, enemyTile = 0;
                int maxRun = eLeagues.Count - Board.DEFAULT_WIN_NUM_TILES, i = 0;
                for (int j = begin; j <= max; j++)
                {
                    if (eLeagues[j] == currentLeague) { currentTile++; }
                    else if (eLeagues[j] == enemyLeague) { enemyTile++; }
                    else if (eLeagues[j] == League.Empty) { emptyTile++; }
                
                    if (j == max)
                    {
                        score += ComputeScore(currentTile, enemyTile, emptyTile);
                        if (i == maxRun) { break; }
                        i++;
                        j = begin;
                        begin++;
                        max++;
                        currentTile = 0;
                        emptyTile = 0;
                        enemyTile = 0;
                    }
                }
                
            }
            
            
            foreach (ImmutableList<League> eLeagues in GetNegativeSlopeRow(board))
            {
                int begin = 0, max = 3;
                int currentTile = 0, emptyTile = 0, enemyTile = 0;
                int maxRun = eLeagues.Count - Board.DEFAULT_WIN_NUM_TILES, i = 0;
                for (int j = begin; j <= max; j++)
                {
                    if (eLeagues[j] == currentLeague) { currentTile++; }
                    else if (eLeagues[j] == enemyLeague) { enemyTile++; }
                    else if (eLeagues[j] == League.Empty) { emptyTile++; }
                        
                    if (j == max)
                    {
                        score += ComputeScore(currentTile, enemyTile, emptyTile);
                        if (i == maxRun) { break; }
                        i++;
                        j = begin;
                        begin++;
                        max++;
                        currentTile = 0;
                        emptyTile = 0;
                        enemyTile = 0;
                    }
                }
            }

            return score;
        }

        private static int ComputeScore(in int currentTile, in int enemyTile, in int emptyTile)
        {
            if (currentTile == 1 && emptyTile == 3) { return 1; }
            if (currentTile == 2 && emptyTile == 2) { return 5; }
            if (currentTile == 3 && emptyTile == 1) { return 1000; }
            if (currentTile == 4 && emptyTile == 0) { return 100000; }

            if (emptyTile == 4 && emptyTile == 0) { return -2000000; }
            if (enemyTile == 3 && emptyTile == 1) { return -1000000; }
            if (enemyTile == 2 && emptyTile == 2) { return -10000; }
            if (enemyTile == 1 && emptyTile == 3) { return -100; }
            if (enemyTile > currentTile) { return -100; }

            if (currentTile > enemyTile) { return 100; }

            return 0;
        }

        private static ImmutableList<ImmutableList<League>> GetAllHorizontalRow(in Board board)
        {
            List<ImmutableList<League>> listOfLeagues = new List<ImmutableList<League>>();
            int begin = 0;
            List<League> leagues = new List<League>();
            for (int i = begin; i < board.NumTiles; i++)
            {
                Tile tile = board.GetTileAt(i);
                leagues.Add(tile.IsTileOccupied() ? tile.GetPiece().GetLeague : League.Empty);
                if (i - begin == 6)
                {
                    begin += board.NumCol;
                    listOfLeagues.Add(leagues.ToImmutableList());
                    leagues = new List<League>();
                }
            }

            return listOfLeagues.ToImmutableList();
        }

        private static ImmutableList<ImmutableList<League>> GetAllVerticalColumn(in Board board)
        {
            List<ImmutableList<League>> listOfLeagues = new List<ImmutableList<League>>();
            int begin = 0, max = board.NumCol * 5;
            List<League> leagues = new List<League>();
            for (int i = begin; i <= max; i += board.NumCol)
            {
                Tile tile = board.GetTileAt(i);
                leagues.Add(tile.IsTileOccupied() ? tile.GetPiece().GetLeague : League.Empty);
                if (i == max)
                {
                    listOfLeagues.Add(leagues.ToImmutableList());
                    leagues = new List<League>();
                    if (max < Board.DEFAULT_NUM_TILES - 1)
                    {
                        begin++;
                        i = begin - board.NumCol;
                        max++;
                    }
                }
            }

            return listOfLeagues.ToImmutableList();
        }

        private static ImmutableList<ImmutableList<League>> GetPositiveSlopeRow(in Board board)
        {
            List<ImmutableList<League>> listOfLeagues = new List<ImmutableList<League>>();
            int increment = board.NumCol - 1, begin = (board.NumCol - 1) / 2;
            int max = 21;
            bool goEdge = false;
            List<League> leagues = new List<League>();
            for (int i = begin; i <= max; i += increment)
            {
                Tile tile = board.GetTileAt(i);
                leagues.Add(tile.IsTileOccupied() ? tile.GetPiece().GetLeague : League.Empty);
                if (i == max)
                {
                    listOfLeagues.Add(leagues.ToImmutableList());
                    leagues = new List<League>();
                    
                    if (begin == 20) { break; }
                    if (begin == increment && !goEdge) { goEdge = true; }

                    begin = goEdge ? begin + board.NumCol : begin + 1;
                    max = max + board.NumCol >= board.NumTiles ? max + 1 : max + board.NumCol;
                    i = begin - increment;
                }
            }

            return listOfLeagues.ToImmutableList();
        }

        private static ImmutableList<ImmutableList<League>> GetNegativeSlopeRow(in Board board)
        {
            List<ImmutableList<League>> listOfLeagues = new List<ImmutableList<League>>();
            int increment = board.NumCol + 1, begin = (board.NumCol - 1) / 2;
            int max = 27;
            bool goEdge = false;
            List<League> leagues = new List<League>();
            for (int i = begin; i <= max; i += increment)
            {
                Tile tile = board.GetTileAt(i);
                leagues.Add(tile.IsTileOccupied() ? tile.GetPiece().GetLeague : League.Empty);
                if (i == max)
                {
                    listOfLeagues.Add(leagues.ToImmutableList());
                    leagues = new List<League>();
                    
                    if (begin == 14) { break; }
                    if (begin == 0 && !goEdge) { goEdge = true; }

                    begin = goEdge ? begin + board.NumCol : begin - 1;
                    max = max + board.NumCol >= board.NumTiles ? max - 1 : max + board.NumCol;
                    i = begin - increment;
                }
            }

            return listOfLeagues.ToImmutableList();
        }
    }
}