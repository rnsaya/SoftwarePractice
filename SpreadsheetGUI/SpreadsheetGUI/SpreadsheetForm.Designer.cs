using System;

namespace SpreadsheetGUI
{
    partial class SpreadsheetForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.valueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scrollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mouseClickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arrowKeysToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSpreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeSpreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.shortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCtrlSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCtrlNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCtrlOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentTextBox = new System.Windows.Forms.TextBox();
            this.contentLabel = new System.Windows.Forms.Label();
            this.valueLabel = new System.Windows.Forms.Label();
            this.cellValueLabel = new System.Windows.Forms.Label();
            this.currentCellLabel = new System.Windows.Forms.Label();
            this.cellLabel = new System.Windows.Forms.Label();
            this.ssInnards = new System.ComponentModel.BackgroundWorker();
            this.GoToCellLabel = new System.Windows.Forms.Label();
            this.goToCellTextBox = new System.Windows.Forms.TextBox();
            this.spreadsheetPanel = new SS.SpreadsheetPanel();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1108, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileButton,
            this.openFileButton,
            this.saveFileButton,
            this.saveAsToolStripMenuItem,
            this.closeFileButton});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(44, 24);
            this.fileMenu.Text = "File";
            // 
            // newFileButton
            // 
            this.newFileButton.Name = "newFileButton";
            this.newFileButton.Size = new System.Drawing.Size(120, 26);
            this.newFileButton.Text = "New";
            this.newFileButton.Click += new System.EventHandler(this.newFileButton_Click);
            // 
            // openFileButton
            // 
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(120, 26);
            this.openFileButton.Text = "Open";
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // saveFileButton
            // 
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(120, 26);
            this.saveFileButton.Text = "Save";
            this.saveFileButton.Click += new System.EventHandler(this.saveFileButton_Click);
            // 
            // closeFileButton
            // 
            this.closeFileButton.Name = "closeFileButton";
            this.closeFileButton.Size = new System.Drawing.Size(120, 26);
            this.closeFileButton.Text = "Close";
            this.closeFileButton.Click += new System.EventHandler(this.closeFileButton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCellToolStripMenuItem,
            this.scrollToolStripMenuItem,
            this.fileMenuToolStripMenuItem,
            this.toolStripSeparator1,
            this.shortcutsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // editCellToolStripMenuItem
            // 
            this.editCellToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.valueToolStripMenuItem,
            this.cellNameToolStripMenuItem});
            this.editCellToolStripMenuItem.Name = "editCellToolStripMenuItem";
            this.editCellToolStripMenuItem.Size = new System.Drawing.Size(148, 26);
            this.editCellToolStripMenuItem.Text = "Edit Cell";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.contentsToolStripMenuItem.Text = "Contents";
            this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
            // 
            // valueToolStripMenuItem
            // 
            this.valueToolStripMenuItem.Name = "valueToolStripMenuItem";
            this.valueToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.valueToolStripMenuItem.Text = "Value";
            this.valueToolStripMenuItem.Click += new System.EventHandler(this.valueToolStripMenuItem_Click);
            // 
            // cellNameToolStripMenuItem
            // 
            this.cellNameToolStripMenuItem.Name = "cellNameToolStripMenuItem";
            this.cellNameToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.cellNameToolStripMenuItem.Text = "Cell Name";
            this.cellNameToolStripMenuItem.Click += new System.EventHandler(this.cellNameToolStripMenuItem_Click);
            // 
            // scrollToolStripMenuItem
            // 
            this.scrollToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mouseClickToolStripMenuItem,
            this.goToToolStripMenuItem,
            this.arrowKeysToolStripMenuItem1});
            this.scrollToolStripMenuItem.Name = "scrollToolStripMenuItem";
            this.scrollToolStripMenuItem.Size = new System.Drawing.Size(148, 26);
            this.scrollToolStripMenuItem.Text = "Navigate";
            // 
            // mouseClickToolStripMenuItem
            // 
            this.mouseClickToolStripMenuItem.Name = "mouseClickToolStripMenuItem";
            this.mouseClickToolStripMenuItem.Size = new System.Drawing.Size(163, 26);
            this.mouseClickToolStripMenuItem.Text = "Mouse Click";
            this.mouseClickToolStripMenuItem.Click += new System.EventHandler(this.mouseClickToolStripMenuItem_Click);
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(163, 26);
            this.goToToolStripMenuItem.Text = "Go To Cell";
            this.goToToolStripMenuItem.Click += new System.EventHandler(this.goToToolStripMenuItem_Click);
            // 
            // arrowKeysToolStripMenuItem1
            // 
            this.arrowKeysToolStripMenuItem1.Name = "arrowKeysToolStripMenuItem1";
            this.arrowKeysToolStripMenuItem1.Size = new System.Drawing.Size(163, 26);
            this.arrowKeysToolStripMenuItem1.Text = "Arrow Keys";
            this.arrowKeysToolStripMenuItem1.Click += new System.EventHandler(this.arrowKeysToolStripMenuItem1_Click);
            // 
            // fileMenuToolStripMenuItem
            // 
            this.fileMenuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSpreadsheetToolStripMenuItem,
            this.openFileToolStripMenuItem,
            this.saveFileToolStripMenuItem,
            this.closeSpreadsheetToolStripMenuItem});
            this.fileMenuToolStripMenuItem.Name = "fileMenuToolStripMenuItem";
            this.fileMenuToolStripMenuItem.Size = new System.Drawing.Size(148, 26);
            this.fileMenuToolStripMenuItem.Text = "File Menu";
            // 
            // newSpreadsheetToolStripMenuItem
            // 
            this.newSpreadsheetToolStripMenuItem.Name = "newSpreadsheetToolStripMenuItem";
            this.newSpreadsheetToolStripMenuItem.Size = new System.Drawing.Size(206, 26);
            this.newSpreadsheetToolStripMenuItem.Text = "New Spreadsheet";
            this.newSpreadsheetToolStripMenuItem.Click += new System.EventHandler(this.newSpreadsheetToolStripMenuItem_Click);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(206, 26);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // saveFileToolStripMenuItem
            // 
            this.saveFileToolStripMenuItem.Name = "saveFileToolStripMenuItem";
            this.saveFileToolStripMenuItem.Size = new System.Drawing.Size(206, 26);
            this.saveFileToolStripMenuItem.Text = "Save File";
            this.saveFileToolStripMenuItem.Click += new System.EventHandler(this.saveFileToolStripMenuItem_Click);
            // 
            // closeSpreadsheetToolStripMenuItem
            // 
            this.closeSpreadsheetToolStripMenuItem.Name = "closeSpreadsheetToolStripMenuItem";
            this.closeSpreadsheetToolStripMenuItem.Size = new System.Drawing.Size(206, 26);
            this.closeSpreadsheetToolStripMenuItem.Text = "Close Spreadsheet";
            this.closeSpreadsheetToolStripMenuItem.Click += new System.EventHandler(this.closeSpreadsheetToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // shortcutsToolStripMenuItem
            // 
            this.shortcutsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveCtrlSToolStripMenuItem,
            this.newCtrlNToolStripMenuItem,
            this.openCtrlOToolStripMenuItem});
            this.shortcutsToolStripMenuItem.Name = "shortcutsToolStripMenuItem";
            this.shortcutsToolStripMenuItem.Size = new System.Drawing.Size(148, 26);
            this.shortcutsToolStripMenuItem.Text = "Shortcuts";
            // 
            // saveCtrlSToolStripMenuItem
            // 
            this.saveCtrlSToolStripMenuItem.Name = "saveCtrlSToolStripMenuItem";
            this.saveCtrlSToolStripMenuItem.Size = new System.Drawing.Size(175, 26);
            this.saveCtrlSToolStripMenuItem.Text = "Save: ctrl + s";
            // 
            // newCtrlNToolStripMenuItem
            // 
            this.newCtrlNToolStripMenuItem.Name = "newCtrlNToolStripMenuItem";
            this.newCtrlNToolStripMenuItem.Size = new System.Drawing.Size(175, 26);
            this.newCtrlNToolStripMenuItem.Text = "New: ctrl + n";
            // 
            // openCtrlOToolStripMenuItem
            // 
            this.openCtrlOToolStripMenuItem.Name = "openCtrlOToolStripMenuItem";
            this.openCtrlOToolStripMenuItem.Size = new System.Drawing.Size(175, 26);
            this.openCtrlOToolStripMenuItem.Text = "Open: ctrl + o";
            // 
            // contentTextBox
            // 
            this.contentTextBox.Location = new System.Drawing.Point(57, 55);
            this.contentTextBox.Name = "contentTextBox";
            this.contentTextBox.Size = new System.Drawing.Size(103, 22);
            this.contentTextBox.TabIndex = 3;
            this.contentTextBox.TextChanged += new System.EventHandler(this.contentTextBox_TextChanged);
            this.contentTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.contentTextBox_EnterPressed);
            this.contentTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.contentTextBox_PreviewKeyDown);
            // 
            // contentLabel
            // 
            this.contentLabel.AutoSize = true;
            this.contentLabel.Font = new System.Drawing.Font("Lucida Calligraphy", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contentLabel.Location = new System.Drawing.Point(12, 55);
            this.contentLabel.Name = "contentLabel";
            this.contentLabel.Size = new System.Drawing.Size(39, 19);
            this.contentLabel.TabIndex = 4;
            this.contentLabel.Text = "fx =";
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(12, 81);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(48, 17);
            this.valueLabel.TabIndex = 5;
            this.valueLabel.Text = "Value:";
            // 
            // cellValueLabel
            // 
            this.cellValueLabel.AutoSize = true;
            this.cellValueLabel.Location = new System.Drawing.Point(62, 82);
            this.cellValueLabel.Name = "cellValueLabel";
            this.cellValueLabel.Size = new System.Drawing.Size(0, 17);
            this.cellValueLabel.TabIndex = 6;
            // 
            // currentCellLabel
            // 
            this.currentCellLabel.AutoSize = true;
            this.currentCellLabel.Location = new System.Drawing.Point(94, 34);
            this.currentCellLabel.Name = "currentCellLabel";
            this.currentCellLabel.Size = new System.Drawing.Size(25, 17);
            this.currentCellLabel.TabIndex = 7;
            this.currentCellLabel.Text = "A1";
            // 
            // cellLabel
            // 
            this.cellLabel.AutoSize = true;
            this.cellLabel.Location = new System.Drawing.Point(12, 34);
            this.cellLabel.Name = "cellLabel";
            this.cellLabel.Size = new System.Drawing.Size(76, 17);
            this.cellLabel.TabIndex = 8;
            this.cellLabel.Text = "Cell Name:";
            // 
            // ssInnards
            // 
            this.ssInnards.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ssInnards_DoWork);
            this.ssInnards.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ssInnards_RunWorkerCompleted);
            // 
            // GoToCellLabel
            // 
            this.GoToCellLabel.AutoSize = true;
            this.GoToCellLabel.Location = new System.Drawing.Point(159, 34);
            this.GoToCellLabel.Name = "GoToCellLabel";
            this.GoToCellLabel.Size = new System.Drawing.Size(83, 17);
            this.GoToCellLabel.TabIndex = 10;
            this.GoToCellLabel.Text = "Go To Cell: ";
            // 
            // goToCellTextBox
            // 
            this.goToCellTextBox.Location = new System.Drawing.Point(248, 31);
            this.goToCellTextBox.Name = "goToCellTextBox";
            this.goToCellTextBox.Size = new System.Drawing.Size(77, 22);
            this.goToCellTextBox.TabIndex = 11;
            this.goToCellTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.goToCellTextBox_EnterPressed);
            // 
            // spreadsheetPanel
            // 
            this.spreadsheetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel.Location = new System.Drawing.Point(0, 104);
            this.spreadsheetPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(1108, 600);
            this.spreadsheetPanel.TabIndex = 9;
            this.spreadsheetPanel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.SpreadsheetPanel_PreviewKeyDown);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // SpreadsheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1108, 701);
            this.Controls.Add(this.goToCellTextBox);
            this.Controls.Add(this.GoToCellLabel);
            this.Controls.Add(this.spreadsheetPanel);
            this.Controls.Add(this.cellLabel);
            this.Controls.Add(this.currentCellLabel);
            this.Controls.Add(this.cellValueLabel);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.contentLabel);
            this.Controls.Add(this.contentTextBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(199, 198);
            this.Name = "SpreadsheetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spreadsheet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem newFileButton;
        private System.Windows.Forms.ToolStripMenuItem openFileButton;
        private System.Windows.Forms.ToolStripMenuItem saveFileButton;
        private System.Windows.Forms.ToolStripMenuItem closeFileButton;
        private System.Windows.Forms.TextBox contentTextBox;
        private System.Windows.Forms.Label contentLabel;
        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.Label cellValueLabel;
        private System.Windows.Forms.Label currentCellLabel;
        private System.Windows.Forms.Label cellLabel;
        private System.ComponentModel.BackgroundWorker ssInnards;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editCellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem valueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cellNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scrollToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arrowKeysToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fileMenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSpreadsheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeSpreadsheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mouseClickToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem shortcutsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCtrlSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newCtrlNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openCtrlOToolStripMenuItem;
        private SS.SpreadsheetPanel spreadsheetPanel;
        private System.Windows.Forms.Label GoToCellLabel;
        private System.Windows.Forms.TextBox goToCellTextBox;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    }
}

