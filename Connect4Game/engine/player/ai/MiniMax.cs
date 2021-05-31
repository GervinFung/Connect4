using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Connect4Game.engine.board;
using Connect4Game.engine.move;
using Connect4Game.engine.piece;

namespace Connect4Game.engine.player.ai
{
    public class MiniMax
    {
        private readonly int _depth;
        private readonly Evaluator _evaluator;
        private readonly League _aiLeague;
        private static readonly ImmutableList<int> POSITION_EVAL = GeneratePositionEval();

        private static ImmutableList<int> GeneratePositionEval()
        {
            int[] posEval =
            {
                3, 4, 5, 7, 5, 4, 3,
                4, 6, 8, 10, 8, 6, 4,
                5, 8, 11, 13, 11, 8, 5,
                5, 8, 11, 13, 11, 8, 5,
                4, 6, 8, 10, 8, 6, 4,
                3, 4, 5, 7, 5, 4, 3
            };
            return posEval.ToImmutableList();
        }

        public MiniMax(in League aiLeague, in int depth)
        {
            _aiLeague = aiLeague;
            _depth = depth - (depth % 2 == 1 ? 1 : 0);
            _evaluator = new StandardEval();
        }

        private static ImmutableList<Move> MoveSorter(in ImmutableList<Move> legalMoves)
        {
            List<Move> sortedList = new List<Move>();
            Dictionary<Move, int> moveScore = new Dictionary<Move, int>();
            foreach (Move move in legalMoves) { moveScore[move] = POSITION_EVAL[move.GetIndex()]; }
            
            List<KeyValuePair<Move, int>> myList = moveScore.ToList();

            myList.Sort(
                delegate(KeyValuePair<Move, int> pair1, KeyValuePair<Move, int> pair2) { return pair1.Value.CompareTo(pair2.Value) * -1; }
            );

            foreach (KeyValuePair<Move, int> entry in myList) { sortedList.Add(entry.Key); }
            return sortedList.ToImmutableList();
        }

        public Move MakeMove(Board board)
        {
            Player currentPlayer = board.GetCurrentPlayer;
            int highestSeenValue = Int32.MinValue, lowestSeenValue = Int32.MaxValue;

            ImmutableList<Move> legalMove = MoveSorter(currentPlayer.GetLegalMoves);
            Move bestMove = legalMove[0];
            
            foreach (Move move in legalMove)
            {
                Board tempBoard = currentPlayer.MakeMove(move);
                if (tempBoard.IsWin()) { return move; }
                int currentVal = LeagueExtensions.IsBlack(currentPlayer.GetLeague()) ?
                    Min(tempBoard, _depth - 1, highestSeenValue, lowestSeenValue) :
                    Max(tempBoard, _depth - 1, highestSeenValue, lowestSeenValue);
                if (!LeagueExtensions.IsBlack(currentPlayer.GetLeague()) && currentVal > highestSeenValue) {
                    highestSeenValue = currentVal;
                    bestMove = move;
                }
                else if (LeagueExtensions.IsBlack(currentPlayer.GetLeague()) && currentVal < lowestSeenValue) {
                    lowestSeenValue = currentVal;
                    bestMove = move;
                }
            }

            return bestMove;
        }
        
        private int Min(in Board board, in int depth, in int highestValue, in int lowestValue)
        {
            if (depth == 0) { return _evaluator.EvaluateBoard(board, _aiLeague); }
            int currentLowest = lowestValue;
            foreach (Move move in MoveSorter(board.GetCurrentPlayer.GetLegalMoves)) 
            {
                Board tempBoard = board.GetCurrentPlayer.MakeMove(move);
                if (tempBoard.IsWin()) { return Int32.MaxValue; }
                currentLowest = Math.Min(currentLowest, Max(tempBoard, depth - 1, highestValue, currentLowest));
                if (currentLowest <= highestValue) {
                    return highestValue;
                }
            }
            return currentLowest;
        }

        private int Max(in Board board, in int depth, in int highestValue, in int lowestValue)
        {
            if (depth == 0) { return _evaluator.EvaluateBoard(board, _aiLeague); }
            int currentHighest = highestValue;
            foreach (Move move in MoveSorter(board.GetCurrentPlayer.GetLegalMoves)) 
            {
                Board tempBoard = board.GetCurrentPlayer.MakeMove(move);
                if (tempBoard.IsWin()) { return Int32.MinValue; }
                currentHighest = Math.Max(currentHighest, Min(tempBoard, depth - 1, currentHighest, lowestValue));
                if (currentHighest >= lowestValue) {
                    return lowestValue;
                }
            }
            return currentHighest;
        }
    }
}