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
            position = Level.Player.Position - new Vector3(0, -7.5f, 15);
            transformedPos = Vector3.Transform(position - Level.Player.Position, Matrix.RotationAxis(new Vector3(Level.Player.Position.X, 1, Level.Player.Position.Z), Level.Player.angle));
            position = new Vector3(transformedPos.X, Level.Player.Position.Y + transformedPos.Y, transformedPos.Z);

            view = Matrix.LookAtLH(position, Level.Player.Position, Vector3.Up);
        }
    }
}
