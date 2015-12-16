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
        public float angle, frameTime;
        private Level level;
        private Matrix translation, view, projection, rotation;
        private Model model;
        public Vector3 rotationInRadians, position, gravitationalAcceleration = new Vector3(0, -(10.260796f / .22f) * 2f, 0), acceleration = new Vector3(0, 0, (10.260796f / .22f) * 2f), velocity, friction = new Vector3(0, 0, -(10.260796f / .44f) * 2f);

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

        public KartEntity(Level level)
        {
            angle = 0;
            World = Matrix.Zero;
            View = Matrix.Zero;
            Projection = Matrix.Zero;
            position = Vector3.Zero;
            acceleration = Vector3.Zero;
            velocity = Vector3.Zero;
            Level = level;
        }

        public virtual void LoadContent()
        {
            Effect = new BasicEffect(null);
            Model = null;
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
