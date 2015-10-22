using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Vector3 position = new Vector3(0, -1, 0);

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

            angle += .03f;
            position += new Vector3(0, -.01f, 0);
            translation = Matrix.Translation(position) * Matrix.RotationAxis(position, angle);

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
