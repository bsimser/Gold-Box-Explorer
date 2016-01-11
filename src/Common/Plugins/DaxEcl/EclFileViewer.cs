using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GoldBoxExplorer.Lib.Plugins.DaxEcl
{
    public class EclFileViewer : IGoldBoxViewer
    {
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;
        private readonly DaxEclFile _file;
        private TabControl tab;
        private int findNext = 0;

        public EclFileViewer(DaxEclFile file)
        {
            _file = file;
        }

// dynamically create ECL code listing when the tab is clicked on
        private void ECLTabControlUnloadDeselected(Object sender, TabControlEventArgs e)
        {
            if (e.TabPage != null && e.TabPage.Controls.Find("codepanel", false).Length > 0)
            {
                EmptyECLCodePanel(e.TabPage);

            }
        }
   

 
        // dynamically create ECL code listing when the tab is clicked on
        private void ECLTabControlLoadSelected(Object sender, TabControlEventArgs e)
        {
            if (e.TabPage != null && e.TabPage.Controls.Find("codepanel", false).Length > 0)
            {
                FillECLCodePanel(e.TabPage);

            }
        }
        public Control GetControl()
        {

                tab = new TabControl { Dock = DockStyle.Fill };

                Control bookmarkedRow = null; // the row we want to start at

                foreach (var ecl in _file.eclDumps)
                {
                    var page = new TabPage(ecl._blockName);
                    Panel codePanel = (Panel) ViewerHelper.CreatePanel();
                    codePanel.Name = "codepanel";
                    codePanel.Tag = ecl;
                    //page.AutoScroll = true;
                    page.Controls.Add(codePanel);

                    // fill the code panel with the decoded ecl code
                    bookmarkedRow = FillECLCodePanel(page);
                    // add a search bar and 'select all' button to the top of the ecl listing
                    var selectAll = ViewerHelper.CreateButton();
                    selectAll.Text = "Copy to clipboard";
                    selectAll.MouseClick += selectAllRows;
                    selectAll.Dock = DockStyle.Right;

                    var findNext = ViewerHelper.CreateButton();
                    findNext.Text = "find next";
                    findNext.MouseClick += searchEclNext;
                    findNext.Dock = DockStyle.Right;

                    TextBox headerText = (TextBox) ViewerHelper.CreateTextBox();
                    headerText.ReadOnly = false;
                    headerText.Text = "Type text to find";
                    headerText.TextChanged += searchEcl;
                    headerText.KeyDown += searchEclKeyPressed;
                    headerText.Dock = DockStyle.Fill;
                    var row1 = ViewerHelper.CreateRow();
                    page.Controls.Add(row1);
                    row1.Controls.Add(headerText);
                    row1.Controls.Add(findNext);
                    row1.Controls.Add(selectAll);


                    tab.TabPages.Add(page);
                    if (page.Text == ChangeFileEventArgs.currentDaxId.ToString())
                    {
                        tab.SelectedTab = page;
                        codePanel.ScrollControlIntoView(bookmarkedRow);
                    }
                }
                var stringPage = new TabPage("ECL Text");
                stringPage.AutoScroll = true;
                var control = ViewerHelper.CreateTextBoxMultiline();
                control.Text = _file.ToString();
                stringPage.Controls.Add(control);
                tab.TabPages.Add(stringPage);
                tab.Selected += ECLTabControlLoadSelected;
                tab.Deselected += ECLTabControlUnloadDeselected;
                return tab;
            
        }
        private static void EmptyECLCodePanel(TabPage tabPage)
        {
            Panel codePanel = (Panel)tabPage.Controls.Find("codepanel", false)[0];
            codePanel.Controls.Clear();
        }
        private static Control FillECLCodePanel(TabPage page)
        {
            // this can take a while, so make sure the cursor is set to wait
            Application.UseWaitCursor = true;
            Application.DoEvents();

            Control bookmarkedRow = null;
            Panel codePanel = (Panel)page.Controls.Find("codepanel", false)[0];
            EclDump.EclDump ecl = (EclDump.EclDump) codePanel.Tag;
            // decode ecl files, and put each line in its own textbox
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
                var row = ViewerHelper.CreateRow();
                codePanel.Controls.Add(row);
                row.Controls.Add(eclText);

                row.Controls.Add(eclAnnotation);
                if (ChangeFileEventArgs.targetPlace != "" && eclAnnotation.Text.Contains(ChangeFileEventArgs.targetPlace) && page.Text == ChangeFileEventArgs.currentDaxId.ToString())
                    bookmarkedRow = row;
            }
            Application.UseWaitCursor = false;
            Application.DoEvents();

            return bookmarkedRow;
        }

        public float Zoom { get; set; }
        public void findInEcl(object sender, int index = 0)
        {
            var findTextBox = (TextBox)sender;
            var text = findTextBox.Text;
            Panel eclListing = (Panel)findTextBox.Parent.Parent.Controls[0];
            string textFound = "";
            // clear all selections
            foreach (var c in eclListing.Controls)
            {
                var co = (System.Windows.Forms.Panel)c;
                var tb = (System.Windows.Forms.TextBox)co.Controls[0];
                tb.HideSelection = true;
            }
            // search through the textboxes to find the text, select it
            for (int i = eclListing.Controls.Count-1; i > 0; i--)
            {
                System.Windows.Forms.Panel rowControl = (System.Windows.Forms.Panel) eclListing.Controls[i];
                System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)rowControl.Controls[0];
                System.Windows.Forms.TextBox atb = (System.Windows.Forms.TextBox)rowControl.Controls[1];
                int textStart = tb.Text.IndexOf(text, StringComparison.CurrentCultureIgnoreCase);

                if (textStart > -1)
                {
                    if (index <= 1)
                    {
                        scrollIntoViewAndHighlight(text, eclListing, rowControl, tb, textStart);
                        return;
                    }
                    else
                        index--;
                }
                else
                {
                    // can't find the text in the ecl code textbox, so try the annotations textbox next to it
                    textStart = atb.Text.IndexOf(text, StringComparison.CurrentCultureIgnoreCase);
                    if (textStart > -1)
                    {
                        if (index <= 1)
                        {
                            scrollIntoViewAndHighlight(text, eclListing, rowControl, atb, textStart);
                            return;
                        }
                        else
                            index--;
                    }

                }
            }

            // nothing found! reset the find next counter
            findNext = 0;

        }

        private static void scrollIntoViewAndHighlight(string text, Panel eclListing, System.Windows.Forms.Panel co, System.Windows.Forms.TextBox tb, int textStart)
        {
            eclListing.ScrollControlIntoView(co);
            tb.Select(textStart, text.Length);
            tb.HideSelection = false;
            return;
        }
  
        public void searchEcl(object sender, EventArgs e)
        {
            findInEcl(sender);
        }
        public void searchEclNext(object sender, MouseEventArgs e)
        {
            var b = (System.Windows.Forms.ButtonBase)sender;
            findNext++;
            findInEcl(b.Parent.Controls[0], findNext);
        }
        void searchEclKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var b = (System.Windows.Forms.TextBoxBase)sender;
                findNext++;
                findInEcl(b.Parent.Controls[0], findNext);
            }
        }
        public void selectAllRows(object sender, MouseEventArgs e)
        {
            // select all the ecl code in the panel and send it to the clipboard
            var b = (System.Windows.Forms.ButtonBase) sender;
            var eclListing = b.Parent.Parent.Controls[0];
            string text = "";
            foreach (System.Windows.Forms.Panel co in eclListing.Controls) {
                System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox) co.Controls[0];
                text = tb.Text + "\r\n" + text;
            }
            Clipboard.SetText(text);
        }
        public int ContainerWidth { get; set; }
    }
}