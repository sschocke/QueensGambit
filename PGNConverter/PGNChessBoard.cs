using PGNChessLib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PGNConverter
{
    public partial class PGNChessBoard : UserControl
    {
        private Brush WhiteTileColor = new SolidBrush(Color.FromArgb(245, 215, 143));
        private Brush BlackTileColor = new SolidBrush(Color.FromArgb(98, 23, 4));
        private Pen SourceTileColor = new Pen(Color.LawnGreen, 5f);
        private Pen DestTileColor = new Pen(Color.CornflowerBlue, 5f);
        private readonly string[] Cols = { "A", "B", "C", "D", "E", "F", "G", "H" };
        private readonly string[] Rows = { "1", "2", "3", "4", "5", "6", "7", "8" };
        private const int paddingSize = 18;


        private int w, h;
        private PGNMove lastMove;

        private Bitmap bmpBoard;
        private PGNBoardState board;

        public PGNChessBoard()
        {
            InitializeComponent();

            board = new PGNBoardState();
        }

        private void PGNChessBoard_Load(object sender, EventArgs e)
        {
            w = (ClientSize.Width - (paddingSize * 2)) / 8;
            h = (ClientSize.Height - (paddingSize * 2)) / 8;
            w = h = Math.Min(w, h);

            this.bmpBoard = new Bitmap(ClientSize.Width, ClientSize.Height);
            Graphics tmp = Graphics.FromImage(this.bmpBoard);
            Font head_font = new Font("Arial", 12);

            for (int c = 0; c < 8; c++)
            {
                string txt = Cols[c];
                SizeF txt_sz = tmp.MeasureString(txt, head_font);
                tmp.DrawString(txt, head_font, Brushes.Black, (c * w) + paddingSize + (w / 2) - (txt_sz.Width / 2), (h * 8) + paddingSize);
                tmp.DrawString(txt, head_font, Brushes.Black, (c * w) + paddingSize + (w / 2) - (txt_sz.Width / 2), 0);
            }

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    tmp.FillRectangle((((r * 9) + c) % 2 == 0 ? WhiteTileColor : BlackTileColor), (c * w) + paddingSize, (r * h) + paddingSize, w, h);
                }

                string txt = Rows[r];
                SizeF txt_sz = tmp.MeasureString(txt, head_font);
                tmp.DrawString(txt, head_font, Brushes.Black, (8 * w) + paddingSize, ((7 - r) * h) + paddingSize + (h / 2) - (txt_sz.Height / 2));
                tmp.DrawString(txt, head_font, Brushes.Black, 0, ((7 - r) * h) + paddingSize + (h / 2) - (txt_sz.Height / 2));
            }

            tmp.Dispose();
            tmp = null;
        }

        public void ResetBoard()
        {
            this.board.ResetBoard();
            Invalidate();
        }

        public void DoMove(PGNMove move)
        {
            lastMove = this.board.DoMove(move);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(this.bmpBoard, ClientRectangle);
            if (lastMove != null)
            {
                int lastSrcRow = lastMove.SourceRank;
                int lastSrcCol = lastMove.SourceFile;
                int lastDestRow = lastMove.DestinationRank;
                int lastDestCol = lastMove.DestinationFile;

                e.Graphics.DrawRectangle(SourceTileColor, (lastSrcCol * w) + paddingSize, ((7 - lastSrcRow) * h) + paddingSize, w, h);
                e.Graphics.DrawRectangle(DestTileColor, (lastDestCol * w) + paddingSize, ((7 - lastDestRow) * h) + paddingSize, w, h);
            }

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    int board_idx = (r * 8) + c;
                    if (board[board_idx] > 0)
                    {
                        e.Graphics.DrawImage(piecesList.Images[board[board_idx] - 1], (c * w) + paddingSize, ((7 - r) * h) + paddingSize, w, h);
                    }
                }
            }

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.OnLoad(e);
            this.Invalidate();
        }
    }
}
