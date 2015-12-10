// Written by Ken Bonar and Rachel Saya for CS 3500, November 2015


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Model;
using Network_Controller;
using Newtonsoft.Json;


namespace View
{
    //==================================================================================================================================================
    //                                                                          GUI Form
    //==================================================================================================================================================
    public partial class form : Form
    {
        //==================================================================================================================================================
        //                                                                   Global Variables
        //==================================================================================================================================================

        /// <summary>
        ///     World object that keeps track of all cubes
        /// </summary>
        public World World;

        /// <summary>
        ///     The Server for the GUI to communicate with
        /// </summary>
        public Socket Server;

        /// <summary>
        ///     Boolean that tracks whether or not things are hidden
        /// </summary>
        public bool HidFirstRun { get; private set; }

        /// <summary>
        ///     Keeps track of this clients player cube UID
        /// </summary>
        private long _playerUid = -1;

        /// <summary>
        ///     Keeps track of this clients plater cube UID
        /// </summary>
        private long _teamUID;

        /// <summary>
        ///     The user name of the client player
        /// </summary>
        private string _username;

        /// <summary>
        ///     The player cube of the client
        /// </summary>
        private Cube _playerCube;

        /// <summary>
        ///     Brush for drawing
        /// </summary>
        private SolidBrush _mybrush;

        /// <summary>
        ///     Scale everything according to play size.
        /// </summary>
        private double _scaleFactor = 2f;

        /// <summary>
        ///     Boolean that keeps track of whether or not player cube is dead.
        /// </summary>
        public bool ItsDeadJim;

        /// <summary>
        ///     Tracks the offset caused by the player mass changing
        /// </summary>
        private int _xOffset, _yOffset;

        /// <summary>
        ///     Generates the FPS
        /// </summary>
        private int FPSCount;

        /// <summary>
        ///     Counts for FPS
        /// </summary>
        private readonly Stopwatch watch = new Stopwatch();


        //==================================================================================================================================================
        //                                                                   Display Code
        //==================================================================================================================================================

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public form()
        {
            InitializeComponent();

            DoubleBuffered = true;

            ActiveControl = UsernameTextbox;

            World = new World();
        }

        /// <summary>
        ///     Paints the scene on the GUI and updates the mouse pointer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (HidFirstRun && World.dict.Count > 0)
            {
                SendPosition();

                // Begin FPS
                ++FPSCount;

                //measure time elapsed
                TimeSpan elapsed = watch.Elapsed;

                Update();

                // display fps since time elapsed
                if (elapsed.Seconds > 0)
                {
                    FPSValue.Text = string.Concat(FPSCount/elapsed.Seconds);
                    FPSValue.Update();
                }
                if (elapsed.Seconds > 50)
                {
                    watch.Restart();
                    FPSCount = 0;
                }

                lock (World)
                {
                    if (ItsDeadJim)
                    {
                        GameOver();
                        HidFirstRun = false;
                        ItsDeadJim = false;
                    }

                    //finds playercube in dictionary
                    try
                    {
                        _playerCube = World.dict[_playerUid];
                    }
                    catch (Exception t)
                    {
                        Console.WriteLine(t.Message);
                    }

                    //set counter values for GUI
                    FoodValue.Text = World.foodCount.ToString();
                    MassValue.Text = _playerCube.Mass.ToString();
                    WidthValue.Text = _playerCube.Size.ToString();
                    FoodValue.Update();
                    MassValue.Update();
                    WidthValue.Update();


                    // Update side display
                    FPSLabel.Update();
                    FoodLabel.Update();
                    MassLabel.Update();
                    WidthLabel.Update();

                    _scaleFactor = 1.2;

                    //transforms the x and y plane that it is reporesented on
                    e.Graphics.TranslateTransform(-_playerCube.loc_x, -_playerCube.loc_y);
                    e.Graphics.ScaleTransform(2f, 2f);


                    // Draw all cubes
                    foreach (KeyValuePair<long, Cube> pair in World.dict)
                    {
                        _xOffset = (int) (pair.Value.loc_x - pair.Value.Size/2);
                        _yOffset = (int) (pair.Value.loc_y - pair.Value.Size/2);

                        // For all player cubes
                        if (!pair.Value.food)
                        {
                            Color color = Color.FromArgb(pair.Value.argb_color);
                            _mybrush = new SolidBrush(color);

                            // Adjust for server offset
                            Rectangle rect = new Rectangle(_xOffset, _yOffset,
                                (int) _playerCube.Size, (int) _playerCube.Size);

                            //draw player
                            e.Graphics.FillRectangle(_mybrush, rect);

                            Brush newbrush = new SolidBrush(Color.White);

                            Font font = new Font("Arial", 16f);
                            SizeF size = e.Graphics.MeasureString(pair.Value.Name, font);

                            // draw name
                            if (size.Width < pair.Value.Size)
                            {
                                e.Graphics.DrawString(pair.Value.Name, font, newbrush,
                                    (int) (_xOffset + pair.Value.Size/2 - size.Width/2),
                                    (int) (_yOffset + pair.Value.Size/2 - size.Height/2));
                            }
                        }

                        // For all food cubes
                        else
                        {
                            Color color = Color.FromArgb(pair.Value.argb_color);
                            _mybrush = new SolidBrush(color);

                            Rectangle rect = new Rectangle(_xOffset, _yOffset,
                                (int) pair.Value.Size, (int) pair.Value.Size);

                            e.Graphics.FillRectangle(_mybrush, rect);
                        }
                    }
                }
            }
            // Redraw based off of mouse event
            if (SendPosition())
            {
                Invalidate();
                Focus();
            }
        }

