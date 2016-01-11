using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GoldBoxExplorer.Lib.Plugins.Character
{
    public class FruaCharacterViewer :  IGoldBoxViewer
    {
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;
        private readonly FruaCharacterFile _file;
        private TabControl tab;
        private bool displayItemTypeName = false;

        public FruaCharacterViewer(FruaCharacterFile file, PluginParameter parameters)
        {
            _file = file;
            Zoom = parameters.Zoom;
            ContainerWidth = parameters.ContainerWidth;
            if (_file.formatType == "item template")
                displayItemTypeName = true;
        }

        private void TabControl1_Selected(Object sender, TabControlEventArgs e)
        {
            if (e.TabPage != null && e.TabPage.Controls.Count == 0)
                e.TabPage.Controls.Add(addAnnotationPage(e.TabPage));
        }
        private void TabControl1_Deselected(Object sender, TabControlEventArgs e)
        {
            // TODO: remove this message box code
            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "TabPage", e.TabPage);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "TabPageIndex", e.TabPageIndex);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Action", e.Action);
            messageBoxCS.AppendLine();
            //MessageBox.Show(messageBoxCS.ToString(), "Deselected Event");
            if (e.TabPage != null)
                unloadTabPage(e.TabPage);

        }

        public Control GetControl()
        {
                tab = new TabControl { Dock = DockStyle.Fill };
                tab.Deselected += TabControl1_Deselected;
                tab.Selected += TabControl1_Selected;

                if (_file.daxFile)
                {
                    for (var i = 0; i < _file._daxBlocks.Count; i++)
                    {
                        processRecords(_file._daxBlocks[i], _file._blockIds[i].ToString());
                    }
                    return tab;
                }
                if (_file.formatType == "item" || _file.formatType == "spc"
                    ||_file.formatType == "item template")
                {
                    processRecords(_file.GetBytes().ToArray(), _file.formatType);
                }
                else
                {
                    tab.TabPages.Add(initTabPage("Character", _file.GetBytes()));
                }
                return tab;
        }

        private void processRecords(byte[] data, string id)
        {
            int recordSize = _file.LoadCharacter().recordLength; // TODO get rid of multiple loadcharacter calls
            int recordStart = _file.variableRecordStart;
            int blockSize = data.Length;
            int recordCount = blockSize / recordSize;
            if (recordCount == 1)
            {
                tab.TabPages.Add(initTabPage(id, data));
            }
            if (recordCount > 1)
            {
                for (var j = 0; j < recordCount; j++)
                {
                    IList<byte> record;
                    string sub_id;
                    
                    
                    // records are in daxblocks
                /*    if (_file._daxBlocks.Count > 0)
                    {
                        record = _file._daxBlocks[i].Skip(j * recordSize).Take(recordSize).ToList();
                        subid = _file._blockIds[i].ToString();
                    }
                    // records are lumped together in a file
                    else
                    {
                        record = _file.GetBytes().Skip(i * _file.variableRecordLength).Take(_file.variableRecordLength).ToList();
                        id = i.ToString();
                    }*/
                    record = data.Skip(recordStart + (j * recordSize)).Take(recordSize).ToList();
                    tab.TabPages.Add(initTabPage(id + "-" + j.ToString(), record, j));
                }

            }
        }
        class AnnotationData { public string PageName; public IList<byte> data; public int pageNumber = 0;}

        private TabPage initTabPage(string pageName, IList<byte> data, int pageNumber = 0)
        {
            var page = new TabPage(pageName);
            
            var annotationData = new AnnotationData();
            annotationData.PageName = pageName;
            annotationData.data = data;
            annotationData.pageNumber = pageNumber;
            page.Tag = annotationData;
            page.Controls.Add(addAnnotationPage(page));
            return page;

        }
        private void unloadTabPage(TabPage tabPage)
        {
            tabPage.Controls.Clear();
            
        }
        private Control addAnnotationPage(TabPage tabPage)
        {
            var annotationData = (AnnotationData)tabPage.Tag;
            var data = annotationData.data;
            var pageNumber = annotationData.pageNumber;
            var outerPanel = ViewerHelper.CreatePanel();
            var buttonPanel = ViewerHelper.CreatePanel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 30;
            var innerPanel = ViewerHelper.CreatePanel();
            var saveCharButton = ViewerHelper.CreateButton();
            saveCharButton.Dock = DockStyle.Left;
            var saveAnnotationButton = ViewerHelper.CreateButton();
            saveAnnotationButton.Dock = DockStyle.Right;
            saveCharButton.Text = "save to file";
            saveCharButton.Enabled = false;
            saveAnnotationButton.Text = "record annotation";
            saveAnnotationButton.Enabled = false;
            //saveAnnotationButton.Dock = DockStyle.Top | DockStyle.Right;

        //    buttonPanel.Controls.Add(saveCharButton);
        //    buttonPanel.Controls.Add(saveAnnotationButton);
            outerPanel.Controls.Add(innerPanel);
            outerPanel.Controls.Add(buttonPanel);

            // TODO merge these two calls for more efficiency

            var annotations = _file.LoadCharacter().getAnnotations();
            if (annotations != null)
            {
                annotations.Sort((x, y) => y.addr.CompareTo(x.addr));
                foreach (var a in annotations)
                {
                    var row = ViewerHelper.CreateRow();
                    var label = ViewerHelper.CreateTextBox(true);
                    label.Width = 400;
                    label.Text = a.description;
                    if (a.firstInRecord)
                        label.BackColor = System.Drawing.Color.Azure;
                    var control = ViewerHelper.CreateTextBox(false);
                    control.Text = a.interpret(data);
                    control.Width = 200;
                    row.Controls.Add(control);
                    row.Controls.Add(label);

                    innerPanel.Controls.Add(row);
                }
            }
            if (displayItemTypeName)
            {
                var nameRow = ViewerHelper.CreateRow();
                var nameLabel = ViewerHelper.CreateTextBox(true);
                nameLabel.Width = 400;
                nameLabel.Text = Annotation.GetItemTemplateName(pageNumber);
                nameRow.Controls.Add(nameLabel);
                innerPanel.Controls.Add(nameRow);
            }
            return outerPanel;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}