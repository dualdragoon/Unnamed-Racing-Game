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
    sealed class Test : KartEntity
    {
        Stopwatch s = new Stopwatch();

        public Test(Level level)
        {
            Level = level;
        }

        public override void LoadContent()
        {
            Model = Main.GameContent.Load<Model>("Test/Test Kart");

            //BasicEffect.EnableDefaultLighting(model);

            Effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            Effect.LightingEnabled = true;
            Effect.DirectionalLight0.DiffuseColor = Color.BurlyWood.ToVector3();
            Effect.DirectionalLight0.Direction = new Vector3(1, 0, -1);
            Effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);
        }

        public void Update(GameTime gameTime, Matrix view, Matrix projection)
        {
            this.View = view;
            this.Projection = projection;

            frameTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (!CollisionHelper.IsCollision(this, Level))
            {
                ApplyGravity();
            }

            if (!s.IsRunning)
            {
                s.Start();
            }

            angle += .03f;
            World = Matrix.RotationY(MathUtil.DegreesToRadians(s.ElapsedMilliseconds / 15.625f)) * Matrix.Translation(Position);

            if (s.ElapsedMilliseconds == 5625)
            {
                s.Restart();
            }

            if (Main.CurrentKeyboard.IsKeyPressed(Keys.A))
            {
                Position = Vector3.Zero;
            }
        }

        public override void ApplyGravity()
        {
            if (Velocity.Y > -19)
            {
                Velocity += GravitationalAcceleration * frameTime; 
            }
            Position += Velocity * frameTime;
        }
    }
}