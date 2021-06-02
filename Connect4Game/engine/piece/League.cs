using System;

namespace Connect4Game.engine.piece
{
    public enum League { Red, Black, Empty }
    public static class LeagueExtensions
    {
        public static bool IsBlack(in League league)
        {
            if (league == League.Empty) { throw new ArgumentException("League cannot be of EMPTY"); }
            return league == League.Black;
        }
    }
}