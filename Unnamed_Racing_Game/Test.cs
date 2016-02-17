﻿using System;
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
        private bool turnLeft, turnRight, grounded, accel, backward;
        public bool colliding;
        int currentRoom, collideFactor;
        Vector3 tempPos;

        public Test(Level level)
        {
            Level = level;
        }

        public override void LoadContent()
        {
            Model = Main.GameContent.Load<Model>(string.Format("Models/{0}", stringT));

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

            currentRoom = CurrentRoom();

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

            Input();

            for (int i = 0; i < Level.Rooms[currentRoom].Walls.Meshes.Count; i++)
            {
                BoundingSphere s1 = CollisionHelper.CreateBoundingSphere(Level.Rooms[currentRoom].Walls, Level.Rooms[currentRoom].trans), s2 = CollisionHelper.CreateBoundingSphere(Model, World);
                colliding = Collision.SphereIntersectsSphere(ref s1, ref s2);
                if (!colliding) tempPos = position - World.Backward * collideFactor;
                else
                {
                    position = tempPos;
                    velocity = .5f;
                }
            }

            //angle = MathUtil.DegreesToRadians(s.ElapsedMilliseconds / 15.625f);
            World = Rotation * Matrix.Translation(position);

            if (Main.CurrentKeyboard.IsKeyPressed(Keys.D1)) position = Vector3.Zero;

            if (Main.CurrentKeyboard.IsKeyPressed(Keys.D2)) position = new Vector3(-10, 0, 0);
        }

        private void Input()
        {
            grounded = (position.Y <= Level.Rooms[currentRoom].YValue);
            turnLeft = Main.CurrentKeyboard.IsKeyDown(Keys.A);
            turnRight = Main.CurrentKeyboard.IsKeyDown(Keys.D);
            accel = Main.CurrentKeyboard.IsKeyDown(Keys.W);
            backward = Main.CurrentKeyboard.IsKeyDown(Keys.S);
            collideFactor = (velocity >= 0) ? 2 : -2;

            if (turnLeft && !turnRight) angle -= MathUtil.DegreesToRadians(5);
            else if (turnRight && !turnLeft) angle += MathUtil.DegreesToRadians(5);

            if (accel && velocity < 75) velocity += acceleration * frameTime;

            if (backward && velocity > -37.5) velocity -= acceleration * frameTime;

            if (!accel && velocity > 0)
            {
                velocity += friction * frameTime;
                if (velocity <= 0) velocity = 0;
            }

            if (!backward && velocity < 0)
            {
                velocity -= friction * frameTime;
                if (velocity >= 0) velocity = 0;
            }

            position -= World.Forward * (velocity * frameTime);
        }
    }
}