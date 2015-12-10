using System.Windows.Forms;

namespace View
{
    partial class form
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
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.FPSLabel = new System.Windows.Forms.Label();
            this.FoodLabel = new System.Windows.Forms.Label();
            this.MassLabel = new System.Windows.Forms.Label();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.FPSValue = new System.Windows.Forms.Label();
            this.FoodValue = new System.Windows.Forms.Label();
            this.MassValue = new System.Windows.Forms.Label();
            this.WidthValue = new System.Windows.Forms.Label();
            this.UsernameTextbox = new System.Windows.Forms.TextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.ServerTextbox = new System.Windows.Forms.TextBox();
            this.GameOverP = new System.Windows.Forms.Panel();
            this.FoodEatenLabel = new System.Windows.Forms.Label();
            this.FoodEatenValue = new System.Windows.Forms.Label();
            this.ResetButton = new System.Windows.Forms.Button();
            this.lastMassLabel = new System.Windows.Forms.Label();
            this.lastMassValue = new System.Windows.Forms.Label();
            this.agCubio = new System.Windows.Forms.Panel();
            this.statisticsPanel = new System.Windows.Forms.Panel();
            this.statisticsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(433, 411);
            this.UsernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(140, 20);
            this.UsernameLabel.TabIndex = 9;
            this.UsernameLabel.Text = "Enter _username :";
            // 
            // FPSLabel
            // 
            this.FPSLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FPSLabel.AutoSize = true;
            this.FPSLabel.Location = new System.Drawing.Point(4, 21);
            this.FPSLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FPSLabel.Name = "FPSLabel";
            this.FPSLabel.Size = new System.Drawing.Size(40, 20);
            this.FPSLabel.TabIndex = 1;
            this.FPSLabel.Text = "FPS";
            this.FPSLabel.Visible = false;
            // 
            // FoodLabel
            // 
            this.FoodLabel.AutoSize = true;
            this.FoodLabel.Location = new System.Drawing.Point(4, 56);
            this.FoodLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FoodLabel.Name = "FoodLabel";
            this.FoodLabel.Size = new System.Drawing.Size(46, 20);
            this.FoodLabel.TabIndex = 2;
            this.FoodLabel.Text = "Food";
            this.FoodLabel.Visible = false;
            // 
            // MassLabel
            // 
            this.MassLabel.AutoSize = true;
            this.MassLabel.Location = new System.Drawing.Point(4, 91);
            this.MassLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MassLabel.Name = "MassLabel";
            this.MassLabel.Size = new System.Drawing.Size(47, 20);
            this.MassLabel.TabIndex = 3;
            this.MassLabel.Text = "Mass";
            this.MassLabel.Visible = false;
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(4, 129);
            this.WidthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(50, 20);
            this.WidthLabel.TabIndex = 4;
            this.WidthLabel.Text = "Width";
            this.WidthLabel.Visible = false;
            // 
            // FPSValue
            // 
            this.FPSValue.AutoSize = true;
            this.FPSValue.Location = new System.Drawing.Point(91, 21);
            this.FPSValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FPSValue.Name = "FPSValue";
            this.FPSValue.Size = new System.Drawing.Size(18, 20);
            this.FPSValue.TabIndex = 5;
            this.FPSValue.Text = "0";
            this.FPSValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.FPSValue.Visible = false;
            // 
            // FoodValue
            // 
            this.FoodValue.AutoSize = true;
            this.FoodValue.Location = new System.Drawing.Point(91, 56);
            this.FoodValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FoodValue.Name = "FoodValue";
            this.FoodValue.Size = new System.Drawing.Size(18, 20);
            this.FoodValue.TabIndex = 6;
            this.FoodValue.Text = "0";
            this.FoodValue.Visible = false;
            // 
            // MassValue
            // 
            this.MassValue.AutoSize = true;
            this.MassValue.Location = new System.Drawing.Point(91, 91);
            this.MassValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MassValue.Name = "MassValue";
            this.MassValue.Size = new System.Drawing.Size(18, 20);
            this.MassValue.TabIndex = 7;
            this.MassValue.Text = "0";
            this.MassValue.Visible = false;
            // 
            // WidthValue
            // 
            this.WidthValue.AutoSize = true;
            this.WidthValue.Location = new System.Drawing.Point(91, 129);
            this.WidthValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WidthValue.Name = "WidthValue";
            this.WidthValue.Size = new System.Drawing.Size(18, 20);
            this.WidthValue.TabIndex = 8;
            this.WidthValue.Text = "0";
            this.WidthValue.Visible = false;
            // 
            // UsernameTextbox
            // 
            this.UsernameTextbox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.UsernameTextbox.Location = new System.Drawing.Point(612, 405);
            this.UsernameTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UsernameTextbox.Name = "UsernameTextbox";
            this.UsernameTextbox.Size = new System.Drawing.Size(148, 26);
            this.UsernameTextbox.TabIndex = 10;
            this.UsernameTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.UsernameTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UsernameTextbox_KeyPress);
            // 
            // ServerLabel
            // 
            this.ServerLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(510, 458);
            this.ServerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(63, 20);
            this.ServerLabel.TabIndex = 11;
            this.ServerLabel.Text = "Server :";
            // 
            // ServerTextbox
            // 
            this.ServerTextbox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ServerTextbox.Location = new System.Drawing.Point(612, 452);
            this.ServerTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ServerTextbox.Name = "ServerTextbox";
            this.ServerTextbox.Size = new System.Drawing.Size(148, 26);
            this.ServerTextbox.TabIndex = 12;
            this.ServerTextbox.Text = "localhost";
            this.ServerTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ServerTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ServerTextbox_KeyPress);
            // 
            // GameOverP
            // 
            this.GameOverP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GameOverP.AutoSize = true;
            this.GameOverP.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.GameOverP.BackgroundImage = global::View.Properties.Resources.tumblr_mbqcnvs08p1ren9jno1_500;
            this.GameOverP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.GameOverP.Location = new System.Drawing.Point(0, 0);
            this.GameOverP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GameOverP.Name = "GameOverP";
            this.GameOverP.Size = new System.Drawing.Size(267, 217);
            this.GameOverP.TabIndex = 13;
            this.GameOverP.Visible = false;
            // 
            // FoodEatenLabel
            // 
            this.FoodEatenLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.FoodEatenLabel.AutoSize = true;
            this.FoodEatenLabel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FoodEatenLabel.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FoodEatenLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FoodEatenLabel.Location = new System.Drawing.Point(18, 380);
            this.FoodEatenLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FoodEatenLabel.Name = "FoodEatenLabel";
            this.FoodEatenLabel.Size = new System.Drawing.Size(156, 26);
            this.FoodEatenLabel.TabIndex = 14;
            this.FoodEatenLabel.Text = "Food Eaten :";
            this.FoodEatenLabel.Visible = false;
            // 
            // FoodEatenValue
            // 
            this.FoodEatenValue.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.FoodEatenValue.AutoSize = true;
            this.FoodEatenValue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.FoodEatenValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FoodEatenValue.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FoodEatenValue.Location = new System.Drawing.Point(180, 380);
            this.FoodEatenValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FoodEatenValue.Name = "FoodEatenValue";
            this.FoodEatenValue.Size = new System.Drawing.Size(25, 28);
            this.FoodEatenValue.TabIndex = 15;
            this.FoodEatenValue.Text = "0";
            this.FoodEatenValue.Visible = false;
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ResetButton.AutoSize = true;
            this.ResetButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ResetButton.Enabled = false;
            this.ResetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ResetButton.Location = new System.Drawing.Point(18, 248);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(230, 122);
            this.ResetButton.TabIndex = 16;
            this.ResetButton.Text = "Play Again";
            this.ResetButton.UseVisualStyleBackColor = false;
            this.ResetButton.Visible = false;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // lastMassLabel
            // 
            this.lastMassLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lastMassLabel.AutoSize = true;
            this.lastMassLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lastMassLabel.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastMassLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lastMassLabel.Location = new System.Drawing.Point(18, 420);
            this.lastMassLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lastMassLabel.Name = "lastMassLabel";
            this.lastMassLabel.Size = new System.Drawing.Size(155, 28);
            this.lastMassLabel.TabIndex = 17;
            this.lastMassLabel.Text = "Last Mass :";
            this.lastMassLabel.Visible = false;
            // 
            // lastMassValue
            // 
            this.lastMassValue.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lastMassValue.AutoSize = true;
            this.lastMassValue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lastMassValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastMassValue.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lastMassValue.Location = new System.Drawing.Point(181, 420);
            this.lastMassValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lastMassValue.Name = "lastMassValue";
            this.lastMassValue.Size = new System.Drawing.Size(24, 26);
            this.lastMassValue.TabIndex = 18;
            this.lastMassValue.Text = "0";
            this.lastMassValue.Visible = false;
            // 
            // agCubio
            // 
            this.agCubio.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.agCubio.AutoSize = true;
            this.agCubio.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.agCubio.BackgroundImage = global::View.Properties.Resources.abstract_cube_11458963;
            this.agCubio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.agCubio.Location = new System.Drawing.Point(383, 34);
            this.agCubio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.agCubio.Name = "agCubio";
            this.agCubio.Size = new System.Drawing.Size(493, 336);
            this.agCubio.TabIndex = 19;
            // 
            // statisticsPanel
            // 
            this.statisticsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statisticsPanel.AutoSize = true;
            this.statisticsPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statisticsPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.statisticsPanel.Controls.Add(this.FPSLabel);
            this.statisticsPanel.Controls.Add(this.FoodLabel);
            this.statisticsPanel.Controls.Add(this.MassLabel);
            this.statisticsPanel.Controls.Add(this.WidthLabel);
            this.statisticsPanel.Controls.Add(this.FPSValue);
            this.statisticsPanel.Controls.Add(this.FoodValue);
            this.statisticsPanel.Controls.Add(this.MassValue);
            this.statisticsPanel.Controls.Add(this.WidthValue);
            this.statisticsPanel.Location = new System.Drawing.Point(1118, 0);
            this.statisticsPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.statisticsPanel.Name = "statisticsPanel";
            this.statisticsPanel.Size = new System.Drawing.Size(182, 811);
            this.statisticsPanel.TabIndex = 20;
            this.statisticsPanel.Visible = false;
            // 
            // form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1251, 1018);
            this.Controls.Add(this.statisticsPanel);
            this.Controls.Add(this.agCubio);
            this.Controls.Add(this.lastMassValue);
            this.Controls.Add(this.lastMassLabel);
            this.Controls.Add(this.FoodEatenValue);
            this.Controls.Add(this.FoodEatenLabel);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.GameOverP);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.ServerTextbox);
            this.Controls.Add(this.UsernameTextbox);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "form";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form_KeyPress);
            this.statisticsPanel.ResumeLayout(false);
            this.statisticsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label FPSLabel;
        private System.Windows.Forms.Label FoodLabel;
        private System.Windows.Forms.Label MassLabel;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.Label FPSValue;
        private System.Windows.Forms.Label FoodValue;
        private System.Windows.Forms.Label MassValue;
        private System.Windows.Forms.Label WidthValue;
        private System.Windows.Forms.TextBox ServerTextbox;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.TextBox UsernameTextbox;
        private Panel GameOverP;
        private Label FoodEatenLabel;
        private Label FoodEatenValue;
        private Button ResetButton;
        private Label lastMassLabel;
        private Label lastMassValue;
        private Panel agCubio;
        private Panel statisticsPanel;
    }
}

