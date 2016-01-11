namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    partial class WallTemplateExportForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.exportTargetFolder = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.exportAsOverlay = new System.Windows.Forms.CheckBox();
            this.selectWallNumber = new System.Windows.Forms.ComboBox();
            this.selectTemplateWallNumber = new System.Windows.Forms.ComboBox();
            this.templateFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.templatePreview = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.exportFileName = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.templatePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Folder:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // exportTargetFolder
            // 
            this.exportTargetFolder.Location = new System.Drawing.Point(16, 29);
            this.exportTargetFolder.Name = "exportTargetFolder";
            this.exportTargetFolder.ReadOnly = true;
            this.exportTargetFolder.Size = new System.Drawing.Size(256, 20);
            this.exportTargetFolder.TabIndex = 1;
            this.exportTargetFolder.Click += new System.EventHandler(this.textBox1_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(372, 251);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(189, 251);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Export";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.TextChanged += new System.EventHandler(this.maybe);
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Wall Number";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Against Wall Number";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // exportAsOverlay
            // 
            this.exportAsOverlay.AutoSize = true;
            this.exportAsOverlay.Location = new System.Drawing.Point(12, 202);
            this.exportAsOverlay.Name = "exportAsOverlay";
            this.exportAsOverlay.Size = new System.Drawing.Size(109, 17);
            this.exportAsOverlay.TabIndex = 9;
            this.exportAsOverlay.Text = "Export as Overlay";
            this.exportAsOverlay.UseVisualStyleBackColor = true;
            this.exportAsOverlay.CheckedChanged += new System.EventHandler(this.exportAsOverlay_CheckedChanged);
            // 
            // selectWallNumber
            // 
            this.selectWallNumber.FormattingEnabled = true;
            this.selectWallNumber.Location = new System.Drawing.Point(121, 167);
            this.selectWallNumber.Name = "selectWallNumber";
            this.selectWallNumber.Size = new System.Drawing.Size(121, 21);
            this.selectWallNumber.TabIndex = 10;
            this.selectWallNumber.SelectedIndexChanged += new System.EventHandler(this.selectWallNumber_SelectedIndexChanged);
            // 
            // selectTemplateWallNumber
            // 
            this.selectTemplateWallNumber.Enabled = false;
            this.selectTemplateWallNumber.FormattingEnabled = true;
            this.selectTemplateWallNumber.Location = new System.Drawing.Point(120, 224);
            this.selectTemplateWallNumber.Name = "selectTemplateWallNumber";
            this.selectTemplateWallNumber.Size = new System.Drawing.Size(121, 21);
            this.selectTemplateWallNumber.TabIndex = 11;
            this.selectTemplateWallNumber.SelectedIndexChanged += new System.EventHandler(this.selectTemplateWallNumber_SelectedIndexChanged);
            // 
            // templateFile
            // 
            this.templateFile.Location = new System.Drawing.Point(16, 125);
            this.templateFile.Name = "templateFile";
            this.templateFile.ReadOnly = true;
            this.templateFile.Size = new System.Drawing.Size(256, 20);
            this.templateFile.TabIndex = 12;
            this.templateFile.Click += new System.EventHandler(this.templateFile_Click);
            this.templateFile.TextChanged += new System.EventHandler(this.templateFile_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Use Template File:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // templatePreview
            // 
            this.templatePreview.Location = new System.Drawing.Point(278, 29);
            this.templatePreview.Name = "templatePreview";
            this.templatePreview.Size = new System.Drawing.Size(306, 204);
            this.templatePreview.TabIndex = 13;
            this.templatePreview.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "File Name:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // exportFileName
            // 
            this.exportFileName.Location = new System.Drawing.Point(15, 77);
            this.exportFileName.Name = "exportFileName";
            this.exportFileName.Size = new System.Drawing.Size(256, 20);
            this.exportFileName.TabIndex = 12;
            this.exportFileName.TextChanged += new System.EventHandler(this.exportFileName_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(278, 251);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "Export All";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // WallTemplateExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 309);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.templatePreview);
            this.Controls.Add(this.exportFileName);
            this.Controls.Add(this.templateFile);
            this.Controls.Add(this.selectTemplateWallNumber);
            this.Controls.Add(this.selectWallNumber);
            this.Controls.Add(this.exportAsOverlay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.exportTargetFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "WallTemplateExportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export Images";
            ((System.ComponentModel.ISupportInitialize)(this.templatePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox exportTargetFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox exportAsOverlay;
        private System.Windows.Forms.ComboBox selectWallNumber;
        private System.Windows.Forms.ComboBox selectTemplateWallNumber;
        private System.Windows.Forms.TextBox templateFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox templatePreview;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox exportFileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button3;
    }
}