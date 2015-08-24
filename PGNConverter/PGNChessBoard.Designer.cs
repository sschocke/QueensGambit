using System;

namespace PGNConverter
{
    partial class PGNChessBoard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PGNChessBoard));
            this.piecesList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // piecesList
            // 
            this.piecesList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("piecesList.ImageStream")));
            this.piecesList.TransparentColor = System.Drawing.Color.Transparent;
            this.piecesList.Images.SetKeyName(0, "PawnWhite.png");
            this.piecesList.Images.SetKeyName(1, "KnightWhite.png");
            this.piecesList.Images.SetKeyName(2, "BishopWhite.png");
            this.piecesList.Images.SetKeyName(3, "RookWhite.png");
            this.piecesList.Images.SetKeyName(4, "QueenWhite.png");
            this.piecesList.Images.SetKeyName(5, "KingWhite.png");
            this.piecesList.Images.SetKeyName(6, "PawnBlack.png");
            this.piecesList.Images.SetKeyName(7, "KnightBlack.png");
            this.piecesList.Images.SetKeyName(8, "BishopBlack.png");
            this.piecesList.Images.SetKeyName(9, "RookBlack.png");
            this.piecesList.Images.SetKeyName(10, "QueenBlack.png");
            this.piecesList.Images.SetKeyName(11, "KingBlack.png");
            // 
            // PGNChessBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "PGNChessBoard";
            this.Size = new System.Drawing.Size(484, 385);
            this.Load += new System.EventHandler(this.PGNChessBoard_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ImageList piecesList;
    }
}
