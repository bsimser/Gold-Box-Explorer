using System.Windows.Forms;
using System;

namespace GoldBoxExplorer.Lib.Plugins.Items
{
    public class FruaItemViewer : IGoldBoxViewer
    {
        private readonly FruaItemFile _file;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;

        public FruaItemViewer(FruaItemFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {
            var view = CreateListView();
            AddColumnsTo(view);
            AddItemsTo(view);
            return view;
        }

        private static ListView CreateListView()
        {
            return new ListView
                       {
                           Dock = DockStyle.Fill,
                           View = View.Details,
                           FullRowSelect = true,
                       };
        }

        private void AddItemsTo(ListView view)
        {
            foreach (var item in _file.GetItems())
            {
                var listViewItem = view.Items.Add(item.Name);
                listViewItem.SubItems.Add(item.Encumbrance.ToString());
                listViewItem.SubItems.Add(item.Price.ToString("#,#"));
                listViewItem.SubItems.Add(item.MagicBonus.ToString());
                listViewItem.SubItems.Add(item.SecondaryCode.ToString());
                listViewItem.SubItems.Add(item.IsReady ? "Yes" : "No");
                listViewItem.SubItems.Add(item.Identified.ToString());
                listViewItem.SubItems.Add(item.IsCursed ? "Yes" : "No");
                listViewItem.SubItems.Add(item.NumberOfItemsInBundle.ToString());
                listViewItem.SubItems.Add(item.Charges.ToString());
                listViewItem.SubItems.Add(item.MagicalEffectCode.ToString());
                listViewItem.SubItems.Add(item.SpecialCode);
                listViewItem.SubItems.Add(item.GetLocation());
                listViewItem.SubItems.Add(item.Hands.ToString());
                listViewItem.SubItems.Add(item.DamageVsLarge.ToString());
                listViewItem.SubItems.Add(item.Rate.ToString());
                listViewItem.SubItems.Add(item.Protection.ToString());
                listViewItem.SubItems.Add(item.WeaponClass);
                listViewItem.SubItems.Add(item.MeleeClass);
                listViewItem.SubItems.Add(item.DamageVsMedium.ToString());
                listViewItem.SubItems.Add(item.Range.ToString());
                listViewItem.SubItems.Add(item.ClassUsage.ToString());
                listViewItem.SubItems.Add(item.MissileType.ToString());
            }
        }

        private static void AddColumnsTo(ListView view)
        {
            view.Columns.Add(new ColumnHeader { Text = "Name", Name = "Name", Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Encumbrance", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Price", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Magic Bonus", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Secondary Code", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Ready", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Identified", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Cursed", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Items in Bundle", TextAlign = HorizontalAlignment.Right, Width = -2});
            view.Columns.Add(new ColumnHeader { Text = "Charges", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Magical Effect", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Special Code", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Location", Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Hands", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Damage vs. Large", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Rate", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Protection", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Weapon Type", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Melee Usage", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Damage vs. Medium/Small", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Range", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Class", TextAlign = HorizontalAlignment.Right, Width = -2 });
            view.Columns.Add(new ColumnHeader { Text = "Missile Type", TextAlign = HorizontalAlignment.Right, Width = -2 });
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}