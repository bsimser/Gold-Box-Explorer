using System;
using System.Windows.Forms;
using DaxFileLibrary.Frua;
using GoldBoxExplorer.Common.Viewers;

namespace GoldBoxExplorer.Common.Frua
{
    public class FruaVaultFileViewer : IGoldBoxViewer
    {
        private readonly FruaVaultFile _file;

        public FruaVaultFileViewer(FruaVaultFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {
            var viewer = new ListBox { Dock = DockStyle.Fill };
            viewer.Items.Add(_file.ToString());
            return viewer;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
        
        public string GetMode()
        {
            return "FRUA Vault File";
        }
    }
}