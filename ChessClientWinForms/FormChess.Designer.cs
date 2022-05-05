namespace ChessClientWinForms
{
    partial class FormChess
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChess));
            this.board = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // board
            // 
            this.board.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.board.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.board.Location = new System.Drawing.Point(23, 22);
            this.board.Name = "board";
            this.board.Size = new System.Drawing.Size(450, 450);
            this.board.TabIndex = 0;
            // 
            // FormChess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 498);
            this.Controls.Add(this.board);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormChess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Windows Chess";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FormChess_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel board;
    }
}

