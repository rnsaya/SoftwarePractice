using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Model;

namespace View
{
    public partial class DeathPanel : Panel
    {
        /// <summary>
        /// MainWindow handle
        /// </summary>
        private MainWindow view;

        public DeathPanel(MainWindow view)
        {
            this.view = view;
            InitializeComponent();
        }

        /// <summary>
        /// Sets the text of LabelStatAteMsg
        /// </summary>
        public void AteMsg(string PlayerName, Cube Enemy)
        {
            Random RG = new Random();

            string[] msg = new string[] { " gobbled you up!", " ate your bones!", " made you for dinner!", " made " + PlayerName + " into pie!" };

            this.LabelStatAteMsg.Text = Enemy.name + msg[RG.Next(0, msg.Length)];
            this.LabelStatAteMsg.ForeColor = Color.FromArgb(Enemy.argb_color);
            this.LabelStatAteMsg.Location = new System.Drawing.Point((view.ClientSize.Width - this.LabelStatAteMsg.Size.Width) / 2, 166);
        }

        /// <summary>
        /// Sets the text of LabelNetStatSentVal
        /// </summary>
        public int SentPackets
        {
            set
            {
                this.LabelNetStatSentVal.Text = value.ToString();
            }
        }

        /// <summary>
        /// Sets the text of LabelNetStatReceivedVal
        /// </summary>
        public int RecvPackets
        {
            set
            {
                this.LabelNetStatReceivedVal.Text = value.ToString();
            }
        }

        /// <summary>
        /// Sets the text of LabelStatTimeVal
        /// </summary>
        public string TimePlayed
        {
            set
            {
                this.LabelStatTimeVal.Text = value;
            }
        }

        /// <summary>
        /// Sets the text of LabelStatMassVal
        /// </summary>
        public double Mass
        {
            set
            {
                this.LabelStatMassVal.Text = value.ToString();
            }
        }

        /// <summary>
        /// BtnPlayAgain_Click callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPlayAgain_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.SendToBack();

            view.Reset();
            view.Reconnect();
        }

        /// <summary>
        /// BtnNewServer_Click callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewServer_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.SendToBack();

            view.Reset();
            view.ShowLogin(view.LoginToServer);
        }
    }
}
