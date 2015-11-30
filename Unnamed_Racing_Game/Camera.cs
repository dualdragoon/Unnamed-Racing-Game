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
            position = Level.Player.Position - new Vector3(0, -10, 15);

            view = Matrix.LookAtLH(position, Level.Player.Position, Vector3.Up);
        }

    }
}
