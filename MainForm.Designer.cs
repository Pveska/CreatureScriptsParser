namespace CreatureScriptsParser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip_Top = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_ImportSniff = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorSecond = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Search = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox_CreatureGuid = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel_CreatureGuid = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator_First = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip_Low = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_FileStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_CurrentAction = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.textBox_Output = new System.Windows.Forms.TextBox();
            this.checkBox_CreateDataFile = new System.Windows.Forms.CheckBox();
            this.toolStrip_Top.SuspendLayout();
            this.toolStrip_Low.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip_Top
            // 
            this.toolStrip_Top.BackColor = System.Drawing.Color.LightGray;
            this.toolStrip_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_ImportSniff,
            this.toolStripSeparatorSecond,
            this.toolStripButton_Search,
            this.toolStripTextBox_CreatureGuid,
            this.toolStripLabel_CreatureGuid,
            this.toolStripSeparator_First});
            this.toolStrip_Top.Location = new System.Drawing.Point(0, 0);
            this.toolStrip_Top.Name = "toolStrip_Top";
            this.toolStrip_Top.Size = new System.Drawing.Size(1838, 38);
            this.toolStrip_Top.TabIndex = 2;
            // 
            // toolStripButton_ImportSniff
            // 
            this.toolStripButton_ImportSniff.Image = global::CreatureScriptsParser.Properties.Resources.PIC_Import;
            this.toolStripButton_ImportSniff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ImportSniff.Name = "toolStripButton_ImportSniff";
            this.toolStripButton_ImportSniff.Size = new System.Drawing.Size(128, 33);
            this.toolStripButton_ImportSniff.Text = "Import Sniff";
            this.toolStripButton_ImportSniff.Click += new System.EventHandler(this.toolStripButton_ImportSniff_Click);
            // 
            // toolStripSeparatorSecond
            // 
            this.toolStripSeparatorSecond.Name = "toolStripSeparatorSecond";
            this.toolStripSeparatorSecond.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripButton_Search
            // 
            this.toolStripButton_Search.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_Search.Enabled = false;
            this.toolStripButton_Search.Image = global::CreatureScriptsParser.Properties.Resources.PIC_Search;
            this.toolStripButton_Search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Search.Name = "toolStripButton_Search";
            this.toolStripButton_Search.Size = new System.Drawing.Size(84, 33);
            this.toolStripButton_Search.Text = "Search";
            this.toolStripButton_Search.Click += new System.EventHandler(this.toolStripButton_Search_Click);
            // 
            // toolStripTextBox_CreatureGuid
            // 
            this.toolStripTextBox_CreatureGuid.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBox_CreatureGuid.Enabled = false;
            this.toolStripTextBox_CreatureGuid.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox_CreatureGuid.MaxLength = 40;
            this.toolStripTextBox_CreatureGuid.Name = "toolStripTextBox_CreatureGuid";
            this.toolStripTextBox_CreatureGuid.Size = new System.Drawing.Size(320, 38);
            // 
            // toolStripLabel_CreatureGuid
            // 
            this.toolStripLabel_CreatureGuid.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel_CreatureGuid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripLabel_CreatureGuid.Name = "toolStripLabel_CreatureGuid";
            this.toolStripLabel_CreatureGuid.Size = new System.Drawing.Size(124, 33);
            this.toolStripLabel_CreatureGuid.Text = "Creature Guid:";
            // 
            // toolStripSeparator_First
            // 
            this.toolStripSeparator_First.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator_First.Name = "toolStripSeparator_First";
            this.toolStripSeparator_First.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStrip_Low
            // 
            this.toolStrip_Low.BackColor = System.Drawing.Color.LightGray;
            this.toolStrip_Low.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_FileStatus,
            this.toolStripStatusLabel_CurrentAction});
            this.toolStrip_Low.Location = new System.Drawing.Point(0, 1106);
            this.toolStrip_Low.Name = "toolStrip_Low";
            this.toolStrip_Low.Padding = new System.Windows.Forms.Padding(2, 0, 14, 0);
            this.toolStrip_Low.Size = new System.Drawing.Size(1838, 32);
            this.toolStrip_Low.TabIndex = 3;
            // 
            // toolStripStatusLabel_FileStatus
            // 
            this.toolStripStatusLabel_FileStatus.Name = "toolStripStatusLabel_FileStatus";
            this.toolStripStatusLabel_FileStatus.Size = new System.Drawing.Size(131, 25);
            this.toolStripStatusLabel_FileStatus.Text = "No File Loaded";
            // 
            // toolStripStatusLabel_CurrentAction
            // 
            this.toolStripStatusLabel_CurrentAction.Name = "toolStripStatusLabel_CurrentAction";
            this.toolStripStatusLabel_CurrentAction.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripStatusLabel_CurrentAction.Size = new System.Drawing.Size(0, 25);
            // 
            // textBox_Output
            // 
            this.textBox_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Output.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_Output.Location = new System.Drawing.Point(18, 58);
            this.textBox_Output.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_Output.Multiline = true;
            this.textBox_Output.Name = "textBox_Output";
            this.textBox_Output.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Output.Size = new System.Drawing.Size(1800, 1025);
            this.textBox_Output.TabIndex = 4;
            // 
            // checkBox_CreateDataFile
            // 
            this.checkBox_CreateDataFile.AutoSize = true;
            this.checkBox_CreateDataFile.BackColor = System.Drawing.Color.LightGray;
            this.checkBox_CreateDataFile.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_CreateDataFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_CreateDataFile.Location = new System.Drawing.Point(820, 4);
            this.checkBox_CreateDataFile.Name = "checkBox_CreateDataFile";
            this.checkBox_CreateDataFile.Size = new System.Drawing.Size(161, 29);
            this.checkBox_CreateDataFile.TabIndex = 5;
            this.checkBox_CreateDataFile.Text = "Create Data File";
            this.checkBox_CreateDataFile.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1838, 1138);
            this.Controls.Add(this.checkBox_CreateDataFile);
            this.Controls.Add(this.textBox_Output);
            this.Controls.Add(this.toolStrip_Low);
            this.Controls.Add(this.toolStrip_Top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Creature Scripts Parser";
            this.toolStrip_Top.ResumeLayout(false);
            this.toolStrip_Top.PerformLayout();
            this.toolStrip_Low.ResumeLayout(false);
            this.toolStrip_Low.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip_Top;
        public System.Windows.Forms.ToolStripButton toolStripButton_ImportSniff;
        public System.Windows.Forms.ToolStripButton toolStripButton_Search;
        public System.Windows.Forms.ToolStripTextBox toolStripTextBox_CreatureGuid;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_CreatureGuid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_First;
        private System.Windows.Forms.StatusStrip toolStrip_Low;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_FileStatus;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_CurrentAction;
        public System.Windows.Forms.OpenFileDialog openFileDialog;
        public System.Windows.Forms.TextBox textBox_Output;
        public System.Windows.Forms.CheckBox checkBox_CreateDataFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorSecond;
    }
}

