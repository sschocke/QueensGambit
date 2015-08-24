using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace PGNChessLib
{
    public class PGNDatabase : ObservableCollection<PGNGame>
    {

        public PGNDatabase(StreamReader reader)
            : base()
        {
            while(!reader.EndOfStream)
            {
                Add(new PGNGame(reader));
            }

            Debug.WriteLine("Added " + Count + " games");
        }
    }
}
