namespace View
{
    partial class DeathPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnPlayAgain = new System.Windows.Forms.Button();
            this.LabelYouDied = new System.Windows.Forms.Label();
            this.LabelNoob = new System.Windows.Forms.Label();
            this.LabelStatMass = new System.Windows.Forms.Label();
            this.BtnNewServer = new System.Windows.Forms.Button();
            this.LabelStatTime = new System.Windows.Forms.Label();
            this.LabelNetStats = new System.Windows.Forms.Label();
            this.LabelNetStatSent = new System.Windows.Forms.Label();
            this.LabelNetStatReceived = new System.Windows.Forms.Label();
            this.LabelStatAteMsg = new System.Windows.Forms.Label();
            this.LabelStatTimeVal = new System.Windows.Forms.Label();
            this.LabelStatMassVal = new System.Windows.Forms.Label();
            this.LabelNetStatSentVal = new System.Windows.Forms.Label();
            this.LabelNetStatReceivedVal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnPlayAgain
            // 
            this.BtnPlayAgain.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.BtnPlayAgain.Location = new System.Drawing.Point(476, 206);
            this.BtnPlayAgain.Name = "BtnPlayAgain";
            this.BtnPlayAgain.Size = new System.Drawing.Size(127, 49);
            this.BtnPlayAgain.TabIndex = 0;
            this.BtnPlayAgain.Text = "Play Again";
            this.BtnPlayAgain.UseVisualStyleBackColor = true;
            this.BtnPlayAgain.Click += new System.EventHandler(this.BtnPlayAgain_Click);
            // 
            // LabelYouDied
            // 
            this.LabelYouDied.AutoSize = true;
            this.LabelYouDied.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelYouDied.ForeColor = System.Drawing.Color.Red;
            this.LabelYouDied.Location = new System.Drawing.Point((view.ClientSize.Width - this.LabelYouDied.Size.Width) / 2, 22);
            this.LabelYouDied.Name = "LabelYouDied";
            this.LabelYouDied.Size = new System.Drawing.Size(464, 108);
            this.LabelYouDied.TabIndex = 1;
            this.LabelYouDied.Text = "You Died!";
            // 
            // LabelNoob
            // 
            this.LabelNoob.AutoSize = true;
            this.LabelNoob.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.LabelNoob.ForeColor = System.Drawing.Color.Maroon;
            this.LabelNoob.Location = new System.Drawing.Point(260, 118);
            this.LabelNoob.Name = "LabelNoob";
            this.LabelNoob.Size = new System.Drawing.Size(115, 39);
            this.LabelNoob.TabIndex = 2;
            this.LabelNoob.Text = "(noob)";
            // 
            // LabelStatMass
            // 
            this.LabelStatMass.AutoSize = true;
            this.LabelStatMass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.LabelStatMass.Location = new System.Drawing.Point(114, 226);
            this.LabelStatMass.Name = "LabelStatMass";
            this.LabelStatMass.Size = new System.Drawing.Size(110, 20);
            this.LabelStatMass.TabIndex = 3;
            this.LabelStatMass.Text = "Highest Mass:";
            // 
            // BtnNewServer
            // 
            this.BtnNewServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.BtnNewServer.Location = new System.Drawing.Point(476, 278);
            this.BtnNewServer.Name = "BtnNewServer";
            this.BtnNewServer.Size = new System.Drawing.Size(127, 49);
            this.BtnNewServer.TabIndex = 4;
            this.BtnNewServer.Text = "New Server";
            this.BtnNewServer.UseVisualStyleBackColor = true;
            this.BtnNewServer.Click += new System.EventHandler(this.BtnNewServer_Click);
            // 
            // LabelStatTime
            // 
            this.LabelStatTime.AutoSize = true;
            this.LabelStatTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.LabelStatTime.Location = new System.Drawing.Point(126, 206);
            this.LabelStatTime.Name = "LabelStatTime";
            this.LabelStatTime.Size = new System.Drawing.Size(98, 20);
            this.LabelStatTime.TabIndex = 5;
            this.LabelStatTime.Text = "Time Played:";
            // 
            // LabelNetStats
            // 
            this.LabelNetStats.AutoSize = true;
            this.LabelNetStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.LabelNetStats.Location = new System.Drawing.Point(48, 386);
            this.LabelNetStats.Name = "LabelNetStats";
            this.LabelNetStats.Size = new System.Drawing.Size(206, 20);
            this.LabelNetStats.TabIndex = 6;
            this.LabelNetStats.Text = "Network Statistics (packets)";
            // 
            // LabelNetStatSent
            // 
            this.LabelNetStatSent.AutoSize = true;
            this.LabelNetStatSent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LabelNetStatSent.Location = new System.Drawing.Point(101, 406);
            this.LabelNetStatSent.Name = "LabelNetStatSent";
            this.LabelNetStatSent.Size = new System.Drawing.Size(41, 17);
            this.LabelNetStatSent.TabIndex = 7;
            this.LabelNetStatSent.Text = "Sent:";
            // 
            // LabelNetStatReceived
            // 
            this.LabelNetStatReceived.AutoSize = true;
            this.LabelNetStatReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LabelNetStatReceived.Location = new System.Drawing.Point(71, 423);
            this.LabelNetStatReceived.Name = "LabelNetStatReceived";
            this.LabelNetStatReceived.Size = new System.Drawing.Size(71, 17);
            this.LabelNetStatReceived.TabIndex = 8;
            this.LabelNetStatReceived.Text = "Received:";
            // 
            // LabelStatAteMsg
            // 
            this.LabelStatAteMsg.AutoSize = true;
            this.LabelStatAteMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.LabelStatAteMsg.Location = new System.Drawing.Point(250, 166);
            this.LabelStatAteMsg.Name = "LabelStatAteMsg";
            this.LabelStatAteMsg.Size = new System.Drawing.Size(80, 25);
            this.LabelStatAteMsg.TabIndex = 9;
            this.LabelStatAteMsg.Text = "AteMsg";
            // 
            // LabelStatTimeVal
            // 
            this.LabelStatTimeVal.AutoSize = true;
            this.LabelStatTimeVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LabelStatTimeVal.Location = new System.Drawing.Point(230, 206);
            this.LabelStatTimeVal.Name = "LabelStatTimeVal";
            this.LabelStatTimeVal.Size = new System.Drawing.Size(44, 17);
            this.LabelStatTimeVal.TabIndex = 11;
            this.LabelStatTimeVal.Text = "10:00";
            // 
            // LabelStatMassVal
            // 
            this.LabelStatMassVal.AutoSize = true;
            this.LabelStatMassVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LabelStatMassVal.Location = new System.Drawing.Point(230, 226);
            this.LabelStatMassVal.Name = "LabelStatMassVal";
            this.LabelStatMassVal.Size = new System.Drawing.Size(40, 17);
            this.LabelStatMassVal.TabIndex = 12;
            this.LabelStatMassVal.Text = "9001";
            // 
            // LabelNetStatSentVal
            // 
            this.LabelNetStatSentVal.AutoSize = true;
            this.LabelNetStatSentVal.Location = new System.Drawing.Point(148, 410);
            this.LabelNetStatSentVal.Name = "LabelNetStatSentVal";
            this.LabelNetStatSentVal.Size = new System.Drawing.Size(31, 13);
            this.LabelNetStatSentVal.TabIndex = 14;
            this.LabelNetStatSentVal.Text = "1337";
            // 
            // LabelNetStatReceivedVal
            // 
            this.LabelNetStatReceivedVal.AutoSize = true;
            this.LabelNetStatReceivedVal.Location = new System.Drawing.Point(149, 427);
            this.LabelNetStatReceivedVal.Name = "LabelNetStatReceivedVal";
            this.LabelNetStatReceivedVal.Size = new System.Drawing.Size(31, 13);
            this.LabelNetStatReceivedVal.TabIndex = 15;
            this.LabelNetStatReceivedVal.Text = "7331";
            // 
            // DeathPanel
            // 
            this.Controls.Add(this.LabelNetStatReceivedVal);
            this.Controls.Add(this.LabelNetStatSentVal);
            this.Controls.Add(this.LabelStatMassVal);
            this.Controls.Add(this.LabelStatTimeVal);
            this.Controls.Add(this.LabelStatAteMsg);
            this.Controls.Add(this.LabelNetStatReceived);
            this.Controls.Add(this.LabelNetStatSent);
            this.Controls.Add(this.LabelNetStats);
            this.Controls.Add(this.LabelStatTime);
            this.Controls.Add(this.BtnNewServer);
            this.Controls.Add(this.LabelStatMass);
            this.Controls.Add(this.LabelNoob);
            this.Controls.Add(this.LabelYouDied);
            this.Controls.Add(this.BtnPlayAgain);
            this.Name = "DeathForm";
            this.Size = new System.Drawing.Size(674, 503);
            this.Text = "DeathForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnPlayAgain;
        private System.Windows.Forms.Label LabelYouDied;
        private System.Windows.Forms.Label LabelNoob;
        private System.Windows.Forms.Label LabelStatMass;
        private System.Windows.Forms.Button BtnNewServer;
        private System.Windows.Forms.Label LabelStatTime;
        private System.Windows.Forms.Label LabelNetStats;
        private System.Windows.Forms.Label LabelNetStatSent;
        private System.Windows.Forms.Label LabelNetStatReceived;
        private System.Windows.Forms.Label LabelStatAteMsg;
        private System.Windows.Forms.Label LabelStatTimeVal;
        private System.Windows.Forms.Label LabelStatMassVal;
        private System.Windows.Forms.Label LabelNetStatSentVal;
        private System.Windows.Forms.Label LabelNetStatReceivedVal;
    }
}
