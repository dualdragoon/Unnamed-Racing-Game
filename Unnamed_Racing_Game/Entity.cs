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
        public float angle, frameTime, acceleration = (10.260796f / .22f) * 2f, velocity, yVelocity, friction = -(10.260796f / .44f) * 2f, gravitationalAcceleration = -(10.260796f / .22f) * 2f;
        private Level level;
        private List<byte[,,]> Weight = new List<byte[,,]>(8);
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

        public KartEntity(Level level, List<byte[,,]> weight)
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
            Effect = new BasicEffect(null);
            Model = null;
        }

        public List<Vector3> Pathfind(Vector3 start, Vector3 end)
        {
            var closedSet = new List<Vector3>();
            var openSet = new List<Vector3> { start };
            var cameFrom = new Dictionary<Vector3, Vector3>();
            var currentDistance = new Dictionary<Vector3, int>();
            var predictedDistance = new Dictionary<Vector3, float>();

            currentDistance.Add(start, 0);
            predictedDistance.Add(start, 0 + +Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y) + Math.Abs(start.Z - end.Z));

            while (openSet.Count > 0)
            {
                var current = (from p in openSet orderby predictedDistance[p] ascending select p).First();

                if (current.X == end.X && current.Y == end.Y && current.Z == end.Z) return ReconstructPath(cameFrom, end);

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in GetNeighborNodes(current))
                {
                    var tempCurrentDistance = currentDistance[current] + 1;

                    if (closedSet.Contains(neighbor) && tempCurrentDistance >= currentDistance[neighbor]) continue;

                    if (!closedSet.Contains(neighbor) || tempCurrentDistance < currentDistance[neighbor])
                    {
                        if (cameFrom.Keys.Contains(neighbor)) cameFrom[neighbor] = current;
                        else cameFrom.Add(neighbor, current);

                        currentDistance[neighbor] = tempCurrentDistance;
                        predictedDistance[neighbor] = currentDistance[neighbor] + Math.Abs(neighbor.X - end.X) + Math.Abs(neighbor.Y - end.Y) + Math.Abs(neighbor.Z - end.Z);

                        if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                    }
                }
            }

            throw new Exception(string.Format("Unable to find a path between {0},{1},{2} and {3},{4},{5}", start.X, start.Y, start.Z, end.X, end.Y, end.Z));
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

        public virtual void ApplyGravity()
        {
            velocity += gravitationalAcceleration * frameTime;
            position += velocity * frameTime;
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            Model.Draw(graphicsDevice, World, View, Projection, Effect);
        }
    }
}