        /// <summary>
        ///     This Helper class is run to hide the componenets of the GUI no longer in use, such as the
        ///     Server and username information panels. It is utilized by both boxes, so broken out for simplicity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartScreen(object sender, KeyPressEventArgs e)
        {
            string serverIP;

            // show side display 
            FPSValue.Visible = true;
            FPSLabel.Visible = true;
            FoodValue.Visible = true;
            FoodLabel.Visible = true;
            MassValue.Visible = true;
            MassLabel.Visible = true;
            WidthValue.Visible = true;
            WidthLabel.Visible = true;
            statisticsPanel.Visible = true;

            // Turn StartScreen invisible
            UsernameTextbox.Visible = false;
            UsernameTextbox.Enabled = false;
            UsernameLabel.Visible = false;
            ServerLabel.Visible = false;
            ServerTextbox.Visible = false;
            ServerTextbox.Enabled = false;
            agCubio.Visible = false;

            watch.Start();

            // Connect to the server
            serverIP = ServerTextbox.Text;
            HidFirstRun = true;

            Server = NetworkController.ConnectToServer(SendName, serverIP);
        }

        /// <summary>
        ///     Helper fuction that updates various GUI components.
        /// </summary>
        private void GameOver()
        {
            // Game Over screen
            GameOverP.Visible = true;
            GameOverP.Height = Height;
            GameOverP.Width = Width;
            statisticsPanel.Visible = false;

            // Reset Button
            ResetButton.Visible = true;
            ResetButton.Enabled = true;
            ResetButton.BringToFront();
            ResetButton.Update();

            // Food Eaten Display
            FoodEatenValue.Visible = true;
            FoodEatenValue.Text = World.foodEaten.ToString();
            FoodEatenValue.BringToFront();
            FoodEatenValue.Update();
            FoodEatenLabel.Visible = true;
            FoodEatenLabel.BringToFront();
            FoodEatenLabel.Update();

            // Players Eaten Display
            lastMassValue.Visible = true;
            string lastMass = ((int) _playerCube.Mass).ToString();
            lastMassValue.Text = lastMass;
            lastMassValue.BringToFront();
            lastMassValue.Update();
            lastMassLabel.Visible = true;
            lastMassLabel.BringToFront();
            lastMassLabel.Update();

            GameOverP.Update();
        }

        //==================================================================================================================================================
        //                                                                Network Interaction Methods
        //==================================================================================================================================================


