using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueensGambit.NNDataLib
{
    public class ChessBoardInputState
    {
        public const byte BLANK = 0x00;

        public const byte PAWN = 0x01;
        public const byte KNIGHT = 0x02;
        public const byte BISHOP = 0x03;
        public const byte ROOK = 0x04;
        public const byte QUEEN = 0x05;
        public const byte KING = 0x06;

        public const byte WHITE = 0x00;
        public const byte BLACK = 0x08;

        private uint[] board;
        private byte[] status;

        public ChessBoardInputState()
        {
            board = new uint[8];
            status = new byte[3];
        }

        public ChessBoardInputState(byte[] curBoard, byte moveStatus, byte pieceStatus, byte castlingStatus)
            : this()
        {
            if (curBoard.Length != 64)
            {
                throw new ArgumentOutOfRangeException(nameof(curBoard), "Current Board array should be 64 items");
            }

            for (int r = 0; r < 8; r++)
            {
                board[r] = 0x00;
                for (int c = 0; c < 8; c++)
                {
                    board[r] |= (uint)(curBoard[(r * 8) + c]) << (c * 4);
                }
            }
            status[0] = moveStatus;
            status[1] = pieceStatus;
            status[2] = castlingStatus;
        }

        public byte PieceAt(byte boardIdx)
        {
            if (boardIdx > 63)
            {
                throw new ArgumentOutOfRangeException(nameof(boardIdx), "Board Index is out of range (0-63)");
            }

            byte row = (byte)(boardIdx / 8);
            byte col = (byte)(boardIdx % 8);
            uint bitmask = (uint)(0x1111 >> col);

            byte pieceID = (byte)((board[row] & bitmask) << col);

            return pieceID;
        }
    }
}
