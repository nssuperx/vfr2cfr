using System.Windows.Forms;

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
            this.components = new System.ComponentModel.Container();
            this.openButton = new System.Windows.Forms.Button();
            this.outButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openFilesList = new System.Windows.Forms.ListBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.fpsButton = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.openButton.Location = new System.Drawing.Point(12, 280);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 27);
            this.openButton.TabIndex = 0;
            this.openButton.Text = "開く";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // outButton
            // 
            this.outButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.outButton.Enabled = false;
            this.outButton.Location = new System.Drawing.Point(197, 280);
            this.outButton.Name = "outButton";
            this.outButton.Size = new System.Drawing.Size(75, 27);
            this.outButton.TabIndex = 1;
            this.outButton.Text = "変換";
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
            this.openFilesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openFilesList.FormattingEnabled = true;
            this.openFilesList.HorizontalScrollbar = true;
            this.openFilesList.ItemHeight = 12;
            this.openFilesList.Location = new System.Drawing.Point(0, 0);
            this.openFilesList.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.openFilesList.Name = "openFilesList";
            this.openFilesList.Size = new System.Drawing.Size(260, 124);
            this.openFilesList.TabIndex = 2;
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox.Location = new System.Drawing.Point(0, 132);
            this.textBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(260, 123);
            this.textBox.TabIndex = 3;
            this.textBox.WordWrap = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 339);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(284, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(269, 17);
            this.toolStripStatusLabel.Spring = true;
            this.toolStripStatusLabel.Text = "  ";
            this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 313);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(260, 23);
            this.progressBar.TabIndex = 5;
            // 
            // timer
            // 
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.openFilesList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(260, 258);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // fpsButton
            // 
            this.fpsButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.fpsButton.Location = new System.Drawing.Point(105, 280);
            this.fpsButton.Name = "fpsButton";
            this.fpsButton.Size = new System.Drawing.Size(75, 27);
            this.fpsButton.TabIndex = 2;
            this.fpsButton.Text = "60 fps";
            this.fpsButton.UseVisualStyleBackColor = true;
            this.fpsButton.Click += new System.EventHandler(this.FpsButton_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 361);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.fpsButton);
            this.Controls.Add(this.outButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.statusStrip);
            this.MinimumSize = new System.Drawing.Size(300, 400);
            this.Name = "FormMain";
            this.Text = "vfr2cfr";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button outButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ListBox openFilesList;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ProgressBar progressBar;
        private Timer timer;
        private ToolStripStatusLabel toolStripStatusLabel;
        private TableLayoutPanel tableLayoutPanel1;
        private Button fpsButton;
    }
}

