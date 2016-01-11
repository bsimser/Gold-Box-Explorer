namespace GoldBoxExplorer.Lib
{
    partial class ExceptionDialog
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
            this.btnMore = new System.Windows.Forms.Button();
            this.txtMore = new System.Windows.Forms.TextBox();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.lblMoreHeading = new System.Windows.Forms.Label();
            this.lblActionHeading = new System.Windows.Forms.Label();
            this.lblScopeHeading = new System.Windows.Forms.Label();
            this.lblErrorHeading = new System.Windows.Forms.Label();
            this.ActionBox = new System.Windows.Forms.RichTextBox();
            this.ScopeBox = new System.Windows.Forms.RichTextBox();
            this.ErrorBox = new System.Windows.Forms.RichTextBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMore
            // 
            this.btnMore.Location = new System.Drawing.Point(112, 297);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(28, 24);
            this.btnMore.TabIndex = 20;
            this.btnMore.Text = ">>";
            this.btnMore.Click += new System.EventHandler(this.BtnMoreClick);
            // 
            // txtMore
            // 
            this.txtMore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMore.CausesValidation = false;
            this.txtMore.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMore.Location = new System.Drawing.Point(8, 325);
            this.txtMore.Multiline = true;
            this.txtMore.Name = "txtMore";
            this.txtMore.ReadOnly = true;
            this.txtMore.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMore.Size = new System.Drawing.Size(456, 212);
            this.txtMore.TabIndex = 21;
            this.txtMore.Text = "(detailed information, such as exception details)";
            // 
            // btn3
            // 
            this.btn3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn3.Location = new System.Drawing.Point(388, 545);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(75, 23);
            this.btn3.TabIndex = 24;
            this.btn3.Text = "Button3";
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btn2
            // 
            this.btn2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn2.Location = new System.Drawing.Point(304, 545);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(75, 23);
            this.btn2.TabIndex = 23;
            this.btn2.Text = "Button2";
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btn1
            // 
            this.btn1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn1.Location = new System.Drawing.Point(220, 545);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(75, 23);
            this.btn1.TabIndex = 22;
            this.btn1.Text = "Button1";
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // lblMoreHeading
            // 
            this.lblMoreHeading.AutoSize = true;
            this.lblMoreHeading.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblMoreHeading.Location = new System.Drawing.Point(8, 301);
            this.lblMoreHeading.Name = "lblMoreHeading";
            this.lblMoreHeading.Size = new System.Drawing.Size(105, 13);
            this.lblMoreHeading.TabIndex = 19;
            this.lblMoreHeading.Text = "More information";
            // 
            // lblActionHeading
            // 
            this.lblActionHeading.AutoSize = true;
            this.lblActionHeading.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblActionHeading.Location = new System.Drawing.Point(8, 181);
            this.lblActionHeading.Name = "lblActionHeading";
            this.lblActionHeading.Size = new System.Drawing.Size(148, 13);
            this.lblActionHeading.TabIndex = 17;
            this.lblActionHeading.Text = "What you can do about it";
            // 
            // lblScopeHeading
            // 
            this.lblScopeHeading.AutoSize = true;
            this.lblScopeHeading.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblScopeHeading.Location = new System.Drawing.Point(8, 93);
            this.lblScopeHeading.Name = "lblScopeHeading";
            this.lblScopeHeading.Size = new System.Drawing.Size(136, 13);
            this.lblScopeHeading.TabIndex = 15;
            this.lblScopeHeading.Text = "How this will affect you";
            // 
            // lblErrorHeading
            // 
            this.lblErrorHeading.AutoSize = true;
            this.lblErrorHeading.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblErrorHeading.Location = new System.Drawing.Point(48, 5);
            this.lblErrorHeading.Name = "lblErrorHeading";
            this.lblErrorHeading.Size = new System.Drawing.Size(96, 13);
            this.lblErrorHeading.TabIndex = 12;
            this.lblErrorHeading.Text = "What happened";
            // 
            // ActionBox
            // 
            this.ActionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionBox.BackColor = System.Drawing.SystemColors.Control;
            this.ActionBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ActionBox.CausesValidation = false;
            this.ActionBox.Location = new System.Drawing.Point(24, 201);
            this.ActionBox.Name = "ActionBox";
            this.ActionBox.ReadOnly = true;
            this.ActionBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ActionBox.Size = new System.Drawing.Size(440, 92);
            this.ActionBox.TabIndex = 18;
            this.ActionBox.Text = "(action)";
            // 
            // ScopeBox
            // 
            this.ScopeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ScopeBox.BackColor = System.Drawing.SystemColors.Control;
            this.ScopeBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ScopeBox.CausesValidation = false;
            this.ScopeBox.Location = new System.Drawing.Point(24, 113);
            this.ScopeBox.Name = "ScopeBox";
            this.ScopeBox.ReadOnly = true;
            this.ScopeBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ScopeBox.Size = new System.Drawing.Size(440, 64);
            this.ScopeBox.TabIndex = 16;
            this.ScopeBox.Text = "(scope)";
            // 
            // ErrorBox
            // 
            this.ErrorBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorBox.BackColor = System.Drawing.SystemColors.Control;
            this.ErrorBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ErrorBox.CausesValidation = false;
            this.ErrorBox.Location = new System.Drawing.Point(48, 25);
            this.ErrorBox.Name = "ErrorBox";
            this.ErrorBox.ReadOnly = true;
            this.ErrorBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ErrorBox.Size = new System.Drawing.Size(416, 64);
            this.ErrorBox.TabIndex = 14;
            this.ErrorBox.Text = "(error message)";
            // 
            // PictureBox1
            // 
            this.PictureBox1.Location = new System.Drawing.Point(8, 9);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(32, 32);
            this.PictureBox1.TabIndex = 13;
            this.PictureBox1.TabStop = false;
            // 
            // ExceptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 573);
            this.Controls.Add(this.btnMore);
            this.Controls.Add(this.txtMore);
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.lblMoreHeading);
            this.Controls.Add(this.lblActionHeading);
            this.Controls.Add(this.lblScopeHeading);
            this.Controls.Add(this.lblErrorHeading);
            this.Controls.Add(this.ActionBox);
            this.Controls.Add(this.ScopeBox);
            this.Controls.Add(this.ErrorBox);
            this.Controls.Add(this.PictureBox1);
            this.Name = "ExceptionDialog";
            this.Text = "(app) has encountered a problem";
            this.Load += new System.EventHandler(this.ExceptionDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnMore;
        internal System.Windows.Forms.TextBox txtMore;
        internal System.Windows.Forms.Button btn3;
        internal System.Windows.Forms.Button btn2;
        internal System.Windows.Forms.Button btn1;
        internal System.Windows.Forms.Label lblMoreHeading;
        internal System.Windows.Forms.Label lblActionHeading;
        internal System.Windows.Forms.Label lblScopeHeading;
        internal System.Windows.Forms.Label lblErrorHeading;
        internal System.Windows.Forms.RichTextBox ActionBox;
        internal System.Windows.Forms.RichTextBox ScopeBox;
        internal System.Windows.Forms.RichTextBox ErrorBox;
        internal System.Windows.Forms.PictureBox PictureBox1;

    }
}