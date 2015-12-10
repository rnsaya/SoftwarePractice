// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Ross DiMassino.
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
    /// <summary>
    /// This class logs a user into the system.
    /// </summary>
    public partial class Login : Form
    {
        /// <summary>
        /// The callback for once the user inputs their info.
        /// </summary>
        private readonly LoginEventHandler loginHandler;

        /// <summary>
        /// A simple check for if the event handler has been invoked yet.
        /// </summary>
        private bool eventTriggered = false;

        /// <summary>
        /// Initializes a blank login screen.
        /// </summary>
        /// <param name="handler">The handler once the user inputs their login info.</param>
        public Login(LoginEventHandler handler)
        {
            this.loginHandler = handler;
            InitializeComponent();
            this.ResetLogin();
        }

        /// <summary>
        /// This method resets the text fields.
        /// </summary>
        public void ResetLogin()
        {
            this.serverInput.Text = "localhost";
            this.nameInput.Text = "";
        }

        /// <summary>
        /// This method is called when the login button is clicked.
        /// </summary>
        /// <param name="sender">The originating object.</param>
        /// <param name="e">Extra args</param>
        private void LoginButtonClick(object sender, EventArgs e)
        {
            string name = this.nameInput.Text;
            string server = this.serverInput.Text;
            this.eventTriggered = true;
            this.loginHandler(name, server);
        }

        /// <summary>
        /// This is called when the form closes and checks that
        /// the event has been triggered, sending nulls to the handler
        /// if it has not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginFormClosed(object sender, FormClosedEventArgs e)
        {
            if (!this.eventTriggered)
            {
                this.eventTriggered = true;
                this.loginHandler(null, null);
            }
        }

        /// <summary>
        /// This method is invoked when the clear button is clicked and sets
        /// the name to "" and the server to "localhost".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButtonClick(object sender, EventArgs e)
        {
            this.ResetLogin();
        }
    }
}
