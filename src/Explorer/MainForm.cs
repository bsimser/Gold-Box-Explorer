using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GoldBoxExplorer.Lib;
using GoldBoxExplorer.Properties;
using System.Diagnostics;

namespace GoldBoxExplorer
{
    public partial class MainForm : Form, IMainView
    {
        private readonly MainViewPresenter _presenter;
        private bool _ctrlIsDown;
        
      //  private static System.Timers.Timer aTimer;
      //  delegate void SetTextCallback(string text);

//       [System.Runtime.InteropServices.DllImport("User32")]
  //      private extern static int GetGuiResources(IntPtr hProcess, int uiFlags);

//        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
  //      {
            //statusLabel.Text
//                if (this.statusLabel.InvokeRequired)
  //  {
        // It's on a different thread, so use Invoke.

    //        string h = "";
      //  using (Process process = Process.GetCurrentProcess())
    //    {
    //        var gdiHandles = GetGuiResources(process.Handle, 0);
    //        var userHandles = GetGuiResources(process.Handle, 1);
    //        h = gdiHandles.ToString() + " , " + userHandles.ToString();
      //  }

        //SetTextCallback d = new SetTextCallback(SetText);
        //this.Invoke(d, new object[] {h});
    //}
    //else
    //{
        // It's on the same thread, no need for Invoke 
        //SetTextCallback d = new SetTextCallback(SetText);
        //this.statusLabel.Text= e.SignalTime.ToShortTimeString();
    //    }
//        private void SetText(string text)
  //      {
    //        this.statusLabel.Text = text;
      //  }
        public MainForm()
        {
            InitializeComponent();
            _presenter = new MainViewPresenter(this);
            HookupEventHandlers();
            HookupExtraKeyboardShortcuts();

        /*    aTimer = new System.Timers.Timer();
            aTimer.Interval = 500;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Enabled = true;*/
        }

        private void HookupExtraKeyboardShortcuts()
        {
            zoomInToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Add;
            zoomInToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl +";
            zoomOutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Subtract;
            zoomOutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl -";
        }

        private void HookupEventHandlers()
        {
            folderView.AfterSelect += FolderViewAfterSelect;
            fileView.SelectedIndexChanged += FileViewSelectedIndexChanged;
            Load += MainFormLoad;
            MouseWheel += MainFormMouseWheel;
        }

        void MainFormMouseWheel(object sender, MouseEventArgs e)
        {
            if (!_ctrlIsDown) return;

            var delta = Math.Sign(e.Delta);
            
            if(delta < 0)
            {
                _presenter.DecreaseZoomLevel();
            }
            else if(delta > 0)
            {
                _presenter.IncreaseZoomLevel();
            }
        }

        //public void ChangeSelectedFile(string filename, int daxBlockId)
        public void ChangeSelectedFile(Object sender, ChangeFileEventArgs e)
        {
            var x = fileView.FindItemWithText(e.filename);

            ChangeFileEventArgs.currentDaxId = e.daxId;
            ChangeFileEventArgs.targetPlace = e.place;
            x.Selected = true;
            x.Focused = true;

        }
        private void FileViewSelectedIndexChanged(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                if (fileView.SelectedItems.Count <= 0) return;
                _presenter.CurrentFile = fileView.SelectedItems[0].Text;
                var plugin = _presenter.ProcessFile();
                if (plugin != null)
                    plugin.Viewer.ChangeSelectedFile += ChangeSelectedFile;
            }
        }

