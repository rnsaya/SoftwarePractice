// Written by Ken Bonar and Rachel Saya for CS 3500, November 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using Newtonsoft.Json;


namespace View
{

    //==================================================================================================================================================
    //                                                                          ClientGUI Class
    //==================================================================================================================================================
    public class ClientGui
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new form());
        }
    }
}
