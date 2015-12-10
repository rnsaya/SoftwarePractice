using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using System.Threading;
using System.Diagnostics;
using NetworkController;

namespace View
{
    /// <summary>
    /// This delegate is for handling login.
    /// </summary>
    /// <param name="name">The name of the cell.</param>
    /// <param name="serverIP">The address of the server.</param>
    public delegate void LoginEventHandler(string name, string serverIP);

    /// <summary>
    /// This class represents the main game window for AgCubio.
    /// </summary>
    public partial class MainWindow : Form
    {
        /// <summary>
        /// This is the current login box.
        /// </summary>
        private Form loginBox = null;

        /// <summary>
        /// These variables are used to compute the FPS.
        /// </summary>
        private long framesCount = 0;
        private readonly Stopwatch clock = new Stopwatch();

        /// <summary>
        /// This is the primary tracked cube.
        /// </summary>
        private Cube mainCube;

        /// <summary>
        /// The goal is to redraw the screen at 60 FPS.
        /// </summary>
        private const double FPSGoal = 1 / 60.0;

        /// <summary>
        /// This periodically asks for more data.
        /// </summary>
        private PeriodicTaskExecutor moreDataExecutor;

        /// <summary>
        /// This is the name of the player.
        /// </summary>
        private string name;

        /// <summary>
        /// This is the point that keeps track of the current mouse location.
        /// </summary>
        private Point mouseLocation = new Point(0, 0);

        /// <summary>
        /// These functions both map from points from the model to the viewport and back
        /// again. ZoomFunc will set the size to be 10x the size of the main cube.
        /// </summary>
        private Func<Point, Point> ZoomFunc, ZoomFuncInverse;

        /// <summary>
        /// This * cube size is the size of the viewport.
        /// </summary>
        private const double RelativeSize = 10.0;

        /// <summary>
        /// This size is the minimum cube size to ensure that food is always visible.
        /// </summary>
        private const int MinimumCubeSize = 3;

        /// <summary>
        /// This is supposed to surpress help unless help is actually clicked.
        /// </summary>
        private bool surpressHelp = false;

        /// <summary>
        /// Stopwatch to calculate elapsed game time
        /// </summary>
        private Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// The name of the current server.
        /// </summary>
        private string serverName = "";

        /// <summary>
        /// This method computes new zoom functions based off of the current state of the World.
        /// </summary>
        private void ComputeZoomFunctions()
        {
            // Empty space is the length surrounding the main cube.
            Cube conglom = new Cube(0);
            conglom.Mass = this.mainCube.TrueMass;
            conglom.loc_x = this.mainCube.loc_x;
            conglom.loc_y = this.mainCube.loc_y;
            double newSize = RelativeSize * conglom.Width;
            double maxWidth = World.WorldSingleton.MaxSizeX;
            if (newSize > maxWidth)
            {
                float scaleFactor = (float)(this.gameCanvas.Size.Width / maxWidth);
                Point upperLeft = new Point(0, 0);

                this.ZoomFunc = CubeExtensions.GetZoomFunction(upperLeft, scaleFactor, out this.ZoomFuncInverse);
            }
            else
            {
                double emptySpace = (RelativeSize - 1) * conglom.Width / 2;
                Rectangle rect = conglom.GetRect();
                Point upperLeft = new Point((int)(rect.X - emptySpace), (int)(rect.Y - emptySpace));
                float scaleFactor = (float)(this.gameCanvas.Size.Width / newSize);

                // Calculate the new zoom functions.
                this.ZoomFunc = CubeExtensions.GetZoomFunction(upperLeft, scaleFactor, out this.ZoomFuncInverse);
            }
        }

        /// <summary>
        /// Initializes the window.
        /// </summary>
        public MainWindow()
        {
            this.FormClosed += (arg1, arg2) => this.CloseApplication();
            InitializeComponent();
            this.ShowLogin(this.LoginToServer);
            World.WorldSingleton.WorldChangedEvent += this.UpdateCubesDrawn;
            clock.Restart();

            // Update the frames count every time the game canvas is redrawn.
            this.gameCanvas.Paint += (arg1, arg2) => framesCount++;
            this.gameCanvas.Paint += this.OnPainEvent;
        }

        /// <summary>
        /// This is the callback for when the user logs in.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="serverIP">The server address.</param>
        public void LoginToServer(string name, string serverIP)
        {
            if (name == null || serverIP == null)
            {
                this.CloseApplication();
                return;
            }
            try
            {
                this.loginBox.Close();
            }
            catch { }

            this.name = name;
            this.serverName = serverIP;

            Sockets.Connect(serverIP, 11000, this.ConnectedToServerCB);
        }
        
        /// <summary>
        /// This is called once connected to the server.
        /// </summary>
        /// <param name="success">True if connected successfully.</param>
        private void ConnectedToServerCB(bool success)
        {
            this.stopwatch.Restart();
            this.clock.Restart();

            Sockets.Send(name, this.AfterSentNameCB);
        }

