using System;
using System.Collections.Generic;
using System.Linq;

namespace PGNChessLib
{
    public class PGNBoardState
    {
        private readonly int[] knightMovesOffsets = { 33, 18, -14, -31, -33, -18, 14, 31 };
        private readonly int[] kingMovesOffsets = { -1, 15, 16, 17, 1, -15, -16, -17 };

        public const int WHITE_PAWN = 1;
        public const int WHITE_KNIGHT = 2;
        public const int WHITE_BISHOP = 3;
        public const int WHITE_ROOK = 4;
        public const int WHITE_QUEEN = 5;
        public const int WHITE_KING = 6;
        public const int BLACK_PAWN = 7;
        public const int BLACK_KNIGHT = 8;
        public const int BLACK_BISHOP = 9;
        public const int BLACK_ROOK = 10;
        public const int BLACK_QUEEN = 11;
        public const int BLACK_KING = 12;

        private byte[] board;
        private bool canWhiteCastleKingside, canBlackCastleKingside;
        private bool canWhiteCastleQueenside, canBlackCastleQueenside;
        private int enPassantPawn = -1;

        public PGNPlayer CurrentPlayer { get; private set; } = PGNPlayer.WHITE;

        public PGNBoardState()
        {
            board = new byte[64];
            this.ResetBoard();
        }
        public PGNBoardState(PGNBoardState src)
        {
            board = new byte[64];
            for (int i = 0; i < 64; i++)
            {
                board[i] = src[i];
            }

            canWhiteCastleKingside = src.canWhiteCastleKingside;
            canBlackCastleKingside = src.canBlackCastleKingside;
            canWhiteCastleQueenside = src.canWhiteCastleQueenside;
            canBlackCastleQueenside = src.canBlackCastleQueenside;
            enPassantPawn = src.enPassantPawn;
            CurrentPlayer = src.CurrentPlayer;
        }

        public byte this[int boardIdx]
        {
            get { return board[boardIdx]; }
        }

        public void ResetBoard()
        {
            for (int b = 16; b < 48; b++)
            {
                board[b] = 0;
            }

            board[63] = BLACK_ROOK;
            board[62] = BLACK_KNIGHT;
            board[61] = BLACK_BISHOP;
            board[60] = BLACK_KING;
            board[59] = BLACK_QUEEN;
            board[58] = BLACK_BISHOP;
            board[57] = BLACK_KNIGHT;
            board[56] = BLACK_ROOK;
            board[55] = BLACK_PAWN;
            board[54] = BLACK_PAWN;
            board[53] = BLACK_PAWN;
            board[52] = BLACK_PAWN;
            board[51] = BLACK_PAWN;
            board[50] = BLACK_PAWN;
            board[49] = BLACK_PAWN;
            board[48] = BLACK_PAWN;

            board[15] = WHITE_PAWN;
            board[14] = WHITE_PAWN;
            board[13] = WHITE_PAWN;
            board[12] = WHITE_PAWN;
            board[11] = WHITE_PAWN;
            board[10] = WHITE_PAWN;
            board[9] = WHITE_PAWN;
            board[8] = WHITE_PAWN;
            board[7] = WHITE_ROOK;
            board[6] = WHITE_KNIGHT;
            board[5] = WHITE_BISHOP;
            board[4] = WHITE_KING;
            board[3] = WHITE_QUEEN;
            board[2] = WHITE_BISHOP;
            board[1] = WHITE_KNIGHT;
            board[0] = WHITE_ROOK;

            canWhiteCastleKingside = true;
            canWhiteCastleQueenside = true;
            canBlackCastleKingside = true;
            canBlackCastleQueenside = true;

            CurrentPlayer = PGNPlayer.WHITE;
        }

