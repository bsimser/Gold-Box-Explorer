//namespace GoldBoxExplorer
namespace GoldBoxExplorer.Lib.Plugins.GeoDax
{
    partial class GeoExportForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.formatBmp = new System.Windows.Forms.RadioButton();
            this.formatJpg = new System.Windows.Forms.RadioButton();
            this.formatPng = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(256, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.formatPng);
            this.groupBox1.Controls.Add(this.formatJpg);
            this.groupBox1.Controls.Add(this.formatBmp);
            this.groupBox1.Location = new System.Drawing.Point(16, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 98);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target Format";
            // 
            // formatBmp
            // 
            this.formatBmp.AutoSize = true;
            this.formatBmp.Checked = true;
            this.formatBmp.Location = new System.Drawing.Point(7, 20);
            this.formatBmp.Name = "formatBmp";
            this.formatBmp.Size = new System.Drawing.Size(48, 17);
            this.formatBmp.TabIndex = 0;
            this.formatBmp.TabStop = true;
            this.formatBmp.Text = "BMP";
            this.formatBmp.UseVisualStyleBackColor = true;
            // 
            // formatJpg
            // 
            this.formatJpg.AutoSize = true;
            this.formatJpg.Location = new System.Drawing.Point(7, 44);
            this.formatJpg.Name = "formatJpg";
            this.formatJpg.Size = new System.Drawing.Size(45, 17);
            this.formatJpg.TabIndex = 1;
            this.formatJpg.Text = "JPG";
            this.formatJpg.UseVisualStyleBackColor = true;
            // 
            // formatPng
            // 
            this.formatPng.AutoSize = true;
            this.formatPng.Location = new System.Drawing.Point(7, 67);
            this.formatPng.Name = "formatPng";
            this.formatPng.Size = new System.Drawing.Size(48, 17);
            this.formatPng.TabIndex = 2;
            this.formatPng.Text = "PNG";
            this.formatPng.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(197, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(116, 159);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 196);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ExportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export Images";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton formatPng;
        private System.Windows.Forms.RadioButton formatJpg;
        private System.Windows.Forms.RadioButton formatBmp;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}