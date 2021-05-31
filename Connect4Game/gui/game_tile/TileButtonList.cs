using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

namespace Connect4Game.gui.game_tile
{
    public sealed class TileButtonList
    {
        private readonly ImmutableList<TileButton> _tileButtons;

        public TileButtonList(in Form1 form, in int col) { _tileButtons = CreateTileButtons(form, col); }

        public ImmutableList<TileButton> GetTileButtons => _tileButtons;
        public void OccupiedTheTileButtonAt(in int index, in Color color) { _tileButtons[index].BackColor = color; }

        public void EmptyAllTile() { _tileButtons.ForEach(button => button.RedrawAsDefaultTile()); }

        private ImmutableList<TileButton> CreateTileButtons(in Form1 form, in int col)
        {
            List<TileButton> tileButtons = new List<TileButton>();

            int max = 6, begin = 0, y = 40, x = 20, index = 0;
            for (int i = begin; i < col; i++)
            {
                tileButtons.Add(new TileButton(form, i + index, x, y));
                x += 70;
                if (i == col - 1)
                {
                    i = -1;
                    begin++;
                    if (begin == max) { break; }
                    y += 70;
                    index += 7;
                    x = 20;
                }
            }
            
            return tileButtons.ToImmutableList();
        }
    }
}