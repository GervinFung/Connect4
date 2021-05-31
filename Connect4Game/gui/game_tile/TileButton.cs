using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Connect4Game.engine.move;
using Connect4Game.engine.piece;

namespace Connect4Game.gui.game_tile
{
    public sealed class TileButton : Button
    {
        private static readonly int GAME_BUTTON_SIZE = 50;
        private static readonly Color HOVER_BLACK = Color.FromArgb(18, 18, 18);
        private static readonly Color HOVER_RED = Color.Red;
        private static readonly Color DEFAULT = Color.FromArgb(68, 71, 90);

        public TileButton(Form1 form, int index, in int x, in int y)
        {
            Location = new Point(x, y);
            Height = GAME_BUTTON_SIZE;
            Width = GAME_BUTTON_SIZE;
            Margin = new Padding(20, 20, 20, 20);
            TabStop = false;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            ForeColor = Color.Transparent;
            BackColor = DEFAULT;
            int col = index % form.GetBoard.NumCol;
            Click += (sender, args) =>
            {
                if (sender == null) { throw new ArgumentException("Sender cannot be null"); }
                TileButton tileButton = sender as TileButton;
                if (tileButton == null) { throw new ArgumentException("Sender is not type of TileButton"); }
                bool aiNotFunction = form.IsAIStopped || !form.IsAIThinking;
                if (!form.IsEndGame && aiNotFunction)
                {
                    foreach (Move move in form.GetBoard.GetCurrentPlayer.GetLegalMoves)
                    {
                        if (move.GetIndex() % form.GetBoard.NumCol == col)
                        {
                            form.GetTileButtonList.OccupiedTheTileButtonAt(move.GetIndex(), LeagueExtensions.IsBlack(form.GetBoard.GetCurrentPlayer.GetLeague()) ? Color.Black : Color.FromArgb(191, 10, 18));
                            form.UpdateBoard(form.GetBoard.GetCurrentPlayer.MakeMove(move));
                            form.FireComputer();
                            return;
                        }
                    }
                }
                form.DisplayEndgameMessage();
            };

            MouseEnter += (sender, args) =>
            {
                if (sender == null) { throw new ArgumentException("Sender cannot be null"); }
                TileButton tileButton = sender as TileButton;
                if (tileButton == null) { throw new ArgumentException("Sender is not type of TileButton"); }
                if (BackColor.Equals(DEFAULT))
                {
                    BackColor = LeagueExtensions.IsBlack(form.GetBoard.GetCurrentPlayer.GetLeague()) ? HOVER_BLACK : HOVER_RED;
                }
            };
            
            MouseLeave += (sender, args) =>
            {
                if (sender == null) { throw new ArgumentException("Sender cannot be null"); }
                TileButton tileButton = sender as TileButton;
                if (tileButton == null) { throw new ArgumentException("Sender is not type of TileButton"); }
                if (BackColor.Equals(HOVER_BLACK) || BackColor.Equals(HOVER_RED)) { BackColor = DEFAULT; }
            };
        }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            GraphicsPath graphics = new GraphicsPath();
            graphics.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            Region = new Region(graphics);
            base.OnPaint(paintEventArgs);
        }

        public void RedrawAsDefaultTile() { base.BackColor = DEFAULT; }

        protected override bool ShowFocusCues => false;
    }
}