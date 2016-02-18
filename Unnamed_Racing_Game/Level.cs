﻿using System;
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
        private int trackLength, trackWidth, cells;
        private List<byte[, ,]> weight = new List<byte[, ,]>(8);
        private List<Room> rooms;
        private Main main;
        public Matrix view, projection;
        private Random rand = new Random();
        private SpriteFont font;
        private Player test;
        private Vector3 lowerWeightBound = new Vector3(-10), upperWeightBound = new Vector3(10), roomPos, roomPlacementX = new Vector3(10, 0, 0), roomPlacementZ = new Vector3(0, 0, 10);

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

        public Player Player
        {
            get { return test; }
            set { test = value; }
        }

        #endregion

        public Level(Main main, string kart)
        {
            Cam = new Camera(this);
            Player = new Player(this, kart);
            test.OnCreated += OnPlayerCreate;
            this.main = main;
        }

        public void LoadContent()
        {
            LoadRooms();

            font = Main.GameContent.Load<SpriteFont>("Font/Font");

            effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            /*effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            effect.DirectionalLight0.Direction = new Vector3(1, -10, -1);
            effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);*/
            effect.EnableDefaultLighting();

            test.LoadContent();

            projection = Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(45), 800f / 600f, .1f, 1000f);
        }

        private void LoadRooms()
        {
            trackLength = rand.Next(5, 10);
            trackWidth = rand.Next(3, 5);
            cells = (2 * trackLength) + (2 * (trackWidth - 2));
            roomPos = new Vector3(0, -10, 0);

            rooms = new List<Room>();

            for (int i = 0; i < trackLength; i++)
            {
                rooms.Add(new Room(this, "Test Room", roomPos));
                roomPos += i * roomPlacementX;
            }

            roomPos += roomPlacementZ;
            
            for (int i = 0; i < trackWidth - 1; i++)
            {
                rooms.Add(new Room(this, "Test Room", roomPos));
                roomPos += i * roomPlacementZ;
            }

            roomPos -= roomPlacementX;

            for (int i = 0; i < trackLength - 1; i++)
            {
                rooms.Add(new Room(this, "Test Room", roomPos));
                roomPos -= i * roomPlacementX;
            }

            roomPos -= roomPlacementZ;

            for (int i = 0; i < trackWidth - 2; i++)
            {
                rooms.Add(new Room(this, "Test Room", roomPos));
                roomPos -= i * roomPlacementZ;
            }

            foreach (Room r in rooms)
            {
                r.LoadContent();
            }
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
