using System.Windows.Forms;
using Connect4Game.engine.piece;
using Connect4Game.engine.player;

namespace Connect4Game.gui.game_setup
{
    public sealed class GameSetupPanel : Form
    {
        private PlayerType _redPlayerType, _blackPlayerType;
        private readonly ComboBox _searchDepthComboBox;
        public GameSetupPanel(in Form1 form1)
        {
            StartPosition = FormStartPosition.CenterParent;
            Name = "Game Setup";
            _redPlayerType = PlayerType.Human;
            _blackPlayerType = PlayerType.Human;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MaximizeBox = false;
            MinimizeBox = false;

            InitializeComponent();
            AddButtonListener(form1);
            _searchDepthComboBox = CreateComboBox();
            
            Controls.Add(_searchDepthComboBox);
        }

        private ComboBox CreateComboBox()
        {
            ComboBox searchDepthComboBox = new ComboBox();
            for (int i = 0; i < 5; i++)
            {
                searchDepthComboBox.Items.Add(i + 1);
            }
            searchDepthComboBox.SelectedIndex = 0;
            return searchDepthComboBox;
        }


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            okButton = new Button();
            cancelButton = new Button();
            blackPlayerBtn = new CheckBox();
            redPlayerBtn = new CheckBox();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.Location = new System.Drawing.Point(28, 106);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.TabIndex = 0;
            okButton.Text = "Ok";
            okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(28, 144);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // blackPlayerBtn
            // 
            blackPlayerBtn.Location = new System.Drawing.Point(12, 36);
            blackPlayerBtn.Name = "blackPlayerBtn";
            blackPlayerBtn.Size = new System.Drawing.Size(104, 24);
            blackPlayerBtn.TabIndex = 2;
            blackPlayerBtn.Text = "Black as AI";
            blackPlayerBtn.UseVisualStyleBackColor = true;
            // 
            // redPlayerBtn
            // 
            redPlayerBtn.Location = new System.Drawing.Point(12, 66);
            redPlayerBtn.Name = "redPlayerBtn";
            redPlayerBtn.Size = new System.Drawing.Size(104, 24);
            redPlayerBtn.TabIndex = 3;
            redPlayerBtn.Text = "Red as AI";
            redPlayerBtn.UseVisualStyleBackColor = true;
            // 
            // GameSetupPanel
            // 
            ClientSize = new System.Drawing.Size(120, 181);
            Controls.Add(redPlayerBtn);
            Controls.Add(blackPlayerBtn);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Name = "GameSetupPanel";
            ResumeLayout(false);
        }

        private CheckBox blackPlayerBtn;
        private CheckBox redPlayerBtn;

        private Button okButton;
        private Button cancelButton;

        private void AddButtonListener(Form1 form1)
        {
            okButton.Click += (sender, args) =>
            {
                if (IsAiPlayer(form1.GetBoard.GetCurrentPlayer) && !form1.IsAIStopped) {
                    form1.SetStopAi(true);
                }
                _blackPlayerType = blackPlayerBtn.Checked ? PlayerType.Computer : PlayerType.Human;
                _redPlayerType = redPlayerBtn.Checked ? PlayerType.Computer : PlayerType.Human;
                form1.FireComputer();
                Visible = false;
            };
            cancelButton.Click += (sender, args) => { Visible = false; };
        }

        public bool IsAiPlayer(in Player player) { return LeagueExtensions.IsBlack(player.GetLeague()) ? _blackPlayerType == PlayerType.Computer : _redPlayerType == PlayerType.Computer; }
        public int GetSearchDepth() { return _searchDepthComboBox.SelectedIndex + 1; }
    }
}