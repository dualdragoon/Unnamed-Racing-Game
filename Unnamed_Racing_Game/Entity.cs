using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace Kross_Kart
{
    /// <summary>
    /// Base class for Kart Entities
    /// </summary>
    class KartEntity
    {
        private BasicEffect effect;
        private event EventHandler onCreated;
        public float angle, frameTime, acceleration = (10.260796f / .11f) * 2f,
            velocity, yVelocity, friction = -(10.260796f / .44f) * 2f,
            gravitationalAcceleration = -(10.260796f / .22f) * 2f;
        private Level level;
        protected byte[][,] Weight = new byte[8][,];
        private Matrix translation, view, projection, rotation;
        private Model model;
        public Vector3 rotationInRadians, position;

        #region Properties
        public BasicEffect Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        public event EventHandler OnCreated
        {
            add { onCreated += value; }
            remove { onCreated -= value; }
        }

        public Level Level
        {
            get { return level; }
            set { level = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        public Matrix World
        {
            get { return translation; }
            set { translation = value; }
        }

        public Matrix Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }
        #endregion

        public KartEntity()
        {

        }

        public KartEntity(Level level, byte[][,] weight)
        {
            acceleration = 0;
            velocity = 0;
            angle = 0;
            World = Matrix.Zero;
            View = Matrix.Zero;
            Projection = Matrix.Zero;
            position = Vector3.Zero;
            Level = level;
            Weight = weight;
        }

        public virtual void LoadContent()
        {

        }

        public List<Vector3> Pathfind(Vector3 start, Vector3 end)
        {
            Vector3 pos = new Vector3(start.X, -8.2f, start.Z), endPos = new Vector3(end.X, -8.2f, end.Z);
            var closedSet = new List<Vector3>();
            var openSet = new List<Vector3>();
            openSet.Add(pos);
            var cameFrom = new Dictionary<Vector3, Vector3>();
            var currentDistance = new Dictionary<Vector3, int>();
            var predictedDistance = new Dictionary<Vector3, float>();

            currentDistance.Add(pos, 0);
            predictedDistance.Add(pos, 0 + +Math.Abs(pos.X - endPos.X) + Math.Abs(pos.Y - endPos.Y) + Math.Abs(pos.Z - endPos.Z));

            while (openSet.Count > 0)
            {
                var current = (from p in openSet orderby predictedDistance[p] ascending select p).First();

                if (current.X == endPos.X && current.Y == endPos.Y && current.Z == endPos.Z) return ReconstructPath(cameFrom, endPos);

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in NodeHelper.GetNeighborNodes(current, Weight))
                {
                    var tempCurrentDistance = currentDistance[current] + 1;

                    if (closedSet.Contains(neighbor) && tempCurrentDistance >= currentDistance[neighbor]) continue;

                    if (!closedSet.Contains(neighbor) || tempCurrentDistance < currentDistance[neighbor])
                    {
                        if (cameFrom.Keys.Contains(neighbor)) cameFrom[neighbor] = current;
                        else cameFrom.Add(neighbor, current);

                        currentDistance[neighbor] = tempCurrentDistance;
                        predictedDistance[neighbor] = currentDistance[neighbor] + Math.Abs(neighbor.X - endPos.X) + Math.Abs(neighbor.Y - endPos.Y) + Math.Abs(neighbor.Z - endPos.Z);

                        if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                    }
                }
            }

            throw new Exception(string.Format("Unable to find a path between {0},{1},{2} and {3},{4},{5}", pos.X, pos.Y, pos.Z, endPos.X, endPos.Y, endPos.Z));
        }

        private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
        {
            if (!cameFrom.Keys.Contains(current))
            {
                return new List<Vector3> { current };
            }

            var path = ReconstructPath(cameFrom, cameFrom[current]);
            path.Add(current);
            return path;
        }

        public int CurrentRoom()
        {
            for (int i = 0; i < Level.Rooms.Count; i++)
            {
                if (Collision.BoxContainsPoint(ref Level.boxes[i], ref position) == ContainmentType.Contains)
                {
                    return i;
                }
            }
            return Level.Rooms.Count - 1;
        }

        public virtual void ApplyGravity()
        {
            if (yVelocity > -19)
            {
                yVelocity -= gravitationalAcceleration * frameTime;
            }
            position += World.Down * (yVelocity * frameTime);
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            Model.Draw(graphicsDevice, World, View, Projection, Effect);
        }
    }
}
