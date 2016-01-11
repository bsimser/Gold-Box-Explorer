using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GoldBoxExplorer.Lib.Plugins.DaxEcl
{
    public class EclStringViewer : IGoldBoxViewer
    {
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;
        private readonly DaxEclFile _file;
        private TabControl tab;

        public EclStringViewer(DaxEclFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {

                tab = new TabControl { Dock = DockStyle.Fill };

                Control bookmarkedRow = null; // the row we want to start at

                foreach (var ecl in _file.eclDumps)
                {
                    var page = new TabPage(ecl._blockName);
                    page.AutoScroll = true;

                    foreach (var eclAddr in ecl.decodedEcl.Keys.Reverse())
                    {
                        var eclText = ViewerHelper.CreateTextBox();
                        eclText.Text = ecl.decodedEcl[eclAddr];
                        eclText.Dock = DockStyle.Fill;
                        var eclAnnotation = ViewerHelper.CreateTextBox();
                        if (ecl.annotations.ContainsKey(eclAddr))
                        {
                            eclAnnotation.Text = ecl.annotations[eclAddr];
                            eclText.BackColor = System.Drawing.Color.LightBlue;
                            eclAnnotation.BackColor = System.Drawing.Color.LightBlue;

                        }
                        eclAnnotation.Width = 300;
                        eclAnnotation.Dock = DockStyle.Right;
                        //eclText.Dock = DockStyle.Top;
                        var row = ViewerHelper.CreateRow();
                        page.Controls.Add(row);
                        row.Controls.Add(eclText);

                        row.Controls.Add(eclAnnotation);
                        if (ChangeFileEventArgs.targetPlace != "" && eclAnnotation.Text.Contains(ChangeFileEventArgs.targetPlace) && page.Text == ChangeFileEventArgs.currentDaxId.ToString())
                            bookmarkedRow = row;


                    }
                    tab.TabPages.Add(page);
                    if (page.Text == ChangeFileEventArgs.currentDaxId.ToString())
                    {
                        tab.SelectedTab = page;
                        page.ScrollControlIntoView(bookmarkedRow);
                    }
                }
                var stringPage = new TabPage("ECL Text");
                stringPage.AutoScroll = true;
                var control = ViewerHelper.CreateTextBoxMultiline();
                control.Text = _file.ToString();
                stringPage.Controls.Add(control);
                tab.TabPages.Add(stringPage);
                return tab;
            
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}