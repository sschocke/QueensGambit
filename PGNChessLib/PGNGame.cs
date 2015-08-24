using System.Collections.Generic;
using System.IO;

namespace PGNChessLib
{
    public enum PGNGameResult
    {
        UNKNOWN,
        WHITE_WINS,
        BLACK_WINS,
        DRAW,
    }

    public enum PGNPlayer
    {
        WHITE,
        BLACK
    }

    public class PGNGame
    {
        private static int NextGameID = 1;

        public int GameID { get; private set; }
        public string WhitePlayer { get; private set; }
        public string BlackPlayer { get; private set; }
        public PGNGameResult Result { get; private set; }
        public List<PGNMove> Moves { get; private set; }

        public PGNGame(StreamReader reader)
        {
            GameID = NextGameID++;

            while(!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if(line.StartsWith("[") == true)
                {
                    //This is a tag pair
                    string tag = line.Substring(1, line.IndexOf(' ')).Trim().ToLower();
                    string value = line.Substring(line.IndexOf('"')+1);
                    value = value.Substring(0, value.IndexOf('"'));

                    switch (tag)
                    {
                        case "white":
                            WhitePlayer = value;
                            break;
                        case "black":
                            BlackPlayer = value;
                            break;
                        case "result":
                            Result = ResultTextToEnum(value);
                            break;
                    }
                }
                else
                {
                    //Tag pairs finished
                    while(string.IsNullOrWhiteSpace(line))
                    {
                        line = reader.ReadLine();
                    }

                    string moves = line.Trim();
                    line = reader.ReadLine().Trim(); ;
                    while( string.IsNullOrWhiteSpace(line) == false)
                    {
                        moves += " " + line;
                        line = reader.ReadLine().Trim();
                    }
                    moves = moves.Replace(". ", ".");

                    Moves = PGNMove.Parse(moves);
                    break;
                }
            }
        }

        private PGNGameResult ResultTextToEnum(string resultText)
        {
            switch(resultText.ToLower())
            {
                case "1/2-1/2":
                    return PGNGameResult.DRAW;
                case "1-0":
                    return PGNGameResult.WHITE_WINS;
                case "0-1":
                    return PGNGameResult.BLACK_WINS;
                default:
                    return PGNGameResult.UNKNOWN;
            }
        }
    }
}