        public PGNMove DoMove(PGNMove move)
        {
            if (move.KingSideCastling || move.QueenSideCastling)
            {
                changeBoard(move);
                return move;
            }

            List<PGNMove> legalMoves = GenerateLegalMoves();

            var matchingMoves = from PGNMove legalMove in legalMoves
                                where legalMove.DestinationSquare == move.DestinationSquare 
                                    && legalMove.Piece == move.Piece 
                                    && legalMove.PromotionTo == move.PromotionTo
                                select legalMove;

            if (matchingMoves.Count() < 1)
            {
                throw new ArgumentException("Move " + move.ToString() + " is not a valid move", nameof(move));
            }
            if (matchingMoves.Count() == 1)
            {
                changeBoard(matchingMoves.First());
                return matchingMoves.First();
            }

            if (move.SourceFile >= 0 && move.SourceRank >= 0)
            {
                var filterMoves = from PGNMove legalMove in matchingMoves
                                  where legalMove.SourceFile == move.SourceFile && legalMove.SourceRank == move.SourceRank
                                  select legalMove;

                if (filterMoves.Count() == 1)
                {
                    changeBoard(filterMoves.First());
                    return filterMoves.First();
                }
            }
            else if (move.SourceFile >= 0)
            {
                var filterFileMoves = from PGNMove legalMove in matchingMoves
                                      where legalMove.SourceFile == move.SourceFile
                                      select legalMove;

                if (filterFileMoves.Count() == 1)
                {
                    changeBoard(filterFileMoves.First());
                    return filterFileMoves.First();
                }
            }
            else if (move.SourceRank >= 0)
            {
                var filterRankMoves = from PGNMove legalMove in matchingMoves
                                      where legalMove.SourceRank == move.SourceRank
                                      select legalMove;

                if (filterRankMoves.Count() == 1)
                {
                    changeBoard(filterRankMoves.First());
                    return filterRankMoves.First();
                }
            }

            return null;
        }

        private void changeBoard(PGNMove move)
        {
            if (move.KingSideCastling == true)
            {
                switch (CurrentPlayer)
                {
                    case PGNPlayer.WHITE:
                        board[4] = 0;
                        board[5] = WHITE_ROOK;
                        board[6] = WHITE_KING;
                        board[7] = 0;
                        canWhiteCastleKingside = false;
                        canWhiteCastleQueenside = false;
                        break;
                    case PGNPlayer.BLACK:
                        board[60] = 0;
                        board[61] = BLACK_ROOK;
                        board[62] = BLACK_KING;
                        board[63] = 0;
                        canBlackCastleKingside = false;
                        canBlackCastleQueenside = false;
                        break;
                }
            }
            else if (move.QueenSideCastling == true)
            {
                switch (CurrentPlayer)
                {
                    case PGNPlayer.WHITE:
                        board[0] = 0;
                        board[1] = 0;
                        board[2] = WHITE_KING;
                        board[3] = WHITE_ROOK;
                        board[4] = 0;
                        canWhiteCastleKingside = false;
                        canWhiteCastleQueenside = false;
                        break;
                    case PGNPlayer.BLACK:
                        board[56] = 0;
                        board[57] = 0;
                        board[58] = BLACK_KING;
                        board[59] = BLACK_ROOK;
                        board[60] = 0;
                        canBlackCastleKingside = false;
                        canBlackCastleQueenside = false;
                        break;
                }
            }
            else
            {
                board[move.DestinationSquare] = board[move.SourceSquare];
                board[move.SourceSquare] = 0;
                if (move.SourceSquare == 4) { canWhiteCastleKingside = false; canWhiteCastleQueenside = false; }
                if (move.SourceSquare == 60) { canBlackCastleKingside = false; canBlackCastleQueenside = false; }
                if (move.SourceSquare == 0) { canWhiteCastleQueenside = false; }
                if (move.SourceSquare == 7) { canWhiteCastleKingside = false; }
                if (move.SourceSquare == 56) { canBlackCastleQueenside = false; }
                if (move.SourceSquare == 63) { canBlackCastleKingside = false; }
                if (move.DestinationSquare == enPassantPawn)
                {
                    if (CurrentPlayer == PGNPlayer.WHITE)
                    {
                        board[move.DestinationSquare - 8] = 0;
                    }
                    else
                    {
                        board[move.DestinationSquare + 8] = 0;
                    }
                }
                if(move.PromotionTo > PGNPiece.PAWN)
                {
                    switch(move.PromotionTo)
                    {
                        case PGNPiece.KNIGHT:
                            board[move.DestinationSquare] = (byte)((CurrentPlayer == PGNPlayer.WHITE ? WHITE_KNIGHT : BLACK_KNIGHT)); break;
                        case PGNPiece.BISHOP:
                            board[move.DestinationSquare] = (byte)((CurrentPlayer == PGNPlayer.WHITE ? WHITE_BISHOP : BLACK_BISHOP)); break;
                        case PGNPiece.ROOK:
                            board[move.DestinationSquare] = (byte)((CurrentPlayer == PGNPlayer.WHITE ? WHITE_ROOK : BLACK_ROOK)); break;
                        case PGNPiece.QUEEN:
                            board[move.DestinationSquare] = (byte)((CurrentPlayer == PGNPlayer.WHITE ? WHITE_QUEEN : BLACK_QUEEN)); break;
                    }
                }
            }

            enPassantPawn = -1;
            if (move.Piece == PGNPiece.PAWN)
            {
                bool doublePush = (Math.Abs(move.DestinationSquare - move.SourceSquare) == 16);
                if (doublePush)
                {
                    enPassantPawn = (CurrentPlayer == PGNPlayer.WHITE ? move.DestinationSquare - 8 : move.DestinationSquare + 8);
                }
            }

            if (CurrentPlayer == PGNPlayer.WHITE)
            {
                CurrentPlayer = PGNPlayer.BLACK;
            }
            else
            {
                CurrentPlayer = PGNPlayer.WHITE;
            }
        }

