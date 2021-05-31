using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using Connect4Game.gui.game_tile;

namespace Connect4Game.gui
{
    sealed partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GameMenuBar = new MenuStrip();
            this.gameToolStripMenuItem = new ToolStripMenuItem();
            this.newGameToolStripMenuItem = new ToolStripMenuItem();
            this.exitGameToolStripMenuItem = new ToolStripMenuItem();
            this.gameSetupToolStripMenuItem = new ToolStripMenuItem();
            this.GameMenuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // GameMenuBar
            // 
            this.GameMenuBar.BackColor = Color.White;
            this.GameMenuBar.Items.AddRange(new ToolStripItem[] {this.gameToolStripMenuItem, this.gameSetupToolStripMenuItem});
            this.GameMenuBar.Location = new Point(0, 0);
            this.GameMenuBar.Name = "GameMenuBar";
            this.GameMenuBar.Size = new Size(484, 24);
            this.GameMenuBar.TabIndex = 0;
            this.GameMenuBar.Text = "GameMenuBar";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {this.newGameToolStripMenuItem, this.exitGameToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new Size(132, 22);
            this.newGameToolStripMenuItem.Text = "New Game";
            // 
            // exitGameToolStripMenuItem
            // 
            this.exitGameToolStripMenuItem.Name = "exitGameToolStripMenuItem";
            this.exitGameToolStripMenuItem.Size = new Size(132, 22);
            this.exitGameToolStripMenuItem.Text = "Exit Game";
            // 
            // gameSetupToolStripMenuItem
            // 
            this.gameSetupToolStripMenuItem.Name = "gameSetupToolStripMenuItem";
            this.gameSetupToolStripMenuItem.Size = new Size(83, 20);
            this.gameSetupToolStripMenuItem.Text = "Game Setup";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.FromArgb(((int) (((byte) (40)))), ((int) (((byte) (42)))), ((int) (((byte) (54)))));
            this.ClientSize = new Size(484, 461);
            this.Controls.Add(this.GameMenuBar);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.GameMenuBar;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Name = "Form1";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Connect4";
            this.GameMenuBar.ResumeLayout(false);
            this.GameMenuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void AddTileButtons(in ImmutableList<TileButton> tileButtons) { tileButtons.ForEach(tileButton => { this.Controls.Add(tileButton); }); }

        private void RemoveTileButtons(in ImmutableList<TileButton> tileButtons) { tileButtons.ForEach(tileButton => { this.Controls.Remove(tileButton); }); }

        private MenuStrip GameMenuBar;

        private ToolStripMenuItem newGameToolStripMenuItem;
        private ToolStripMenuItem exitGameToolStripMenuItem;
        
        private ToolStripMenuItem gameToolStripMenuItem;
        private ToolStripMenuItem gameSetupToolStripMenuItem;

        #endregion
    }
}