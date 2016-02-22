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
    class AIKart : KartEntity
    {
        Dictionary<int, string> karts = new Dictionary<int, string>()
        {
            {0, "Test Kart"},
            {1, "TIE"},
            {2, "Shopping Cart"},
            {3, "Gumin"}
        };

        private bool turnLeft, turnRight, grounded, accel, backward;
        public bool colliding;
        int currentRoom = 0, nextRoom;
        float collideFactor, nextAngle;
        Random rand;
        Vector3 tempPos;

        public AIKart(int kartNum, Level level, byte[][,] weight)
        {
            rand = new Random(kartNum);
            Level = level;
            Weight = weight;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Model = Main.GameContent.Load<Model>(string.Format("Models/{0}", karts[rand.Next(0, 4)]));

            Effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            Effect.LightingEnabled = true;
            Effect.DirectionalLight0.DiffuseColor = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)).ToVector3();
            Effect.DirectionalLight0.Direction = new Vector3(1, 0, -1);
            Effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);
        }

        public void Update(GameTime gameTime, Matrix view, Matrix projection)
        {
            View = view;
            Projection = projection;

            currentRoom = CurrentRoom();

            Console.WriteLine(currentRoom);

            nextRoom = ((currentRoom + 1) == Level.Rooms.Count) ? 0 : currentRoom + 1;

            frameTime = gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (grounded)
            {
                yVelocity = 0;
            }
            else
            {
                ApplyGravity();
            }

            nextAngle = (float)Math.Atan2(Level.Rooms[nextRoom].Position.X - position.X, Level.Rooms[nextRoom].Position.Z - position.Z);

            Rotation = Matrix.RotationY(angle);

            Decisions();

            /*for (int i = 0; i < Level.Rooms[currentRoom].Obstacles.Count; i++)
            {
                BoundingSphere s2 = CollisionHelper.CreateBoundingSphere(Model, World);
                BoundingBox b = Level.Rooms[currentRoom].Obstacles[i];

                colliding = Collision.BoxIntersectsSphere(ref b, ref s2);
                if (!colliding) tempPos = position - World.Backward * collideFactor;
                else
                {
                    position = tempPos;
                    velocity = .5f;
                }
            }*/

            World = Rotation * Matrix.Translation(position);
        }

        private void Decisions()
        {
            grounded = (position.Y <= Level.Rooms[currentRoom].YValue);
            accel = !colliding;
            backward = colliding;
            collideFactor = (velocity >= 0) ? 1.25f : -1.25f;

            if (angle > nextAngle)
            {
                angle -= MathUtil.DegreesToRadians(5);
                if (angle - nextAngle < MathUtil.DegreesToRadians(5)) angle = nextAngle;
            }
            else if (angle < nextAngle)
            {
                angle += MathUtil.DegreesToRadians(5);
                if (nextAngle - angle < MathUtil.DegreesToRadians(5)) angle = nextAngle;
            }

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
