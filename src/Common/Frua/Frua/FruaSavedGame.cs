using System.Text;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaSavedGame
    {
        public byte NoMagicFlag { get; set; }
        public int Rounds { get; set; }
        public int Turns { get; set; }
        public int Hours { get; set; }
        public int Days { get; set; }
        public int Months { get; set; }
        public int Years { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Rounds: {0}\r\n", Rounds);
            sb.AppendFormat("Turns: {0}\r\n", Turns);
            sb.AppendFormat("Hours: {0}\r\n", Hours);
            sb.AppendFormat("Days: {0}\r\n", Days);
            sb.AppendFormat("Months: {0}\r\n", Months);
            sb.AppendFormat("Years: {0}\r\n", Years);

            return sb.ToString();
        }
    }
}