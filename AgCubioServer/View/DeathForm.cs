using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public partial class DeathForm : Form
    {
        public DeathForm()
        {
            InitializeComponent();
        }

        public void AteMsg(string PlayerName, string Enemy)
        {
                Random RG = new Random();

                string[] msg = new string[] { "gobbled you up!", "ate your bones!", "made you for dinner!", "made " + PlayerName + "pie!" };

                this.LabelStatAteMsg.Text = Enemy + msg[RG.Next(0,msg.Length)];
        }
    }
}
