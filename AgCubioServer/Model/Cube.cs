// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Ross DiMassino.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Model
{
    /// <summary>
    /// This class represents a cube object in the mode.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Cube
    {
        /// <summary>
        /// Cube Mass
        /// </summary>
        [JsonProperty]
        public double Mass;

        /// <summary>
        /// Cube location x
        /// </summary>
        [JsonProperty]
        public double loc_x;

        /// <summary>
        /// Cube location y
        /// </summary>
        [JsonProperty]
        public double loc_y;

        /// <summary>
        /// This property is the common team id for a given cell.
        /// </summary>
        [JsonProperty]
        public readonly int team_id;
        
        /// <summary>
        /// This is the value for DesiredLocation, see DesiredLocation docs.
        /// </summary>
        private Point desiredLocation;

        /// <summary>
        /// In the case of a split, this stores away the unit vector to move towards.
        /// </summary>
        private PointF splitUnitVector;

        /// <summary>
        /// This is the desired location to move to. If null, there is no
        /// location to move to (so don't move).
        /// </summary>
        [JsonIgnore]
        public Point DesiredLocation
        {
            get
            {
                return this.desiredLocation;
            }

            set
            {
                this.MoveRequested = true;
                if (!this.MomentumActive)
                    this.desiredLocation = value;
            }
        }

        /// <summary>
        /// This is the computed width of the cube (the square root of the mass).
        /// </summary>
        public double Width
        {
            get { return Math.Sqrt(this.Mass); }
        }

        /// <summary>
        /// This represents the globally unique identifier for this particular player/cube.
        /// </summary>
        [JsonProperty]
        public readonly int uid;

        /// <summary>
        /// Represents the name of this Cube.
        /// </summary>
        [JsonProperty]
        public string name;

        /// <summary>
        /// Represents whether this Cube is a piece of food.
        /// </summary>
        [JsonProperty]
        public bool food;

        /// <summary>
        /// This is the momentum of the cube after a split.
        /// </summary>
        [JsonIgnore]
        public double Momentum { get; private set; } = 0;

        /// <summary>
        /// Represents the displayed color of this cube.
        /// </summary>
        [JsonProperty]
        public int argb_color;

        /// <summary>
        /// This property indicates whether a Cube is a virus.
        /// </summary>
        [JsonIgnore]
        public bool IsVirus { get; set; }

        /// <summary>
        /// This is the current speed of the cube according to its mass.
        /// </summary>
        [JsonIgnore]
        public double Speed
        {
            get
            {
                // Max speed at this formula is for cubes with mass of food.
                if (MomentumActive)
                    return this.Momentum * Math.Max(GlobalConstants.MinSpeed * Math.Sqrt(this.Mass / GlobalConstants.FoodMass), GlobalConstants.MaxSpeed);
                else
                    return Math.Min(GlobalConstants.MaxSpeed / Math.Sqrt(this.Mass / GlobalConstants.FoodMass), GlobalConstants.MinSpeed);
            }
        }

        /// <summary>
        /// This indicates whether a move has been requested from the client.
        /// </summary>
        [JsonIgnore]
        public bool MoveRequested { get; private set; }

        /// <summary>
        /// Initializes a blank cube with the input uid.
        /// </summary>
        /// <param name="uid">The uid of the cube.</param>
        public Cube(int uid)
        {
            this.uid = uid;
            this.team_id = uid;
        }

        /// <summary>
        /// Constructor for Json deserialization.
        /// </summary>
        /// <param name="uid">The unique id.</param>
        /// <param name="team_id">The team id.</param>
        [JsonConstructor]
        public Cube(int uid, int team_id)
        {
            this.uid = uid;
            this.team_id = team_id;
        }

        /// <summary>
        /// This method compares on the UID to determine equality.
        /// </summary>
        /// <param name="obj">The other Cube.</param>
        /// <returns>True if the obj UID equals this UID.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is Cube)
            {
                Cube other = (Cube)obj;
                return this.uid.Equals(other.uid);
            }
            else
                return false;
        }
        
        /// <summary>
        /// This method hashes on the UID.
        /// </summary>
        /// <returns>The hash of the UID.</returns>
        public override int GetHashCode()
        {
            return this.uid.GetHashCode();
        }

        /// <summary>
        /// This method updates this cube with the other (which is assumed to have same id).
        /// This method is thread safe unlike other methods of updating this object.
        /// </summary>
        /// <param name="other">The cube containing update info.</param>
        /// <returns>This cube NOT A NEW CUBE.</returns>
        internal Cube UpdateCube(Cube other)
        {
            lock(this)
            {
                this.loc_x = other.loc_x;
                this.loc_y = other.loc_y;
                this.Mass = other.Mass;
                this.name = other.name;
                return this;
            }
        }

        /// <summary>
        /// This method performs Attrition on this cube and moves it to the desired location (reseting
        /// the desired location to null).
        /// </summary>
        /// <param name="eatAction">A callback that passes the eaten cube (by this cube) every time
        /// a cube gets eaten.</param>
        /// <returns>True if this cube has mutated.</returns>
        public bool ServerTick(Action<Cube> eatAction)
        {
            bool modified = false;
            if (this.MoveRequested)
            {
                this.MoveRequested = false;
                this.Move();
                modified = true;
            }
            if (!this.food)
            {
                Rectangle myRect = this.GetRect();
                foreach (Cube other in World.ServerSingleton)
                    if (this.CanEat(other, myRect))
                    {
                        this.EatCube(other);
                        eatAction(other);
                    }
                this.Attrition();
                modified = true;
            }
            return modified;
        }

        /// <summary>
        /// This kills toEat and adds toEat's mass to this.Mass.
        /// </summary>
        /// <param name="toEat">The Cube to eat.</param>
        public void EatCube(Cube toEat)
        {
            this.Mass += toEat.Mass;
            toEat.IsDead = true;
        }

        /// <summary>
        /// This moves this cube towards the desired location at the current speed.
        /// </summary>
        private void Move()
        {
            // Cache this to ensure consistency (this.DesiredLocation could be overwritten).
            Point desired = this.DesiredLocation;

            // Calculate unit vectors.
            double deltaX = desired.X - this.loc_x;
            double deltaY = desired.Y - this.loc_y;
            double mag = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            double unitX = deltaX / mag;
            double unitY = deltaY / mag;

            World world = World.ServerSingleton;
            this.loc_x = this.loc_x + unitX * this.Speed;
            this.loc_y = this.loc_y + unitY * this.Speed;
            this.loc_x = Math.Max(Math.Min(this.loc_x, world.MaxSizeX), 0);
            this.loc_y = Math.Max(Math.Min(this.loc_y, world.MaxSizeY), 0);
            this.Momentum /= GlobalConstants.Friction;
        }


        /// <summary>
        /// This returns whether momentum is active (in which case moving to new locations is not
        /// allowed).
        /// </summary>
        private bool MomentumActive
        {
            get
            {
                return this.Momentum > GlobalConstants.MinSplitSpeed;
            }
        }

        /// <summary>
        /// This method gets the mass of this cube plus any other cubes with a team id equal
        /// to this uid.
        /// </summary>
        public double TrueMass
        {
            get
            {
                double result = this.Mass;
                foreach (Cube cube in World.WorldSingleton)
                    if (cube.team_id == this.uid)
                        result += cube.Mass;
                return result;
            }
        }

        /// <summary>
        /// This method causes the player cube to loose mass as time goes on.
        /// </summary>
        /// <param name="attritionRate">The rate at which the cube looses mass.</param>
        /// <param name="minimum_mass"></param>
        public void Attrition()
        {         
            if (this.Mass <= GlobalConstants.MinCubeSize)
                return;

            Mass -= Math.Sqrt(Mass) / GlobalConstants.AttritionRate;
        }

        /// <summary>
        /// This indicates that this Cell is "dead" and has been eaten.
        /// </summary>
        public bool IsDead
        {
            get
            {
                return this.Mass <= 0.0;
            }

            set
            {
                this.Mass = 0;
                World.ServerSingleton.UpdateCubes(new Cube[] { this });
            }
        }

        /// <summary>
        /// This method converts this Cube into a Json representation.
        /// </summary>
        public string Json
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        /// <summary>
        /// This method splits this Cube into two.
        /// </summary>
        /// <param name="hasMomentum">Indicates whether the split cube will have momentum.</param>
        /// <returns>The other split cube.</returns>
        public Cube Split(bool hasMomentum)
        {
            int id = World.ServerSingleton.GetNewUID();
            Cube result = new Cube(id, this.team_id);
            this.Mass /= 2;

            result.argb_color = this.argb_color;
            result.food = this.food;
            result.loc_x = this.loc_x;
            result.loc_y = this.loc_y;
            result.Mass = this.Mass;
            result.name = this.name;
            result.DesiredLocation = this.DesiredLocation;
            if (hasMomentum)
                result.Momentum = GlobalConstants.SplitSpeed;

            return result;
        }

        /// <summary>
        /// This method determines whether this cube can eat toEat.
        /// </summary>
        /// <param name="toEat">The cube to eat.</param>
        /// <param name="myRectangle">My own rectangle (passed as parameter for performance).</param>
        /// <returns>True if this cube can eat toEat.</returns>
        private bool CanEat(Cube toEat, Rectangle myRectangle)
        {
            // myRectangle is a passed here for performance.
            return !this.food && this.Mass > toEat.Mass && myRectangle.Contains((int)toEat.loc_x, (int)toEat.loc_y) && this.team_id != toEat.team_id;
        }

        /// <summary>
        /// This extension returns a Rectangle from the cube's internal dimensions.
        /// </summary>
        /// <param name="self">The Cube to get a rectangle from.</param>
        /// <returns>A rectangle representing the cube.</returns>
        public Rectangle GetRect()
        {
            // This is not a property because it was originally an extension in the View.
            int cubeSize = (int)(this.Width);
            Point center = new Point((int)(this.loc_x), (int)(this.loc_y));
            Size size = new Size(cubeSize, cubeSize);
            return Cube.GetCenteredCube(new Rectangle(center, size));
        }

        /// <summary>
        /// Gets the upper left point of the input rectange assuming its point
        /// is supposed to be the center of the rectangle.
        /// </summary>
        /// <param name="rect">The input rectangle.</param>
        /// <returns>A rectangle with its center at the point of rect.</returns>
        public static Rectangle GetCenteredCube(Rectangle rect)
        {
            Point oldLoc = rect.Location;
            int x = oldLoc.X - rect.Width / 2;
            int y = oldLoc.Y - rect.Height / 2;
            return new Rectangle(x, y, rect.Width, rect.Height);
        }
    }
}
