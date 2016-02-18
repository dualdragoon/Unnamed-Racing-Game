using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Kross_Kart
{
    class Camera
    {
        private Level level;
        private Matrix view;
        private Vector3 position, yPos = new Vector3(0, 7.5f, 0);
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
            if (Main.CurrentKeyboard.IsKeyDown(Keys.Up)) yPos.Y += .2f;
            if (Main.CurrentKeyboard.IsKeyDown(Keys.Down)) yPos.Y -= .2f;

            position = (Level.Player.position - (Level.Player.World.Backward * 15)) + yPos;
            //transformedPos = Vector3.Transform(floorPos, Level.Player.Rotation);
            //floorPos = new Vector3(transformedPos.X, Level.Player.floorPos.Y + transformedPos.Y, transformedPos.Z) - Level.Player.floorPos;

            view = Matrix.LookAtLH(position, Level.Player.position, Vector3.Up);
        }
    }
}
