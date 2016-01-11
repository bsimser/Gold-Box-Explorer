using System;
using System.Windows.Forms;
using DaxFileLibrary.Frua;
using GoldBoxExplorer.Common.Viewers;

namespace GoldBoxExplorer.Common.Frua
{
    public class FruaItemViewer : IGoldBoxViewer
    {
        private readonly FruaItemFile _file;

        public FruaItemViewer(FruaItemFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {
            var view = createListView();
            addColumnsTo(view);
            addItemsTo(view);
            autoSizeColumnsFor(view);
            return view;
        }

        private static ListView createListView()
        {
            return new ListView
                       {
                           Dock = DockStyle.Fill,
                           View = View.Details,
                           FullRowSelect = true,
                       };
        }

        private void addItemsTo(ListView view)
        {
            foreach (var item in _file.GetItems())
            {
                var listViewItem = view.Items.Add(item.ToString());
//                listViewItem.SubItems.Add(item.Price.ToString("#,#"));
//                listViewItem.SubItems.Add(item.Encumbrance.ToString());
//                listViewItem.SubItems.Add(item.MagicBonus.ToString());
//                listViewItem.SubItems.Add(item.Charges.ToString());
//                listViewItem.SubItems.Add(item.IsCursed ? "Yes" : "No");
//                listViewItem.SubItems.Add(item.IsReady ? "Yes" : "No");
//                listViewItem.SubItems.Add(item.Hands.ToString());
//                listViewItem.SubItems.Add(item.Range.ToString());
//                listViewItem.SubItems.Add(item.GetLocation());
//                listViewItem.SubItems.Add(item.GetDamageDice(item.DamageVsLarge));
//                listViewItem.SubItems.Add(item.GetDamageDice(item.DamageVsMedium));
            }
        }

        private static void addColumnsTo(ListView view)
        {
            view.Columns.Add(new ColumnHeader {Text = "Name", Name = "Name"});
//            view.Columns.Add(new ColumnHeader {Text = "Price", Width = 50, TextAlign = HorizontalAlignment.Right});
//            view.Columns.Add(new ColumnHeader {Text = "Encumbrance", Width = 80, TextAlign = HorizontalAlignment.Right});
//            view.Columns.Add(new ColumnHeader {Text = "Magic Bonus", Width = 80, TextAlign = HorizontalAlignment.Right});
//            view.Columns.Add(new ColumnHeader {Text = "Charges", Width = 60, TextAlign = HorizontalAlignment.Right});
//            view.Columns.Add(new ColumnHeader {Text = "Cursed", Width = 60, TextAlign = HorizontalAlignment.Right});
//            view.Columns.Add(new ColumnHeader {Text = "Ready", Width = 60, TextAlign = HorizontalAlignment.Right});
//            view.Columns.Add(new ColumnHeader {Text = "Hands", Width = 60, TextAlign = HorizontalAlignment.Right});
//            view.Columns.Add(new ColumnHeader {Text = "Range", Width = 60, TextAlign = HorizontalAlignment.Right});
//            view.Columns.Add(new ColumnHeader {Text = "Location", Name = "Location"});
//            view.Columns.Add(new ColumnHeader {Text = "Damage vs. Large", Name = "DamageVsLarge", TextAlign = HorizontalAlignment.Center});
//            view.Columns.Add(new ColumnHeader {Text = "Damage vs. Medium", Name = "DamageVsMedium", TextAlign = HorizontalAlignment.Center});
        }

        private static void autoSizeColumnsFor(ListView view)
        {
            view.Columns["Name"].Width = -1;
//            view.Columns["Location"].Width = -1;
//            view.Columns["DamageVsLarge"].Width = -2;
//            view.Columns["DamageVsMedium"].Width = -2;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
        
        public string GetMode()
        {
            return "FRUA Item List";
        }
    }
}