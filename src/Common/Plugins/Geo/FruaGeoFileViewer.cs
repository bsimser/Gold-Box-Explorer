using System.Windows.Forms;
using System;
using System.Drawing;

namespace GoldBoxExplorer.Lib.Plugins.Geo
{
    public class FruaGeoFileViewer : IGoldBoxViewer
    {
        private readonly FruaGeoFile _file;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;
        public FruaGeoFileViewer(FruaGeoFile file)
        {
            _file = file;
        }


        public Control GetControl()
        {
            var tab = new TabControl
                {
                    Dock = DockStyle.Fill
                };
       /*     var summary = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D,
                Font = new System.Drawing.Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
            };*/
            var summary = ViewerHelper.CreateTextBoxMultiline();
            summary.Text = _file.GetSummary();
            var summaryPage = new TabPage("Summary");
            summaryPage.Controls.Add(summary);
            tab.TabPages.Add(summaryPage);

            var mapPage = new TabPage("Map");
            var mapSummary = ViewerHelper.CreateTextBoxMultiline();/*                = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D,
                Font = new System.Drawing.Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
            };*/

            mapSummary.Text = _file.GetMap();
            mapPage.Controls.Add(mapSummary);
            tab.TabPages.Add(mapPage);

            return tab;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}