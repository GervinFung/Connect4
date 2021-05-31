using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Connect4Game.engine.move;
using Connect4Game.engine.piece;
using Connect4Game.engine.player.ai;

namespace Connect4Game.gui.ai_worker
{
    public sealed class AiWorker : BackgroundWorker
    {
        private readonly Form1 _form1;
        private readonly MiniMax _miniMax;

        public AiWorker(in Form1 form1)
        {
            _form1 = form1;
            _miniMax = new MiniMax(form1.GetBoard.GetCurrentPlayer.GetLeague(), form1.GetSearchDepth() + 3);
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            _form1.SetAiThinking(true);
            _form1.SetStopAi(false);
            e.Result = _miniMax.MakeMove(_form1.GetBoard);
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                return;
            }
            Move bestMove = (Move)e.Result;
            if (bestMove == null) { throw new ArgumentException("Move from AI is null"); }

            if (!_form1.IsAIStopped)
            {
                _form1.GetTileButtonList.OccupiedTheTileButtonAt(bestMove.GetIndex(), LeagueExtensions.IsBlack(_form1.GetBoard.GetCurrentPlayer.GetLeague()) ? Color.Black : Color.Crimson);
                _form1.UpdateBoard(bestMove.ExecuteMove());
            }
            _form1.SetAiThinking(false);
            _form1.SetStopAi(true);
            _form1.FireComputer();
        }
    }
}