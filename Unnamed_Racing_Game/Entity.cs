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
        public BasicEffect effect;
        public float angle, frameTime;
        public Matrix translation, view, projection;
        public Model model;
        public Vector3 position, gravitationalAcceleration = new Vector3(0, -(10.260796f/.22f)*2f, 0), acceleration, velocity;

        public KartEntity()
        {
            angle = 0;
            translation = Matrix.Zero;
            view = Matrix.Zero;
            projection = Matrix.Zero;
            position = Vector3.Zero;
            acceleration = Vector3.Zero;
            velocity = Vector3.Zero;
        }

        public virtual void LoadContent()
        {
            effect = new BasicEffect(null);
            model = null;
        }

        public virtual void ApplyGravity()
        {
            velocity += gravitationalAcceleration * frameTime;
            position += velocity * frameTime;
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            model.Draw(graphicsDevice, translation, view, projection, effect);
        }
    }
}
