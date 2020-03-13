namespace vfr2cfr
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.testButton = new System.Windows.Forms.Button();
            this.outButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openFilesList = new System.Windows.Forms.ListBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(108, 264);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 0;
            this.testButton.Text = "open";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // outButton
            // 
            this.outButton.Enabled = false;
            this.outButton.Location = new System.Drawing.Point(404, 264);
            this.outButton.Name = "outButton";
            this.outButton.Size = new System.Drawing.Size(75, 23);
            this.outButton.TabIndex = 1;
            this.outButton.Text = "convert";
            this.outButton.UseVisualStyleBackColor = true;
            this.outButton.Click += new System.EventHandler(this.OutButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Video Files|*.mp4;*.avi|All Files|*.*";
            this.openFileDialog.Multiselect = true;
            // 
            // openFilesList
            // 
            this.openFilesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openFilesList.FormattingEnabled = true;
            this.openFilesList.HorizontalScrollbar = true;
            this.openFilesList.ItemHeight = 12;
            this.openFilesList.Location = new System.Drawing.Point(12, 17);
            this.openFilesList.Name = "openFilesList";
            this.openFilesList.Size = new System.Drawing.Size(560, 112);
            this.openFilesList.TabIndex = 2;
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox.Location = new System.Drawing.Point(12, 135);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(560, 123);
            this.textBox.TabIndex = 3;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 295);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.openFilesList);
            this.Controls.Add(this.outButton);
            this.Controls.Add(this.testButton);
            this.MinimumSize = new System.Drawing.Size(300, 39);
            this.Name = "FormMain";
            this.Text = "vfr2cfr";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.Button outButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ListBox openFilesList;
        private System.Windows.Forms.TextBox textBox;
    }
}

