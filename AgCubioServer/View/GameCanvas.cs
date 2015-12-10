using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    /// <summary>
    /// This class is a Panel with double buffering.
    /// </summary>
    public class GameCanvas : Panel
    {
        /// <summary>
        /// This initializes the Panel with DoubleBuffering.
        /// </summary>
        public GameCanvas()
        {
            this.DoubleBuffered = true;
        }
    }
}