        void FolderViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            using (new WaitCursor())
            {
                if (!e.Node.IsSelected) return;
                _presenter.LoadDirectory(e.Node.Tag.ToString());
            }
        }

        void MainFormLoad(object sender, EventArgs e)
        {
            _presenter.InitView();
            InitDirectory(Settings.Default.RootFolder);
        }

        public void SetZoomMessage(string message)
        {
            statusLabel.Text = message;
        }

        public void DisplayBlock(Control control)
        {
            splitContainer2.Panel1.Controls.Clear();
            splitContainer2.Panel1.Controls.Add(control);
        }

        public int GetViewerWidth()
        {
            return splitContainer2.Panel1.Width - SystemInformation.VerticalScrollBarWidth;
        }

        public void RefreshView()
        {
            foreach (Control control in splitContainer2.Panel1.Controls)
            {
                control.Invalidate(true);
            }
        }

        public void DisplayMode(string mode)
        {
            statusLabel.Text = mode;
        }

        public void DisplayFileList(IList<FileDto> files)
        {
            fileView.Items.Clear();

            foreach (var file in files)
            {
                var child = new ListViewItem(file.Name);
                var listViewItem = fileView.Items.Add(child);
                listViewItem.SubItems.Add(file.LastWrite);
                listViewItem.SubItems.Add(file.Type);
                listViewItem.SubItems.Add(file.Size);
            }

            foreach (ColumnHeader column in fileView.Columns)
            {
                column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private static void AddDirectories(TreeNode node, string path)
        {
            node.Nodes.Clear();

            try
            {
                var currentDir = new DirectoryInfo(path);
                var subdirs = currentDir.GetDirectories();

                foreach (
                    var child in subdirs.Select(subdir => new TreeNode(subdir.Name.ToUpper()) {Tag = subdir.FullName}))
                {
                    child.Nodes.Add(new TreeNode());
                    node.Nodes.Add(child);
                }
            }
            catch
            {
            }
        }

        private void DirectoryTreeBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                AddDirectories(e.Node, (string) e.Node.Tag);
            }
        }

        private void InitDirectory(string rootFolder)
        {
            if (!rootFolder.EndsWith(@"\")) rootFolder += @"\";
            directoryTextBox.Text = rootFolder.ToUpper();
            var root = new TreeNode(directoryTextBox.Text) {Tag = rootFolder};
            root.Nodes.Add(new TreeNode());
            folderView.Nodes.Clear();
            folderView.Nodes.Add(root);
            root.Expand();
            folderView.SelectedNode = root;
        }

        private void DirectoryTextBoxClick(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (!result.Equals(DialogResult.OK)) return;
            Settings.Default.RootFolder = folderBrowserDialog1.SelectedPath;
            InitDirectory(Settings.Default.RootFolder);
        }

        private void MainFormOnClose(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
        }

        private void ZoomIncreaseButtonClick(object sender, EventArgs e)
        {
            _presenter.IncreaseZoomLevel();
        }

        private void ZoomDecreaseButtonClick(object sender, EventArgs e)
        {
            _presenter.DecreaseZoomLevel();
        }

        private void ChangeDirectoryClick(object sender, EventArgs e)
        {
            if (fileView.SelectedItems.Count != 1) return;
            var filename = fileView.SelectedItems[0].Text;
            
            using (var form = new ExportForm(_presenter.CurrentDirectory, filename, _presenter.getPlugin()))
            {
                form.ShowDialog();
            }
        }

        private void FileListViewColumnClick(object sender, ColumnClickEventArgs e)
        {
            var sorter = new ListViewSorter();
            fileView.ListViewItemSorter = sorter;

            sorter = (ListViewSorter) fileView.ListViewItemSorter;

            if (sorter.LastSort == e.Column)
            {
                fileView.Sorting = fileView.Sorting == SortOrder.Ascending
                                           ? SortOrder.Descending
                                           : SortOrder.Ascending;
            }
            else
            {
                fileView.Sorting = SortOrder.Descending;
            }
            sorter.ByColumn = e.Column;

            fileView.Sort();
        }

        private void SplitContainerPanelResize(object sender, EventArgs e)
        {
            if (_presenter != null) _presenter.ResizeBlockViewer(splitContainer2.Panel1.Size);
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = 200;
        }

        private void MainFormKeyUp(object sender, KeyEventArgs e)
        {
            _ctrlIsDown = e.Control;
        }

        private void ZoomResetClick(object sender, EventArgs e)
        {
            _presenter.ResetZoom();
        }

        private void aboutGoldBoxExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

        private static void ShowAboutDialog()
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.IncreaseZoomLevel();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.DecreaseZoomLevel();
        }

        private void zoom400_Click(object sender, EventArgs e)
        {
            _presenter.SetZoom(4);
        }

        private void zoom200_Click(object sender, EventArgs e)
        {
            _presenter.SetZoom(2);
        }

        private void zoom150_Click(object sender, EventArgs e)
        {
            _presenter.SetZoom(1.5);
        }

        private void zoom125_Click(object sender, EventArgs e)
        {
            _presenter.SetZoom(1.25);
        }

        private void zoom100_Click(object sender, EventArgs e)
        {
            _presenter.SetZoom(1);
        }

        private void zoom75_Click(object sender, EventArgs e)
        {
            _presenter.SetZoom(0.75);
        }

        private void zoom50_Click(object sender, EventArgs e)
        {
            _presenter.SetZoom(0.5);
        }

        private void zoomCustom_Click(object sender, EventArgs e)
        {
            var zoom = (int)(_presenter.ZoomLevel*100);
            var result = InputBox.CreateUsing("Set Custom Zoom Level", "Zoom Level (in percent)", ref zoom);
            if (result == DialogResult.OK)
            {
                _presenter.SetZoom(zoom*0.01);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}