        /// <summary>
        /// This is called after name is sent and will ask for more
        /// data.
        /// </summary>
        /// <param name="success">True if sent successfully.</param>
        private void AfterSentNameCB(bool success)
        {
            Action S404 = () => 
            {
                ShowErrorMessage(this.serverName + " does not exist.");
                Reset();
                ShowLogin(this.LoginToServer);
            };

            if (!success)
                this.Invoke(S404);
            else            
                Sockets.Moar(this.GetPlayerCube);
        }

        /// <summary>
        /// This takes the first cube, sets it as the main message,
        /// and then sends all the cubes to the World.
        /// </summary>
        /// <param name="messages">A list of Json cubes to process.</param>
        private void GetPlayerCube(string messages)
        {
            List<Cube> cubes = World.GetCubesFromJson(messages);
            if (cubes.Count == 0)
            {
                Sockets.Moar(GetPlayerCube);
                return;
            }
            this.mainCube = cubes[0];
            World.WorldSingleton.UpdateCubes(cubes);

            Action askForMoar = () => { Sockets.Moar(HandleMoar); };

            this.moreDataExecutor = new PeriodicTaskExecutor(askForMoar, MainWindow.FPSGoal);
        }

        /// <summary>
        /// Shows the input error message to the user.
        /// </summary>
        /// <param name="message">The message to show.</param>
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Handles edge case behavior of Moar return string
        /// </summary>
        /// <param name="message">Incoming message from Moar()</param>
        private void HandleMoar(string message)
        {
            Action ServerDisconnected = () => 
            {
                this.ShowErrorMessage(this.serverName + " disconnected.");
                Reset();
                ShowLogin(this.LoginToServer);
            };

            if (message == null)
            {
                this.moreDataExecutor.Stopped = true;
                this.Invoke(ServerDisconnected);
            }

            World.WorldSingleton.ProcessCubeJson(message);
        }

        /// <summary>
        /// Resets the GUI after a bad thing happens
        /// </summary>
        public void Reset()
        {
            this.DeathScreen.Visible = false;
            if (this.moreDataExecutor != null)
                this.moreDataExecutor.Stopped = true;
            World.WorldSingleton.ClearWorld();
            stopwatch.Restart();
        }

        /// <summary>
        /// Reconnects to the same server when eaten
        /// </summary>
        public void Reconnect()
        {
            this.Reset();
            this.LoginToServer(this.name, this.serverName);
        }

        /// <summary>
        /// This closes the application cleanly.
        /// </summary>
        private void CloseApplication()
        {
            try
            {
                this.moreDataExecutor.Stopped = true;
            }
            catch { }
            try
            {
                Sockets.Disconnect();
            }
            catch { }
            Application.Exit();
        }

        /// <summary>
        /// This method will ask the user to login.
        /// </summary>
        /// <param name="handler">The code to execute once logged in.</param>
        public void ShowLogin(LoginEventHandler handler)
        {
            this.loginBox = new Login(handler);
            this.loginBox.Show(this);
        }

        /// <summary>
        /// This method is an event handler for when the world changes and cubes need to be redrawn.
        /// </summary>
        /// <param name="origin">The originating world object that has updated.</param>
        /// <param name="changed">The cubes that have changed.</param>
        /// <param name="deleted">The cubes that have been deleted.</param>
        public void UpdateCubesDrawn(World origin, IEnumerable<Cube> changed, IEnumerable<Cube> deleted)
        {
            Action act = () =>
            {
                this.ComputeZoomFunctions();
                this.SetCubeStats();
                this.gameCanvas.Invalidate();
                this.TellServerToMove();
                if (deleted.Contains<Cube>(this.mainCube))
                {
                    stopwatch.Stop();
                    this.moreDataExecutor.Stopped = true;

                    Sockets.Disconnect();
                    this.DisplayStats();
                }
            };

            try
            {
                this.gameCanvas.BeginInvoke(act);
            }
            catch { }
        }

        /// <summary>
        /// This method redraws the game canvas.
        /// </summary>
        /// <param name="gameCanvas">The canvas to redraw.</param>
        /// <param name="args">The paint arguments.</param>
        private void OnPainEvent(object gameCanvas, PaintEventArgs args)
        {
            var graphics = args.Graphics;
            graphics.Clear(this.BackColor);
            foreach (Cube cube in World.WorldSingleton)
            {
                this.DrawCube(cube, graphics);
            }
        }
        
