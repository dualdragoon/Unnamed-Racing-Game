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
    class Entity
    {
        public BasicEffect effect;
        public float angle;
        public Matrix translation, view, projection;
        public Model model;
        public Vector3 position;

        public Entity()
        {
            angle = 0;
            translation = Matrix.Zero;
            view = Matrix.Zero;
            projection = Matrix.Zero;
            position = Vector3.Zero;
        }

        public virtual void LoadContent()
        {
            effect = new BasicEffect(null);
            model = null;
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            model.Draw(graphicsDevice, translation, view, projection, effect);
        }
    }
}
