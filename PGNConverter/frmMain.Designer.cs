namespace PGNConverter
{
    partial class frmMain
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
            this.btnOpenPGN = new System.Windows.Forms.Button();
            this.lblPGNFileName = new System.Windows.Forms.Label();
            this.openPGNDialog = new System.Windows.Forms.OpenFileDialog();
            this.gridGames = new System.Windows.Forms.DataGridView();
            this.lbMoves = new System.Windows.Forms.ListBox();
            this.pgnChessBoard1 = new PGNConverter.PGNChessBoard();
            this.btnRunAllGames = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridGames)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenPGN
            // 
            this.btnOpenPGN.Location = new System.Drawing.Point(12, 12);
            this.btnOpenPGN.Name = "btnOpenPGN";
            this.btnOpenPGN.Size = new System.Drawing.Size(75, 23);
            this.btnOpenPGN.TabIndex = 0;
            this.btnOpenPGN.Text = "Open PGN";
            this.btnOpenPGN.UseVisualStyleBackColor = true;
            this.btnOpenPGN.Click += new System.EventHandler(this.btnOpenPGN_Click);
            // 
            // lblPGNFileName
            // 
            this.lblPGNFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPGNFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPGNFileName.Location = new System.Drawing.Point(93, 12);
            this.lblPGNFileName.Name = "lblPGNFileName";
            this.lblPGNFileName.Size = new System.Drawing.Size(388, 23);
            this.lblPGNFileName.TabIndex = 1;
            this.lblPGNFileName.Text = "(Please select a PGN database file to analyze...)";
            this.lblPGNFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openPGNDialog
            // 
            this.openPGNDialog.DefaultExt = "pgn";
            this.openPGNDialog.Filter = "PGN Files|*.pgn|All FIles|*.*";
            this.openPGNDialog.Title = "Please select a PGN Database file...";
            // 
            // gridGames
            // 
            this.gridGames.AllowUserToAddRows = false;
            this.gridGames.AllowUserToDeleteRows = false;
            this.gridGames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridGames.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridGames.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridGames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGames.Location = new System.Drawing.Point(12, 41);
            this.gridGames.Name = "gridGames";
            this.gridGames.ReadOnly = true;
            this.gridGames.RowHeadersVisible = false;
            this.gridGames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGames.ShowEditingIcon = false;
            this.gridGames.Size = new System.Drawing.Size(584, 150);
            this.gridGames.TabIndex = 3;
            this.gridGames.SelectionChanged += new System.EventHandler(this.gridGames_SelectionChanged);
            // 
            // lbMoves
            // 
            this.lbMoves.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMoves.FormattingEnabled = true;
            this.lbMoves.Location = new System.Drawing.Point(389, 192);
            this.lbMoves.Name = "lbMoves";
            this.lbMoves.Size = new System.Drawing.Size(208, 368);
            this.lbMoves.TabIndex = 4;
            this.lbMoves.SelectedIndexChanged += new System.EventHandler(this.lbMoves_SelectedIndexChanged);
            // 
            // pgnChessBoard1
            // 
            this.pgnChessBoard1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgnChessBoard1.Location = new System.Drawing.Point(12, 197);
            this.pgnChessBoard1.Name = "pgnChessBoard1";
            this.pgnChessBoard1.Size = new System.Drawing.Size(377, 363);
            this.pgnChessBoard1.TabIndex = 5;
            // 
            // btnRunAllGames
            // 
            this.btnRunAllGames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunAllGames.Enabled = false;
            this.btnRunAllGames.Location = new System.Drawing.Point(521, 12);
            this.btnRunAllGames.Name = "btnRunAllGames";
            this.btnRunAllGames.Size = new System.Drawing.Size(75, 23);
            this.btnRunAllGames.TabIndex = 6;
            this.btnRunAllGames.Text = "Run All";
            this.btnRunAllGames.UseVisualStyleBackColor = true;
            this.btnRunAllGames.Click += new System.EventHandler(this.btnRunAllGames_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 572);
            this.Controls.Add(this.btnRunAllGames);
            this.Controls.Add(this.pgnChessBoard1);
            this.Controls.Add(this.lbMoves);
            this.Controls.Add(this.gridGames);
            this.Controls.Add(this.lblPGNFileName);
            this.Controls.Add(this.btnOpenPGN);
            this.Name = "frmMain";
            this.Text = "PGN Conversion Utility";
            ((System.ComponentModel.ISupportInitialize)(this.gridGames)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenPGN;
        private System.Windows.Forms.Label lblPGNFileName;
        private System.Windows.Forms.OpenFileDialog openPGNDialog;
        private System.Windows.Forms.DataGridView gridGames;
        private System.Windows.Forms.ListBox lbMoves;
        private PGNChessBoard pgnChessBoard1;
        private System.Windows.Forms.Button btnRunAllGames;
    }
}