        /// <summary>
        ///     This method is an action that will be recieved by a method in the network
        ///     class. An action Encapsulates a method that has a single parameter and
        ///     does not return a value. This action evaluates the
        /// </summary>
        /// <param name="preservedPreservedState"></param>
        private void RecieveCubes(PreservedStateObject preservedPreservedState)
        {
            // Stringbuilder associated with the preservedState object
            StringBuilder stringBuild = preservedPreservedState.StringBuilder;

            // Create a seperator based off the new line charactere
            char[] newLine = new char[1];
            newLine[0] = '\n';

            // Split the stringbuild based on new line characters
            string[] cubes = stringBuild.ToString().Split(newLine, StringSplitOptions.RemoveEmptyEntries);

            int count = 0;

            // For every string cube in cubes create a Cube object and add it to the world
            foreach (string cube in cubes)
            {
                // checks for cube being complete
                if (cube.StartsWith("{") && cube.EndsWith("}"))
                {
                    //deserializes the string object to a cube
                    Cube tempCube = JsonConvert.DeserializeObject<Cube>(cube);
                    if (tempCube != null)
                    {
                        lock (World)
                        {
                            if (_playerUid == -1)
                            {
                                _playerUid = tempCube.uid;
                                _teamUID = tempCube._teamid;
                            }


                            // Cube is dead
                            if (tempCube.Mass == 0.0)
                            {
                                if (tempCube.uid == _playerCube.uid)
                                {
                                    ItsDeadJim = true;
                                }

                                World.Remove(tempCube);
                            }
                            else
                                World.Add(tempCube);
                            count++;
                        }
                    }
                    else
                        break;
                }
                else
                    break;
            }

            // Handle partial packets of information.
            stringBuild.Clear();
            if (count > 0)
                Invalidate();

            if (count != cubes.Length)
                stringBuild.Append(cubes[cubes.Length - 1]);

            preservedPreservedState.StringBuilder = stringBuild;

            // continues requestion data from socket
            NetworkController.i_want_more_data(preservedPreservedState);
        }

        /// <summary>
        ///     Sends the position of the client cube based off of its mouse position
        /// </summary>
        private bool SendPosition()
        {
            // If still in the start menu and wait to recieve cubes
            if (!HidFirstRun || World.dict.Count < 1)
            {
                return false;
            }
            try
            {
                // Sends mouse location
                Point mouse = PointToClient(new Point(MousePosition.X, MousePosition.Y));
                NetworkController.Send(Server, "(move, " + mouse.X + ", " + mouse.Y + ")\n");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     SendName is an action that is sending the name as the argument to the preservedState
        /// </summary>
        /// <param name="preservedState"></param>
        private void SendName(PreservedStateObject preservedState)
        {
            // Update username     
            preservedState.name = UsernameTextbox.Text;
            preservedState.Call = RecieveCubes;
            _username = preservedState.name;
            if (preservedState.ErrorHappened)
            {
                NewGame();
                MessageBox.Show("Error: Server could not be reached. Please press OK and try again.");
            }

            // Send name to server and check for error


            NetworkController.Send(Server, preservedState.name + "\n");
        }

        //==================================================================================================================================================
        //                                                                 Event Code
        //==================================================================================================================================================

        /// <summary>
        ///     Allows the user to choose their username.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsernameTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (char) 13) && (UsernameTextbox.Text != "") && ServerTextbox.Text != "")
                StartScreen(sender, e);
        }

        /// <summary>
        ///     Allows the user to choose their Server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (char) 13) && (UsernameTextbox.Text != "") && ServerTextbox.Text != "")
                StartScreen(sender, e);
        }

        /// <summary>
        ///     Handles space key presssed and split command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if key pressed is space
            if (e.KeyChar != ' ')
                return;

            // Send split command with mouse location
            Point mouse = PointToClient(new Point(MousePosition.X, MousePosition.Y));
            NetworkController.Send(Server, "(split, " + mouse.X + ", " + mouse.Y + ")\n");
        }

        /// <summary>
        ///     Button to Reset the game window on gameover event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        /// <summary>
        ///     Begins a new game
        /// </summary>
        public void NewGame()
        {
            BeginInvoke(new Action(() =>
            {
                Hide();
                form newform = new form();
                newform.Closed += (s, args) => Close();
                newform.Show();
            }));
        }
    }
}
