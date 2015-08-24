using System;
using System.Collections.Generic;

namespace PGNChessLib
{
    public enum PGNPiece
    {
        PAWN,
        KNIGHT,
        BISHOP,
        ROOK,
        QUEEN,
        KING
    }

    public class PGNMove
    {
        public static List<PGNMove> Parse(string movesText)
        {
            List<PGNMove> moves = new List<PGNMove>();

            int pos = 0;
            char c;
            PGNMove curMove = new PGNMove();
            while (pos < movesText.Length)
            {
                string remainder = movesText.Substring(pos);
                if (remainder == "1/2-1/2" || remainder == "0-1" || remainder == "1-0" || remainder == "*" || remainder == "?") break;

                c = movesText[pos];
                if (Char.IsDigit(c) == true)
                {
                    //This is a move number, which is relatively irrelevant for us, but we'll store it anyway
                    curMove.Number = Int16.Parse(movesText.Substring(pos, movesText.IndexOf('.', pos) - pos));
                    pos = movesText.IndexOf('.', pos) + 1;
                    continue;
                }
                if (c == '.')
                {
                    //This happens when there are multiple dots like the 3 dots after a black move number indication... can just be skipped
                    pos++;
                    continue;
                }
                if (c == ' ')
                {
                    //This happens when there are multiple spaces between moves... can just be skipped
                    pos++;
                    continue;
                }
                if (c == 'O')
                {
                    // Castling indication
                    if (movesText.Substring(pos, 5) == "O-O-O")
                    {
                        curMove.QueenSideCastling = true;
                        pos += 6;
                    }
                    else if (movesText.Substring(pos, 3) == "O-O")
                    {
                        curMove.KingSideCastling = true;
                        pos += 4;
                    }
                    else
                    {
                        throw new Exception("Illegal move detected for move " + curMove.Number);
                    }

                    moves.Add(curMove);
                    curMove = new PGNMove(curMove.Number);
                    continue;
                }
                curMove.Piece = getPieceType(ref pos, c);
                c = movesText[pos];

                if (c == 'x')
                {
                    curMove.Capture = true;
                    pos++;
                    c = movesText[pos];
                }

                if (Char.IsDigit(c) == true)
                {
                    //Source Identifying rank digit
                    curMove.SourceRank = digitToRank(c);
                    pos++;
                    c = movesText[pos];

                    if (c == 'x')
                    {
                        curMove.Capture = true;
                        pos++;
                        c = movesText[pos];
                    }
                }

                // Destination File
                curMove.DestinationFile = letterToFile(c);
                pos++;
                c = movesText[pos];

                while (Char.IsDigit(c) == false)
                {
                    if (c == 'x')
                    {
                        curMove.Capture = true;
                    }
                    else
                    {
                        // Previous File letter was actually a Source File Identifier
                        curMove.SourceFile = curMove.DestinationFile;
                        curMove.DestinationFile = letterToFile(c);
                    }
                    pos++;
                    c = movesText[pos];
                }

                curMove.DestinationRank = digitToRank(c);
                pos++;
                c = movesText[pos];

                while (c != ' ')
                {
                    if (c == '+') curMove.Check = true;
                    if (c == '#') curMove.Mate = true;

                    if (c == '=')
                    {
                        // Promotion
                        pos++;
                        c = movesText[pos];
                        curMove.PromotionTo = getPieceType(ref pos, c);
                        c = movesText[pos];
                        continue;
                    }

                    pos++;
                    c = movesText[pos];
                }

                moves.Add(curMove);
                curMove = new PGNMove(curMove.Number);
                pos++;
                continue;
            }
            return moves;
        }

