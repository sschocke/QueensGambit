using PGNChessLib;
using QueensGambit.NNDataLib;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGNConverter
{
    public partial class frmMain : Form
    {
        public static PGNDatabase Source;
        BindingSource gamesSource;
        IOrderedEnumerable<PGNGame> wantedGames;

        public frmMain()
        {
            InitializeComponent();
            gamesSource = new BindingSource();
            gamesSource.DataSource = typeof(PGNGame);
            gridGames.DataSource = gamesSource;
        }

        private void btnOpenPGN_Click(object sender, EventArgs e)
        {
            if (openPGNDialog.ShowDialog(this) == DialogResult.OK)
            {
                lblPGNFileName.Font = new Font(lblPGNFileName.Font, FontStyle.Regular);
                lblPGNFileName.Text = openPGNDialog.FileName;
                btnOpenPGN.Enabled = false;
                btnRunAllGames.Enabled = true;

                using (StreamReader reader = File.OpenText(openPGNDialog.FileName))
                {
                    Source = new PGNDatabase(reader);
                }

                wantedGames = from PGNGame game in Source
                              where ((game.BlackPlayer.StartsWith("Kasparov") == true && game.Result == PGNGameResult.BLACK_WINS) ||
                              (game.WhitePlayer.StartsWith("Kasparov") == true && game.Result == PGNGameResult.WHITE_WINS)) &&
                              game.Moves.Count > 0
                              orderby game.Result descending
                              select game;

                gamesSource.DataSource = wantedGames;
            }
        }

        private void gridGames_SelectionChanged(object sender, EventArgs e)
        {
            lbMoves.DataSource = ((PGNGame)(gamesSource.Current)).Moves;
        }

        private void lbMoves_SelectedIndexChanged(object sender, EventArgs e)
        {
            PGNGame curGame = gamesSource.Current as PGNGame;
            if (curGame == null) { return; }

            PGNMove selectedMove = lbMoves.SelectedItem as PGNMove;
            if (selectedMove == null) { return; }

            pgnChessBoard1.ResetBoard();
            foreach (PGNMove move in curGame.Moves)
            {
                pgnChessBoard1.DoMove(move);
                if (move == selectedMove) break;
            }
        }

        private void btnRunAllGames_Click(object sender, EventArgs e)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
#if DEBUG
            foreach (PGNGame game in wantedGames.Take(1))
            {
                RunGame(game);
            }
#else
            Parallel.ForEach<PGNGame>(wantedGames, game =>
            {
                RunGame(game);
            });
#endif
            timer.Stop();
            Debug.WriteLine("Run All Games took: " + timer.Elapsed.ToString());

            MessageBox.Show("All Games Run!");
        }

        private static void RunGame(PGNGame game)
        {
            PGNBoardState board = new PGNBoardState();
            foreach (PGNMove move in game.Moves)
            {
                // Get input board state
                byte[] boardState = new byte[64];
                for( int i=0; i<64; i++)
                {
                    if (board[i] != 0) boardState[i] = convertPieceToNNDataID(board[i]);
                }

                ChessBoardInputState inState = new ChessBoardInputState(boardState, 0, 0, 0);

                board.DoMove(move);
            }
        }

        private static byte convertPieceToNNDataID(int piece)
        {
            switch (piece)
            {
                case PGNBoardState.WHITE_PAWN:
                    return ChessBoardInputState.PAWN | ChessBoardInputState.WHITE;
                case PGNBoardState.WHITE_KNIGHT:
                    return ChessBoardInputState.KNIGHT | ChessBoardInputState.WHITE;
                case PGNBoardState.WHITE_BISHOP:
                    return ChessBoardInputState.BISHOP | ChessBoardInputState.WHITE;
                case PGNBoardState.WHITE_ROOK:
                    return ChessBoardInputState.ROOK | ChessBoardInputState.WHITE;
                case PGNBoardState.WHITE_QUEEN:
                    return ChessBoardInputState.QUEEN | ChessBoardInputState.WHITE;
                case PGNBoardState.WHITE_KING:
                    return ChessBoardInputState.KING | ChessBoardInputState.WHITE;
                case PGNBoardState.BLACK_PAWN:
                    return ChessBoardInputState.PAWN | ChessBoardInputState.BLACK;
                case PGNBoardState.BLACK_KNIGHT:
                    return ChessBoardInputState.KNIGHT | ChessBoardInputState.BLACK;
                case PGNBoardState.BLACK_BISHOP:
                    return ChessBoardInputState.BISHOP | ChessBoardInputState.BLACK;
                case PGNBoardState.BLACK_ROOK:
                    return ChessBoardInputState.ROOK | ChessBoardInputState.BLACK;
                case PGNBoardState.BLACK_QUEEN:
                    return ChessBoardInputState.QUEEN | ChessBoardInputState.BLACK;
                case PGNBoardState.BLACK_KING:
                    return ChessBoardInputState.KING | ChessBoardInputState.BLACK;
                default:
                    throw new ArgumentOutOfRangeException(nameof(piece), "Not a valid Piece ID");
            }
        }
    }
}