        private bool CanCastleKingside(PGNPlayer curPlayer)
        {
            switch (curPlayer)
            {
                case PGNPlayer.WHITE:
                    return (canWhiteCastleKingside && board[5] == 0 && board[6] == 0);
                case PGNPlayer.BLACK:
                    return (canBlackCastleKingside && board[61] == 0 && board[62] == 0);
            }

            return false;
        }
        private bool CanCastleQueenside(PGNPlayer curPlayer)
        {
            switch (curPlayer)
            {
                case PGNPlayer.WHITE:
                    return (canWhiteCastleQueenside && board[1] == 0 && board[2] == 0 && board[3] == 0);
                case PGNPlayer.BLACK:
                    return (canBlackCastleQueenside && board[57] == 0 && board[58] == 0 && board[59] == 0);
            }

            return false;
        }

        private int findPieceIndex(int piece)
        {
            for (int i = 0; i < 64; i++)
            {
                if (board[i] == piece)
                {
                    return i;
                }
            }

            return -1;
        }

        public List<PGNMove> GenerateLegalMoves()
        {
            List<PGNMove> srcList = GeneratePseudoValidMoves(CurrentPlayer);
            List<PGNMove> moveList = new List<PGNMove>();
            foreach (PGNMove move in srcList)
            {
                PGNBoardState newState = new PGNBoardState(this);
                newState.changeBoard(move);
                int kingSquare = newState.findPieceIndex((CurrentPlayer == PGNPlayer.WHITE ? WHITE_KING : BLACK_KING));
                List<PGNMove> opponentMoves = newState.GeneratePseudoValidMoves(newState.CurrentPlayer);
                var kingCaptures = from PGNMove opponentMove in opponentMoves
                                   where opponentMove.DestinationSquare == kingSquare
                                   select opponentMove;

                if (kingCaptures.Count() < 1)
                {
                    moveList.Add(move);
                }
            }

            return moveList;
        }
        private List<PGNMove> GeneratePseudoValidMoves(PGNPlayer curPlayer)
        {
            int lookfor = WHITE_PAWN;
            int direction = 1;
            List<PGNMove> moveList = new List<PGNMove>();

            if (curPlayer == PGNPlayer.BLACK)
            {
                lookfor = BLACK_PAWN;
                direction = -1;
            }

            int[] board88 = new int[128];
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    int boardIdx = (r * 8) + c;
                    int board88Idx = (r * 16) + c;

                    board88[board88Idx] = board[boardIdx];
                }
            }

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    int board88Idx = (r * 16) + c;

