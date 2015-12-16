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
        bool turnLeft, turnRight, grounded, accel;
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

            Rotation = Matrix.Identity;
        }

        public void Update(GameTime gameTime, Matrix view, Matrix projection)
        {
            this.View = view;
            this.Projection = projection;

            frameTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (grounded)
            {
                yVelocity = 0;
            }
            else
            {
                ApplyGravity();
            }

            Rotation = Matrix.RotationY(angle);

            if (!s.IsRunning)
            {
                s.Start();
            }

            Input();

            //angle = MathUtil.DegreesToRadians(s.ElapsedMilliseconds / 15.625f);
            World = Rotation * Matrix.Translation(position);

            if (s.ElapsedMilliseconds == 5625)
            {
                s.Restart();
            }

            if (Main.CurrentKeyboard.IsKeyPressed(Keys.D1))
            {
                position = Vector3.Zero;
            }
        }

        public override void ApplyGravity()
        {
            if (yVelocity > -19)
            {
                yVelocity -= gravitationalAcceleration * frameTime; 
            }
            position += World.Down * (yVelocity * frameTime);
        }

        private void Input()
        {
            grounded = CollisionHelper.IsCollision(this, Level);
            turnLeft = Main.CurrentKeyboard.IsKeyDown(Keys.A);
            turnRight = Main.CurrentKeyboard.IsKeyDown(Keys.D);
            accel = Main.CurrentKeyboard.IsKeyDown(Keys.W);

            if (turnLeft && !turnRight)
            {
                angle -= MathUtil.DegreesToRadians(5);
            }
            else if (turnRight && ! turnLeft)
            {
                angle += MathUtil.DegreesToRadians(5);
            }

            if (accel)
            {
                velocity += acceleration * frameTime;
            }

            if (!accel && velocity > 0)
            {
                velocity += friction * frameTime;
                if (velocity <= 0)
                {
                    velocity = 0;
                }
            }

            position -= World.Forward * (velocity * frameTime);
        }
    }
}