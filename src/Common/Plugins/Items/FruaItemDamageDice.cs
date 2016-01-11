namespace GoldBoxExplorer.Lib.Plugins.Items
{
    public class FruaItemDamageDice
    {
        public int Number { get; set; }
        public int Type { get; set; }
        public int Bonus { get; set; }

        public override string ToString()
        {
            if (Number > 0)
            {
                var format = string.Format("{0}d{1}", Number, Type);
                if (Bonus > 0)
                    format += string.Format("+{0}", Bonus);
                return format;
            }
            return "None";
        }
    }
}