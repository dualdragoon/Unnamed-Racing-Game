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
    /// <summary>
    /// Player Kart class.
    /// </summary>
    sealed class Test : Entity
    {
        Stopwatch s = new Stopwatch();

        public Test()
        {
            
        }

        public override void LoadContent()
        {
            model = Main.GameContent.Load<Model>("Test/Sphere");

            //BasicEffect.EnableDefaultLighting(model);

            effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = Color.BurlyWood.ToVector3();
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
            translation = Matrix.RotationY(MathUtil.DegreesToRadians(s.ElapsedMilliseconds / 15.625f)) * Matrix.Translation(position);

            if (s.ElapsedMilliseconds == 5625)
            {
                s.Restart();
            }

            if (Main.CurrentKeyboard.IsKeyPressed(Keys.A))
            {
                position = Vector3.Zero;
            }
        }
    }
}