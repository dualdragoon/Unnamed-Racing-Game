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
        BasicEffect effect;
        float angle = 0;
        Matrix translation, view, projection;
        Model sphere;
        Stopwatch s = new Stopwatch();
        Vector3 position = new Vector3(0, 0, 0), temp;

        public Test()
        {
            
        }

        public void LoadContent()
        {
            sphere = Main.GameContent.Load<Model>("Test/Sphere");

            //BasicEffect.EnableDefaultLighting(sphere);

            effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = Color.Aqua.ToVector3();
            effect.DirectionalLight0.Direction = new Vector3(1, 0, -1);
            effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);
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
            //position += new Vector3(0, -.01f, 0);
            temp = new Vector3(0, 1, 0);
            temp.Normalize();
            translation = Matrix.RotationY(MathUtil.DegreesToRadians(s.ElapsedMilliseconds / 15.625f)) * Matrix.Translation(position);

            if (s.ElapsedMilliseconds == 5625)
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
            sphere.Draw(graphicsDevice, translation, view, projection, effect);
        }
    }
}