        /// <summary>
        /// This method draws the input cube onto the canvas.
        /// </summary>
        /// <param name="cube">The cube to draw.</param>
        /// <param name="graphics">The graphics to use to draw with.</param>
        private void DrawCube(Cube cube, Graphics graphics)
        {
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(cube.argb_color)))
            {
                Rectangle rect = cube.GetRect().MapPointFunc(this.ZoomFunc);
                if (rect.Width < MainWindow.MinimumCubeSize)
                    rect.Size = new Size(MinimumCubeSize, MinimumCubeSize);
                if (this.gameCanvas.DisplayRectangle.Contains(rect.Location))
                    graphics.FillRectangle(brush, rect);
                if (!cube.food && cube.name != null && !cube.name.Equals(""))
                {
                    Color cubeColor = Color.FromArgb(cube.argb_color);
                    using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(255 - cubeColor.R, 255 - cubeColor.G, 255 - cubeColor.B)))
                    {
                        Font font = new Font("Arial", 10.0F);
                        graphics.DrawString(cube.name, font, textBrush, rect.Location);
                    }
                }
            }
        }

        /// <summary>
        /// This method sets various statistics used by the GUI including
        /// cube mass and width.
        /// </summary>
        private void SetCubeStats()
        {
            try
            {
                this.massLabel.Text = "Total Mass:" + this.mainCube.Mass;
                this.widthLabel.Text = "Width:" + Math.Round(this.mainCube.Width, 1);
                this.fpsLabel.Text = "FPS:" + Math.Round(this.FPS, 1);
                this.foodLabel.Text = "Total Food:" + World.WorldSingleton.GetTotalFood();
            }
            catch { }
        }

        /// <summary>
        /// This is the number of frames per second that the game canvas is redrawn.
        /// </summary>
        public double FPS
        {
            get
            {
                return 1000.0 * this.framesCount / this.clock.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// This launches a help window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButtonClick(object sender, EventArgs e)
        {
            if (!this.surpressHelp)
            {
                HelpWindow window = new HelpWindow();
                window.Show(this);
            }
            else
                this.surpressHelp = false;
        }

        /// <summary>
        /// This callback is called whenever the mouse moves on the game canvas
        /// and sends the mouse position to the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameCanvasMouseMove(object sender, MouseEventArgs e)
        {
            this.mouseLocation = e.Location;
        }

        /// <summary>
        /// This sends the server to move the player.
        /// </summary>
        private void TellServerToMove()
        {
            if (this.IsLoggedIn)
            {
                Point loc = this.ZoomFuncInverse(this.mouseLocation);
                string message = "(move, " + loc.X + ", " + loc.Y + ")";
                Sockets.Send(message, (arg) => { });
            }
        }

        private void MainWindowKeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Point loc = this.ZoomFuncInverse(this.mouseLocation);
                string message = "(split, " + loc.X + ", " + loc.Y + ")";
                Sockets.Send(message, (arg) => { });
            }
            catch { }
            this.surpressHelp = true;
        }

        /// <summary>
        /// This returns true if logged in.
        /// </summary>
        public bool IsLoggedIn
        {
            get { return this.moreDataExecutor != null && !this.moreDataExecutor.Stopped; }
        }

        /// <summary>
        /// Displays the statistics screen
        /// </summary>
        private void DisplayStats()
        {
            this.DeathScreen.AteMsg(mainCube.name, World.WorldSingleton.GetClosestCube(this.mainCube));

            this.DeathScreen.Mass = this.mainCube.Mass;
            this.DeathScreen.TimePlayed = stopwatch.Elapsed.ToString(@"m\:ss");

            this.DeathScreen.SentPackets = Sockets.SentPackets;
            this.DeathScreen.RecvPackets = Sockets.RecvPackets;

            this.DeathScreen.Visible = true;
            this.DeathScreen.BringToFront();
        }
    }

    /// <summary>
    /// This class contains Cube extensions relevant to actually drawing Cubes.
    /// </summary>
    public static class CubeExtensions
    {
        /// <summary>
        /// This function computes a function that zooms by setting the new upper left to (0,0) and
        /// scaling by the input scale factor.
        /// </summary>
        /// <param name="upperLeft">The point which will be (0,0)</param>
        /// <param name="scaleFactor">The amount to multiply each point by.</param>
        /// <param name="inverse">A function which will map back to the original domain.</param>
        /// <returns></returns>
        public static Func<Point, Point> GetZoomFunction(Point upperLeft, float scaleFactor, out Func<Point, Point> inverse)
        {
            inverse = (pt) => new Point((int)(pt.X / scaleFactor + upperLeft.X),
                (int)(pt.Y / scaleFactor + upperLeft.Y));
            return (pt) => new Point((int)(scaleFactor * (pt.X - upperLeft.X)), (int)(scaleFactor * (pt.Y - upperLeft.Y)));
        }

        /// <summary>
        /// This method applies the input function over every point in the rectangle returning
        /// a rectangle with every point mapped to the new domain.
        /// </summary>
        /// <param name="rect">The original rectangle.</param>
        /// <param name="mapFunc">The function to apply over every point.</param>
        /// <returns>A new rectangle in the codomain of the mapFunc.</returns>
        public static Rectangle MapPointFunc(this Rectangle rect, Func<Point, Point> mapFunc)
        {
            Point sizePoint = new Point(rect.Width + rect.X, rect.Height + rect.Y);
            sizePoint = mapFunc(sizePoint);
            Point newCorner = mapFunc(rect.Location);
            Size newSize = new Size(sizePoint.X - newCorner.X, sizePoint.Y - newCorner.Y);
            return new Rectangle(newCorner, newSize);
        }

        /// <summary>
        /// This extension returns the total amount of food in the World.
        /// </summary>
        /// <param name="world">The world to calculate the total food of.</param>
        /// <returns>The total amount of food in the world.</returns>
        public static int GetTotalFood(this World world)
        {
            return world.Count<Cube>((cube) => cube.food);
        }
    }
}
