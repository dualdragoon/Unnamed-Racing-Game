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
    class Level
    {
        private BasicEffect effect;
        private Camera cam;
        private List<byte[, ,]> weight = new List<byte[, ,]>(8);
        private List<Room> rooms;
        private Main main;
        public Matrix view, projection;
        private SpriteFont font;
        private Test test;
        private Vector3 position = new Vector3(0, -10, 0), lowerWeightBound = new Vector3(-10), upperWeightBound = new Vector3(10);

        #region Properties

        public BasicEffect Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        public Camera Cam
        {
            get { return cam; }
            set { cam = value; }
        }

        public List<Room> Rooms
        {
            get { return rooms; }
        }

        public Test Player
        {
            get { return test; }
            set { test = value; }
        }

        #endregion

        public Level(Main main)
        {
            Cam = new Camera(this);
            Player = new Test(this);
            test.OnCreated += OnPlayerCreate;
            this.main = main;
        }

        public void LoadContent()
        {
            LoadRooms();

            foreach (Room r in rooms)
            {
                r.LoadContent();
            }

            font = Main.GameContent.Load<SpriteFont>("Font/Font");

            effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            /*effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            effect.DirectionalLight0.Direction = new Vector3(1, -10, -1);
            effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);*/
            effect.EnableDefaultLighting();

            test.LoadContent();

            projection = Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(45), 800f / 600f, .1f, 100f);
        }

        private void LoadRooms()
        {
            rooms = new List<Room>();
            rooms.Add(new Room(this, "Test Room"));
        }

        public void Update(GameTime gameTime)
        {
            Cam.Update(gameTime);
            Player.Update(gameTime, view, projection);

            view = Cam.View;

            if (Main.CurrentKeyboard.IsKeyPressed(Keys.Escape))
            {
                main.GameState = GameStates.Pause;
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            foreach (Room r in rooms)
            {
                r.Draw(graphicsDevice);
            }

            test.Draw(graphicsDevice);

            spriteBatch.DrawString(font, Player.colliding.ToString(), Vector2.Zero, Color.Black);
        }

        private void OnPlayerCreate(object sender, EventArgs args)
        {
            view = Cam.View;
            test.OnCreated -= OnPlayerCreate;
        }

        private void SetPosWeight()
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int l = 0; l < rooms[i].Walls.Meshes.Count; l++)
                {
                    for (int k = (int)lowerWeightBound.Y; k < upperWeightBound.Y; k++)
                    {
                        for (int j = (int)lowerWeightBound.X; j <= upperWeightBound.X; j++)
                        {
                            for (int o = (int)lowerWeightBound.Z; o <= upperWeightBound.Z; o++)
                            {
                                BoundingSphere sphere = rooms[i].Walls.Meshes[l].BoundingSphere;
                                Vector3 point = new Vector3(j, k, o);
                                if (weight[NodeHelper.CheckSector(point)][j, k, o] != (byte)1) weight[NodeHelper.CheckSector(point)][j, k, o] = (Collision.SphereContainsPoint(ref sphere, ref point) == ContainmentType.Contains) ? (byte)1 : (byte)0;
                            }
                        }
                    }
                }
            }
        }
    }
}
