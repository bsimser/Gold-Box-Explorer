using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GoldBoxExplorer.Common.Viewers;

namespace GoldBoxExplorer.Common.Frua
{
    public class FruaGameDataFileViewer : IGoldBoxViewer
    {
        public IEnumerable<string> GameData { get; set; }

        public FruaGameDataFileViewer(IEnumerable<string> gamedata)
        {
            GameData = gamedata;
        }

        public Control GetControl()
        {
            var viewer = new ListBox { Dock = DockStyle.Fill };
            if (GameData != null) viewer.Items.AddRange(GameData.ToArray());
            return viewer;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
        
        public string GetMode()
        {
            return "FRUA Game Data";
        }
    }
}