                    if (board88[board88Idx] >= lookfor && board88[board88Idx] <= lookfor + 6)
                    {
                        // Found a piece that needs to have moves generated
                        switch (board88[board88Idx] - lookfor)
                        {
                            case 0: // pawn
                                GeneratePawnMoves(moveList, board88, board88Idx, direction);
                                break;
                            case 1: // knight
                                GenerateKnightMoves(moveList, curPlayer, board88, board88Idx);
                                break;
                            case 2: // bishop
                                GenerateBishopMoves(moveList, curPlayer, board88, board88Idx);
                                break;
                            case 3: // rook
                                GenerateRookMoves(moveList, curPlayer, board88, board88Idx);
                                break;
                            case 4: // queen
                                GenerateQueenMoves(moveList, curPlayer, board88, board88Idx);
                                break;
                            case 5: // king
                                GenerateKingMoves(moveList, curPlayer, board88, board88Idx);
                                break;
                        }
                    }
                }
            }

            return moveList;
        }

        private void GenerateKingMoves(List<PGNMove> moveList, PGNPlayer curPlayer, int[] board88, int board88Idx)
        {
            short srcSquare = idx88Convert(board88Idx);

            foreach (int offset in kingMovesOffsets)
            {
                int new_idx = board88Idx + offset;
                if ((new_idx & 0x88) == 0)
                {
                    short destSquare = idx88Convert(new_idx);
                    if (board88[new_idx] != 0)
                    {
                        int pieceAtDest = board88[new_idx];
                        if (curPlayer == PGNPlayer.WHITE && pieceAtDest < BLACK_PAWN) continue;
                        if (curPlayer == PGNPlayer.BLACK && pieceAtDest >= BLACK_PAWN) continue;
                    }

                    PGNMove tmp = new PGNMove(PGNPiece.KING, srcSquare, destSquare, (board88[new_idx] != 0));
                    moveList.Add(tmp);
                }
            }

            if (CanCastleKingside(curPlayer) == true)
            {
                PGNMove tmp = new PGNMove(PGNPiece.KING, 0, 64, false);
                moveList.Add(tmp);
            }
            if (CanCastleQueenside(curPlayer) == true)
            {
                PGNMove tmp = new PGNMove(PGNPiece.KING, 64, 0, false);
                moveList.Add(tmp);
            }
        }
        private void GenerateQueenMoves(List<PGNMove> moveList, PGNPlayer curPlayer, int[] board88, int board88Idx)
        {
            short srcSquare = idx88Convert(board88Idx);
            for (int xDir = -1; xDir <= 1; xDir += 1)
            {
                for (int yDir = -16; yDir <= 16; yDir += 16)
                {
                    if (xDir == 0 && yDir == 0) continue;

                    int new_idx = board88Idx + xDir + yDir;

                    while ((new_idx & 0x88) == 0)
                    {
                        short destSquare = idx88Convert(new_idx);
                        if (board88[new_idx] != 0)
                        {
                            int pieceAtDest = board88[new_idx];
                            if (curPlayer == PGNPlayer.WHITE && pieceAtDest < BLACK_PAWN) break;
                            if (curPlayer == PGNPlayer.BLACK && pieceAtDest >= BLACK_PAWN) break;
                        }

                        PGNMove tmp = new PGNMove(PGNPiece.QUEEN, srcSquare, destSquare, (board88[new_idx] != 0));
                        moveList.Add(tmp);
                        if (tmp.Capture == true) break;

                        new_idx = new_idx + xDir + yDir;
                    }
                }
            }
        }
        private void GenerateRookMoves(List<PGNMove> moveList, PGNPlayer curPlayer, int[] board88, int board88Idx)
        {
            short srcSquare = idx88Convert(board88Idx);
            for (int xDir = -1; xDir <= 1; xDir += 2)
            {
                int new_idx = board88Idx + xDir;

                while ((new_idx & 0x88) == 0)
                {
                    short destSquare = idx88Convert(new_idx);
                    if (board88[new_idx] != 0)
                    {
                        int pieceAtDest = board88[new_idx];
                        if (curPlayer == PGNPlayer.WHITE && pieceAtDest < BLACK_PAWN) break;
                        if (curPlayer == PGNPlayer.BLACK && pieceAtDest >= BLACK_PAWN) break;
                    }

                    PGNMove tmp = new PGNMove(PGNPiece.ROOK, srcSquare, destSquare, (board88[new_idx] != 0));
                    moveList.Add(tmp);
                    if (tmp.Capture == true) break;

                    new_idx = new_idx + xDir;
                }
            }
            for (int yDir = -16; yDir <= 16; yDir += 32)
            {
                int new_idx = board88Idx + yDir;

                while ((new_idx & 0x88) == 0)
                {
                    short destSquare = idx88Convert(new_idx);
                    if (board88[new_idx] != 0)
                    {
                        int pieceAtDest = board88[new_idx];
                        if (curPlayer == PGNPlayer.WHITE && pieceAtDest < BLACK_PAWN) break;
                        if (curPlayer == PGNPlayer.BLACK && pieceAtDest >= BLACK_PAWN) break;
                    }

                    PGNMove tmp = new PGNMove(PGNPiece.ROOK, srcSquare, destSquare, (board88[new_idx] != 0));
                    moveList.Add(tmp);
                    if (tmp.Capture == true) break;

                    new_idx = new_idx + yDir;
                }
            }
        }
        private void GenerateBishopMoves(List<PGNMove> moveList, PGNPlayer curPlayer, int[] board88, int board88Idx)
        {
            short srcSquare = idx88Convert(board88Idx);
            for (int xDir = -1; xDir <= 1; xDir += 2)
            {
                for (int yDir = -16; yDir <= 16; yDir += 32)
                {
                    int new_idx = board88Idx + xDir + yDir;

                    while ((new_idx & 0x88) == 0)
                    {
                        short destSquare = idx88Convert(new_idx);
                        if (board88[new_idx] != 0)
                        {
                            int pieceAtDest = board88[new_idx];
                            if (curPlayer == PGNPlayer.WHITE && pieceAtDest < BLACK_PAWN) break;
                            if (curPlayer == PGNPlayer.BLACK && pieceAtDest >= BLACK_PAWN) break;
                        }

                        PGNMove tmp = new PGNMove(PGNPiece.BISHOP, srcSquare, destSquare, (board88[new_idx] != 0));
                        moveList.Add(tmp);
                        if (tmp.Capture == true) break;

                        new_idx = new_idx + xDir + yDir;
                    }
                }
            }
        }
        private void GenerateKnightMoves(List<PGNMove> moveList, PGNPlayer curPlayer, int[] board88, int board88Idx)
        {
            short srcSquare = idx88Convert(board88Idx);

            foreach (int offset in knightMovesOffsets)
            {
                int new_idx = board88Idx + offset;
                if ((new_idx & 0x88) == 0)
                {
                    short destSquare = idx88Convert(new_idx);
                    if (board88[new_idx] != 0)
                    {
                        int pieceAtDest = board88[new_idx];
                        if (curPlayer == PGNPlayer.WHITE && pieceAtDest < BLACK_PAWN) continue;
                        if (curPlayer == PGNPlayer.BLACK && pieceAtDest >= BLACK_PAWN) continue;
                    }

                    PGNMove tmp = new PGNMove(PGNPiece.KNIGHT, srcSquare, destSquare, (board88[new_idx] != 0));
                    moveList.Add(tmp);
                }
            }
        }
        private void GeneratePawnMoves(List<PGNMove> moveList, int[] board88, int board88Idx, int direction)
        {
            short srcSquare = idx88Convert(board88Idx);

            // Check normal 1 move forward
            int new_idx = board88Idx + (direction * 16);
            if (((new_idx & 0x88) == 0) && (board88[new_idx] == 0))
            {
                short destSquare = idx88Convert(new_idx);
                if ((direction == 1 && destSquare > 55) ||
                    (direction == -1 && destSquare < 8))
                {
                    // Pawn Promotion
                    for(PGNPiece promotedTo = PGNPiece.KNIGHT; promotedTo < PGNPiece.KING; promotedTo++)
                    {
                        PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare, promotedTo);
                        moveList.Add(tmp);
                    }
                }
                else
                {
                    PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare);
                    moveList.Add(tmp);
                }
            }
            // Check normal left capture
            new_idx = board88Idx + (direction * 15);
            if (((new_idx & 0x88) == 0) && (board88[new_idx] != 0))
            {
                short destSquare = idx88Convert(new_idx);
                if ((direction == 1 && destSquare > 55) ||
                    (direction == -1 && destSquare < 8))
                {
                    // Pawn Promotion
                    for (PGNPiece promotedTo = PGNPiece.KNIGHT; promotedTo < PGNPiece.KING; promotedTo++)
                    {
                        PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare, true, promotedTo);
                        moveList.Add(tmp);
                    }
                }
                else
                {
                    PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare, true);
                    moveList.Add(tmp);
                }
            }
            // Check normal right capture
            new_idx = board88Idx + (direction * 17);
            if (((new_idx & 0x88) == 0) && (board88[new_idx] != 0))
            {
                short destSquare = idx88Convert(new_idx);
                if ((direction == 1 && destSquare > 55) ||
                    (direction == -1 && destSquare < 8))
                {
                    // Pawn Promotion
                    for (PGNPiece promotedTo = PGNPiece.KNIGHT; promotedTo < PGNPiece.KING; promotedTo++)
                    {
                        PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare, true, promotedTo);
                        moveList.Add(tmp);
                    }
                }
                else
                {
                    PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare, true);
                    moveList.Add(tmp);
                }
            }

            int row = board88Idx / 16;
            if (enPassantPawn > 0)
            {
                int en_passant_idx = ((enPassantPawn / 8) * 16) + (enPassantPawn % 8);
                int en_passant_row = en_passant_idx / 16;
                if ((direction == -1 && en_passant_row == 2) ||
                    (direction == 1 && en_passant_row == 5))
                {
                    new_idx = board88Idx + (direction * 15);
                    if (((new_idx & 0x88) == 0) && (new_idx == en_passant_idx))
                    {
                        short destSquare = idx88Convert(new_idx);
                        PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare, true);
                        moveList.Add(tmp);
                    }
                    new_idx = board88Idx + (direction * 17);
                    if (((new_idx & 0x88) == 0) && (new_idx == en_passant_idx))
                    {
                        short destSquare = idx88Convert(new_idx);
                        PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare, true);
                        moveList.Add(tmp);
                    }
                }
            }

            if ((row == 6) && (direction == -1))
            {
                // Check two forward for black
                new_idx = board88Idx + (direction * 32);
                int new_idx_2 = board88Idx + (direction * 16);
                if (((new_idx & 0x88) == 0) && (board88[new_idx] == 0) && (board88[new_idx_2] == 0))
                {
                    short destSquare = idx88Convert(new_idx);
                    PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare);
                    moveList.Add(tmp);
                }
            }
            else if ((row == 1) && (direction == 1))
            {
                // Check two forward for white
                new_idx = board88Idx + (direction * 32);
                int new_idx_2 = board88Idx + (direction * 16);
                if (((new_idx & 0x88) == 0) && (board88[new_idx] == 0) && (board88[new_idx_2] == 0))
                {
                    short destSquare = idx88Convert(new_idx);
                    PGNMove tmp = new PGNMove(PGNPiece.PAWN, srcSquare, destSquare);
                    moveList.Add(tmp);
                }
            }
        }

        private static short idx88Convert(int idx88)
        {
            return (short)(((idx88 / 16) * 8) + (idx88 % 16));
        }
    }
}
