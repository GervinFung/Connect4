using System;
using System.ComponentModel;
using System.Windows.Forms;
using Connect4Game.engine.board;
using Connect4Game.gui.ai_worker;
using Connect4Game.gui.game_setup;
using Connect4Game.gui.game_tile;

namespace Connect4Game.gui
{
    public enum PlayerType { Human, Computer }
    public sealed partial class Form1 : Form
    {
        private Board _board;
        private bool _endGame, _isAiThinking, _stopAi;
        private readonly TileButtonList _tileButtonList;
        private readonly GameSetupPanel _gameSetupPanel;
        public Form1()
        {
            _endGame = false;
            _isAiThinking = false;
            _stopAi = true;
            _board = Board.CreateStandardBoard();
            _tileButtonList = new TileButtonList(this, _board.NumCol);
            
            InitializeComponent();
            AddTileButtons(_tileButtonList.GetTileButtons);
            AddListenerToMenu();

            _gameSetupPanel = new GameSetupPanel(this);
        }

        public Board GetBoard => _board;
        public bool IsEndGame => _endGame;
        public bool IsAIThinking => _isAiThinking;
        public bool IsAIStopped => _stopAi;
        public int GetSearchDepth() { return _gameSetupPanel.GetSearchDepth(); }

        public void SetAiThinking(in bool isAiThinking) { _isAiThinking = isAiThinking; }
        public void SetStopAi(in bool stopAi) { _stopAi = stopAi; }
        public void UpdateBoard(in Board board) { _board = board; }
        public TileButtonList GetTileButtonList => _tileButtonList;

        public void FireComputer()
        {
            if (_gameSetupPanel.IsAiPlayer(_board.GetCurrentPlayer) && !_board.GetCurrentPlayer.IsInCheckmate() && !_board.GetCurrentPlayer.IsStalemate() && !_isAiThinking)
            {
                Invoke((Action)(() => { new AiWorker(this).RunWorkerAsync(); }));
            }
            DisplayEndgameMessage();
        }

        private void AddListenerToMenu()
        {
            newGameToolStripMenuItem.Click += (sender, args) =>
            {
                DialogResult res = MessageBox.Show("Are you sure you want to start a New Game?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.OK)
                {
                    _endGame = false;
                    _isAiThinking = false;
                    _stopAi = true;
                    _board = Board.CreateStandardBoard();
                    _tileButtonList.EmptyAllTile();
                }
            };
            exitGameToolStripMenuItem.Click += (sender, args) =>
            {
                if (ConfirmClose())
                {
                    Application.Exit();
                    Environment.Exit(0);
                }
            };
            gameSetupToolStripMenuItem.Click += (sender, args) =>
            {
                _gameSetupPanel.ShowDialog(this);
            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (ConfirmClose())
            {
                Application.Exit();
                Environment.Exit(0);
            }
            e.Cancel = true;
        }

        private bool ConfirmClose()
        {
            return DialogResult.OK == MessageBox.Show("Are you sure you want to Exit?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        public void DisplayEndgameMessage()
        {
            if (_endGame)
            {
                MessageBox.Show("Start a new game to continue");
                return;
            }
            if (_board.GetCurrentPlayer.IsInCheckmate())
            {
                _endGame = true;
                MessageBox.Show(_board.GetCurrentPlayer.GetOpponent() + " has won!");
            }

            if (_board.GetCurrentPlayer.IsStalemate())
            {
                _endGame = true;
                MessageBox.Show("Game is drawn!");
            }
        }
    }
}