        private static short digitToRank(char rankDigit)
        {
            switch (rankDigit)
            {
                case '1':
                    return 0;
                case '2':
                    return 1;
                case '3':
                    return 2;
                case '4':
                    return 3;
                case '5':
                    return 4;
                case '6':
                    return 5;
                case '7':
                    return 6;
                case '8':
                    return 7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rankDigit), "Not a valid chess board Rank digit");
            }
        }
        private static short letterToFile(char fileLetter)
        {
            switch (fileLetter)
            {
                case 'a':
                    return 0;
                case 'b':
                    return 1;
                case 'c':
                    return 2;
                case 'd':
                    return 3;
                case 'e':
                    return 4;
                case 'f':
                    return 5;
                case 'g':
                    return 6;
                case 'h':
                    return 7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileLetter), "Not a valid chess board File letter");
            }
        }
        private static char fileToLetter(short file)
        {
            return (char)('a' + file);
        }
        private static PGNPiece getPieceType(ref int pos, char c)
        {
            switch (c)
            {
                case 'P':
                    pos++;
                    return PGNPiece.PAWN;
                case 'N':
                    pos++;
                    return PGNPiece.KNIGHT;
                case 'B':
                    pos++;
                    return PGNPiece.BISHOP;
                case 'R':
                    pos++;
                    return PGNPiece.ROOK;
                case 'Q':
                    pos++;
                    return PGNPiece.QUEEN;
                case 'K':
                    pos++;
                    return PGNPiece.KING;
                default:
                    return PGNPiece.PAWN;
            }
        }
        private static string pieceTypeToLetter(PGNPiece piece)
        {
            switch (piece)
            {
                case PGNPiece.PAWN:
                    return "P";
                case PGNPiece.BISHOP:
                    return "B";
                case PGNPiece.KNIGHT:
                    return "N";
                case PGNPiece.ROOK:
                    return "R";
                case PGNPiece.QUEEN:
                    return "Q";
                case PGNPiece.KING:
                    return "K";
            }

            throw new Exception("Unknown Piece Type : " + piece.ToString());
        }

        public int Number { get; private set; }
        public PGNPiece Piece { get; private set; }
        public short SourceRank { get; private set; } = -1;
        public short SourceFile { get; private set; } = -1;
        public short DestinationRank { get; private set; } = -1;
        public short DestinationFile { get; private set; } = -1;
        public bool Capture { get; private set; }
        public bool QueenSideCastling { get; private set; }
        public bool KingSideCastling { get; private set; }
        public bool Check { get; private set; }
        public bool Mate { get; private set; }
        public PGNPiece PromotionTo { get; private set; } = PGNPiece.PAWN;

        public short SourceSquare { get { return (short)((SourceRank * 8) + SourceFile); } }
        public short DestinationSquare { get { return (short)((DestinationRank * 8) + DestinationFile); } }

        private PGNMove()
            : this(1)
        { }

        private PGNMove(int moveNumber)
        {
            this.Number = moveNumber;
        }

        public PGNMove(PGNPiece piece, short srcSquare, short destSquare)
            : this(piece, srcSquare, destSquare, false)
        { }

        public PGNMove(PGNPiece piece, short srcSquare, short destSquare, bool capture)
            : this(piece, srcSquare, destSquare, capture, PGNPiece.PAWN)
        { }

        public PGNMove(PGNPiece piece, short srcSquare, short destSquare, PGNPiece promotedTo)
            : this(piece, srcSquare, destSquare, false, promotedTo)
        { }

        public PGNMove(PGNPiece piece, short srcSquare, short destSquare, bool capture, PGNPiece promotedTo)
        {
            Piece = piece;
            if (srcSquare == 0 && destSquare == 64)
            {
                KingSideCastling = true;
                return;
            }
            if (srcSquare == 64 && destSquare == 0)
            {
                QueenSideCastling = true;
                return;
            }
            Capture = capture;
            SourceFile = (short)(srcSquare % 8);
            SourceRank = (short)(srcSquare / 8);
            DestinationFile = (short)(destSquare % 8);
            DestinationRank = (short)(destSquare / 8);
            PromotionTo = promotedTo;
        }

        public override string ToString()
        {
            string moveText = Number.ToString() + ".";
            if (KingSideCastling)
            {
                return moveText + "O-O";
            }
            if (QueenSideCastling)
            {
                return moveText + "O-O-O";
            }
            if (DestinationFile == -1 || DestinationRank == -1)
            {
                return moveText + " INVALID";
            }

            if (Piece > PGNPiece.PAWN)
            {
                moveText += pieceTypeToLetter(Piece);
            }
            if (SourceFile != -1)
            {
                moveText += fileToLetter(SourceFile);
            }
            if (SourceRank != -1)
            {
                moveText += (SourceRank + 1).ToString();
            }
            if (Capture == true)
            {
                moveText += "x";
            }
            moveText += fileToLetter(DestinationFile);
            moveText += (DestinationRank + 1).ToString();
            if (PromotionTo > PGNPiece.PAWN)
            {
                moveText += "=";
                moveText += pieceTypeToLetter(PromotionTo);
            }
            if (Check)
            {
                moveText += "+";
            }
            if (Mate)
            {
                moveText += "#";
            }

            return moveText;
        }
    }
}
