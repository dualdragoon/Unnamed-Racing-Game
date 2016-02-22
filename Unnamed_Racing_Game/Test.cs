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
    sealed class Player : KartEntity
    {
        private bool turnLeft, turnRight, grounded, accel, backward;
        public bool colliding;
        int currentRoom = 0;
        float collideFactor;
        string kart;
        Vector3 tempPos;

        public Player(Level level, string kart)
        {
            this.kart = kart;
            Level = level;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Model = Main.GameContent.Load<Model>(string.Format("Models/{0}", kart));

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

            Console.WriteLine(currentRoom);

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

            //for (int i = 0; i < Level.Rooms[/*currentRoom*/0].Walls.Meshes.Count; i++)
            for (int i = 0; i < Level.Rooms[currentRoom].Obstacles.Count; i++)
            {
                //BoundingSphere s1 = CollisionHelper.CreateBoundingSphere(Level.Rooms[/*currentRoom*/0].Walls.Meshes[i], Level.Rooms[/*currentRoom*/0].room, Level.Rooms[/*currentRoom*/0].Position),
                BoundingSphere s2 = CollisionHelper.CreateBoundingSphere(Model, World);
                BoundingBox b = Level.Rooms[currentRoom].Obstacles[i];

                /*Console.WriteLine(b.Maximum);
                Console.WriteLine(b.Minimum);*/

                colliding = Collision.BoxIntersectsSphere(ref b, ref s2);
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

            if (Main.CurrentKeyboard.IsKeyPressed(Keys.D2)) position = Level.Rooms[0].Position;
        }

        private void Input()
        {
            grounded = (position.Y <= Level.Rooms[currentRoom].YValue);
            turnLeft = Main.CurrentKeyboard.IsKeyDown(Keys.A);
            turnRight = Main.CurrentKeyboard.IsKeyDown(Keys.D);
            accel = Main.CurrentKeyboard.IsKeyDown(Keys.W);
            backward = Main.CurrentKeyboard.IsKeyDown(Keys.S);
            collideFactor = (velocity >= 0) ? 1.25f : -1.25f;

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