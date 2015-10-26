using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Kross_Kart
{
    class Test
    {
        float angle = 0;
        Matrix translation, view, projection;
        Model sphere;
        Stopwatch s = new Stopwatch();
        Vector3 position = new Vector3(0, -1, 0), temp;

        public Test()
        {
            
        }

        public void LoadContent()
        {
            sphere = Main.GameContent.Load<Model>("Test/Sphere");

            BasicEffect.EnableDefaultLighting(sphere);
        }

        public void Update(GameTime gameTime, Matrix view, Matrix projection)
        {
            this.view = view;
            this.projection = projection;

            if (!s.IsRunning)
            {
                s.Start(); 
            }

            angle += .03f;
            position += new Vector3(-.01f, -.01f, 0);
            temp = new Vector3(position.ToArray());
            temp.Normalize();
            translation = Matrix.Translation(position) * Matrix.RotationAxis(temp, MathUtil.DegreesToRadians(s.ElapsedMilliseconds));

            if (s.ElapsedMilliseconds == 360)
            {
                s.Restart();
            }

            if (Main.CurrentKeyboard.IsKeyPressed(Keys.A))
            {
                position = new Vector3(0, -1, 0);
            }
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            sphere.Draw(graphicsDevice, translation, view, projection);
        }
    }
}
