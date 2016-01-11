namespace GoldBoxExplorer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.folderView = new System.Windows.Forms.TreeView();
            this.smallImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.fileView = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dateModifiedColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sizeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileExportMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fileExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.zoom400 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom125 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom75 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomCustom = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutGoldBoxExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.directoryLabel = new System.Windows.Forms.ToolStripLabel();
            this.directoryTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomIncreaseButton = new System.Windows.Forms.ToolStripButton();
            this.zoomResetButton = new System.Windows.Forms.ToolStripButton();
            this.zoomDecreaseButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.largeImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.fileExportMenuStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(831, 491);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(831, 562);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(831, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(795, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.folderView);
            this.splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(831, 491);
            this.splitContainer1.SplitterDistance = 211;
            this.splitContainer1.TabIndex = 0;
            // 
            // folderView
            // 
            this.folderView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderView.FullRowSelect = true;
            this.folderView.HideSelection = false;
            this.folderView.ImageIndex = 2;
            this.folderView.ImageList = this.smallImageList;
            this.folderView.Location = new System.Drawing.Point(0, 0);
            this.folderView.Name = "folderView";
            this.folderView.SelectedImageIndex = 3;
            this.folderView.Size = new System.Drawing.Size(211, 491);
            this.folderView.TabIndex = 0;
            this.folderView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.DirectoryTreeBeforeExpand);
            // 
            // smallImageList
            // 
            this.smallImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImageList.ImageStream")));
            this.smallImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.smallImageList.Images.SetKeyName(0, "zoom_in");
            this.smallImageList.Images.SetKeyName(1, "zoom_out");
            this.smallImageList.Images.SetKeyName(2, "folder");
            this.smallImageList.Images.SetKeyName(3, "folder_ok");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.AutoScroll = true;
            this.splitContainer2.Panel1.Resize += new System.EventHandler(this.SplitContainerPanelResize);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.fileView);
            this.splitContainer2.Size = new System.Drawing.Size(616, 491);
            this.splitContainer2.SplitterDistance = 224;
            this.splitContainer2.TabIndex = 0;
            // 
            // fileView
            // 
            this.fileView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.dateModifiedColumn,
            this.typeColumn,
            this.sizeColumn});
            this.fileView.ContextMenuStrip = this.fileExportMenuStrip;
            this.fileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileView.FullRowSelect = true;
            this.fileView.HideSelection = false;
            this.fileView.Location = new System.Drawing.Point(0, 0);
            this.fileView.MultiSelect = false;
            this.fileView.Name = "fileView";
            this.fileView.Size = new System.Drawing.Size(616, 263);
            this.fileView.TabIndex = 0;
            this.fileView.UseCompatibleStateImageBehavior = false;
            this.fileView.View = System.Windows.Forms.View.Details;
            this.fileView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.FileListViewColumnClick);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 100;
            // 
            // dateModifiedColumn
            // 
            this.dateModifiedColumn.Text = "Date modified";
            this.dateModifiedColumn.Width = 150;
            // 
            // typeColumn
            // 
            this.typeColumn.DisplayIndex = 3;
            this.typeColumn.Text = "Type";
            // 
            // sizeColumn
            // 
            this.sizeColumn.DisplayIndex = 2;
            this.sizeColumn.Text = "Size";
            this.sizeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.sizeColumn.Width = 70;
            // 
            // fileExportMenuStrip
            // 
            this.fileExportMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileExportMenuItem});
            this.fileExportMenuStrip.Name = "fileExportMenuStrip";
            this.fileExportMenuStrip.Size = new System.Drawing.Size(117, 26);
            // 
            // fileExportMenuItem
            // 
            this.fileExportMenuItem.Name = "fileExportMenuItem";
            this.fileExportMenuItem.Size = new System.Drawing.Size(116, 22);
            this.fileExportMenuItem.Text = "&Export...";
            this.fileExportMenuItem.Click += new System.EventHandler(this.ChangeDirectoryClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(831, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.toolStripSeparator4,
            this.zoom400,
            this.zoom200,
            this.zoom150,
            this.zoom125,
            this.zoom100,
            this.zoom75,
            this.zoom50,
            this.toolStripSeparator5,
            this.zoomCustom});
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.zoomToolStripMenuItem.Text = "Zoom";
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.zoomInToolStripMenuItem.Text = "Zoom In";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom Out";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(130, 6);
            // 
            // zoom400
            // 
            this.zoom400.Name = "zoom400";
            this.zoom400.Size = new System.Drawing.Size(133, 22);
            this.zoom400.Text = "400%";
            this.zoom400.Click += new System.EventHandler(this.zoom400_Click);
            // 
            // zoom200
            // 
            this.zoom200.Name = "zoom200";
            this.zoom200.Size = new System.Drawing.Size(133, 22);
            this.zoom200.Text = "200%";
            this.zoom200.Click += new System.EventHandler(this.zoom200_Click);
            // 
            // zoom150
            // 
            this.zoom150.Name = "zoom150";
            this.zoom150.Size = new System.Drawing.Size(133, 22);
            this.zoom150.Text = "150%";
            this.zoom150.Click += new System.EventHandler(this.zoom150_Click);
            // 
            // zoom125
            // 
            this.zoom125.Name = "zoom125";
            this.zoom125.Size = new System.Drawing.Size(133, 22);
            this.zoom125.Text = "125%";
            this.zoom125.Click += new System.EventHandler(this.zoom125_Click);
            // 
            // zoom100
            // 
            this.zoom100.Name = "zoom100";
            this.zoom100.Size = new System.Drawing.Size(133, 22);
            this.zoom100.Text = "100%";
            this.zoom100.Click += new System.EventHandler(this.zoom100_Click);
            // 
            // zoom75
            // 
            this.zoom75.Name = "zoom75";
            this.zoom75.Size = new System.Drawing.Size(133, 22);
            this.zoom75.Text = "75%";
            this.zoom75.Click += new System.EventHandler(this.zoom75_Click);
            // 
            // zoom50
            // 
            this.zoom50.Name = "zoom50";
            this.zoom50.Size = new System.Drawing.Size(133, 22);
            this.zoom50.Text = "50%";
            this.zoom50.Click += new System.EventHandler(this.zoom50_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(130, 6);
            // 
            // zoomCustom
            // 
            this.zoomCustom.Name = "zoomCustom";
            this.zoomCustom.Size = new System.Drawing.Size(133, 22);
            this.zoomCustom.Text = "Custom...";
            this.zoomCustom.Click += new System.EventHandler(this.zoomCustom_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutGoldBoxExplorerToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutGoldBoxExplorerToolStripMenuItem
            // 
            this.aboutGoldBoxExplorerToolStripMenuItem.Name = "aboutGoldBoxExplorerToolStripMenuItem";
            this.aboutGoldBoxExplorerToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.aboutGoldBoxExplorerToolStripMenuItem.Text = "About Gold Box Explorer";
            this.aboutGoldBoxExplorerToolStripMenuItem.Click += new System.EventHandler(this.aboutGoldBoxExplorerToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.directoryLabel,
            this.directoryTextBox,
            this.toolStripSeparator1,
            this.zoomIncreaseButton,
            this.zoomResetButton,
            this.zoomDecreaseButton,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(3, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(446, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // directoryLabel
            // 
            this.directoryLabel.Name = "directoryLabel";
            this.directoryLabel.Size = new System.Drawing.Size(51, 22);
            this.directoryLabel.Text = "Directory";
            // 
            // directoryTextBox
            // 
            this.directoryTextBox.Name = "directoryTextBox";
            this.directoryTextBox.ReadOnly = true;
            this.directoryTextBox.Size = new System.Drawing.Size(300, 25);
            this.directoryTextBox.Click += new System.EventHandler(this.DirectoryTextBoxClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // zoomIncreaseButton
            // 
            this.zoomIncreaseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomIncreaseButton.Image = global::GoldBoxExplorer.Properties.Resources.zoomin2blue;
            this.zoomIncreaseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomIncreaseButton.Name = "zoomIncreaseButton";
            this.zoomIncreaseButton.Size = new System.Drawing.Size(23, 22);
            this.zoomIncreaseButton.Text = "Zoom In";
            this.zoomIncreaseButton.Click += new System.EventHandler(this.ZoomIncreaseButtonClick);
            // 
            // zoomResetButton
            // 
            this.zoomResetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomResetButton.Image = global::GoldBoxExplorer.Properties.Resources.Mglasblue;
            this.zoomResetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomResetButton.Name = "zoomResetButton";
            this.zoomResetButton.Size = new System.Drawing.Size(23, 22);
            this.zoomResetButton.Text = "Reset Zoom";
            this.zoomResetButton.Click += new System.EventHandler(this.ZoomResetClick);
            // 
            // zoomDecreaseButton
            // 
            this.zoomDecreaseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomDecreaseButton.Image = global::GoldBoxExplorer.Properties.Resources.zoomout2blue;
            this.zoomDecreaseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomDecreaseButton.Name = "zoomDecreaseButton";
            this.zoomDecreaseButton.Size = new System.Drawing.Size(23, 22);
            this.zoomDecreaseButton.Text = "Zoom Out";
            this.zoomDecreaseButton.Click += new System.EventHandler(this.ZoomDecreaseButtonClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // largeImageList
            // 
            this.largeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeImageList.ImageStream")));
            this.largeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.largeImageList.Images.SetKeyName(0, "zoom_in_32.png");
            this.largeImageList.Images.SetKeyName(1, "zoom_out_32.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 562);
            this.Controls.Add(this.toolStripContainer1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gold Box Explorer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormOnClose);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyUp);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyUp);
            this.Resize += new System.EventHandler(this.MainFormResize);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.fileExportMenuStrip.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView folderView;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView fileView;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader dateModifiedColumn;
        private System.Windows.Forms.ColumnHeader sizeColumn;
        private System.Windows.Forms.ToolStripLabel directoryLabel;
        private System.Windows.Forms.ToolStripTextBox directoryTextBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ImageList smallImageList;
        private System.Windows.Forms.ImageList largeImageList;
        private System.Windows.Forms.ContextMenuStrip fileExportMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileExportMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton zoomIncreaseButton;
        private System.Windows.Forms.ToolStripButton zoomResetButton;
        private System.Windows.Forms.ToolStripButton zoomDecreaseButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem zoom400;
        private System.Windows.Forms.ToolStripMenuItem zoom200;
        private System.Windows.Forms.ToolStripMenuItem zoom150;
        private System.Windows.Forms.ToolStripMenuItem zoom125;
        private System.Windows.Forms.ToolStripMenuItem zoom100;
        private System.Windows.Forms.ToolStripMenuItem zoom75;
        private System.Windows.Forms.ToolStripMenuItem zoom50;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem zoomCustom;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutGoldBoxExplorerToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader typeColumn;

    }
}

