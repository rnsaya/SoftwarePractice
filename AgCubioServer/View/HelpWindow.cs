using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace View
{
    public partial class HelpWindow : Form
    {
        private const string HelpFileName = "..//..//..//Resources//README.txt";

        public HelpWindow()
        {
            InitializeComponent();
            try
            {
                this.helpText.Text = File.ReadAllText(HelpWindow.HelpFileName);
            }
            catch { }
        }
    }
}
