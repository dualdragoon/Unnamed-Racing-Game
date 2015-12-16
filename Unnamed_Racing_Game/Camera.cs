using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Kross_Kart
{
    class Camera
    {
        private Level level;
        private Matrix view;
        private Vector3 position;
        private Vector4 transformedPos;

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        public Level Level
        {
            get { return level; }
            set { level = value; }
        }

        public Camera(Level level)
        {
            Level = level;
        }

        public void Update(GameTime gameTime)
        {
            position = (Level.Player.position - (Level.Player.World.Backward * 15)) + new Vector3(0, 7.5f, 0);
            //transformedPos = Vector3.Transform(position, Level.Player.Rotation);
            //position = new Vector3(transformedPos.X, Level.Player.position.Y + transformedPos.Y, transformedPos.Z) - Level.Player.position;

            view = Matrix.LookAtLH(position, Level.Player.position, Vector3.Up);
        }
    }
}
