using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool complete = false;
        public BoundingBox[] boxes;
        private byte[][,] weight = new byte[8][,];
        private Camera cam;
        private int trackLength, trackWidth, cells, meshes = 0, currentMesh = 0;
        private List<Room> rooms;
        private Main main;
        public Matrix view, projection;
        private Random rand = new Random();
        private SpriteFont font;
        private Texture2D background, loadBar, loadBarBack;
        private Thread t;
        private ThreadStart ts;
        private AIKart test;
        private Vector3 lowerWeightBound, upperWeightBound, roomPos, roomPlacementX = new Vector3(79.68f, 0, 0), roomPlacementZ = new Vector3(0, 0, 79.68f);

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

        public AIKart Player
        {
            get { return test; }
            set { test = value; }
        }

        #endregion

        public Level(Main main, string kart)
        {
            Cam = new Camera(this);
            Player = new AIKart(rand.Next(), this, weight);
            //Player = new Player(this, kart);
            test.OnCreated += OnPlayerCreate;
            this.main = main;
        }

        public void LoadContent()
        {
            LoadRooms();

            boxes = new BoundingBox[Rooms.Count];
            for (int i = 0; i < Rooms.Count; i++)
            {
                boxes[i] = Rooms[i].Cell;
                meshes += Rooms[i].Obstacles.Count;
            }

            BoundingBox b = boxes[0];

            for (int i = 1; i < boxes.Length; i++)
            {
                b = BoundingBox.Merge(b, boxes[i]);
            }

            lowerWeightBound = b.Minimum;
            upperWeightBound = b.Maximum;

            for (int i = 0; i < weight.Length; i++)
            {
                weight[i] = new byte[(int)(Math.Abs(lowerWeightBound.X) + upperWeightBound.X), (int)(Math.Abs(lowerWeightBound.Z) + upperWeightBound.Z)];
            }

            /*ts = new ThreadStart(SetPosWeight);

            t = new Thread(ts);
            t.Start();*/

            font = Main.GameContent.Load<SpriteFont>("Font/Font");
            background = Main.GameContent.Load<Texture2D>("Menus/Background 2");
            loadBar = Main.GameContent.Load<Texture2D>("Menus/Load Bar");
            loadBarBack = Main.GameContent.Load<Texture2D>("Menus/Load Bar Back");

            effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            /*effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            effect.DirectionalLight0.Direction = new Vector3(1, -10, -1);
            effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);*/
            effect.EnableDefaultLighting();

            test.LoadContent();

            projection = Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(45), 800f / 600f, .1f, 1000f);
        }

        /// <summary>
        /// Randomly generates a track.
        /// </summary>
        private void LoadRooms()
        {
            trackLength = rand.Next(5, 10);
            trackWidth = rand.Next(3, 5);
            cells = (2 * trackLength) + (2 * (trackWidth - 2));
            roomPos = new Vector3(0, -10, 0);

            rooms = new List<Room>();

            for (int i = 0; i < trackLength; i++)
            {
                if (i != 0)
                {
                    rooms.Add(new Room(this, "1", roomPos));
                }
                else if (i == 0)
                {
                    rooms.Add(new Room(this, "1 corner", roomPos));
                }
                else if (i == trackLength - 1)
                {
                    rooms.Add(new Room(this, "1", roomPos));
                }
                roomPos += roomPlacementZ;
            }

            for (int i = 0; i < trackWidth - 1; i++)
            {
                rooms.Add(new Room(this, "1", roomPos));
                roomPos += roomPlacementX;
            }

            for (int i = 0; i < trackLength + 1; i++)
            {
                rooms.Add(new Room(this, "1", roomPos));
                roomPos -= roomPlacementZ;
            }

            roomPos += roomPlacementZ;

            for (int i = 0; i < trackWidth - 2; i++)
            {
                roomPos -= roomPlacementX;
                rooms.Add(new Room(this, "Test Room", roomPos));
            }

            foreach (Room r in rooms)
            {
                r.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            //if (complete)
            {
                Cam.Update(gameTime);
                Player.Update(gameTime, view, projection);

                view = Cam.View;

                if (Main.CurrentKeyboard.IsKeyPressed(Keys.Escape))
                {
                    main.GameState = GameStates.Pause;
                }
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            //if (complete)
            {
                foreach (Room r in rooms)
                {
                    r.Draw(graphicsDevice);
                }

                test.Draw(graphicsDevice);

                spriteBatch.DrawString(font, Player.colliding.ToString(), Vector2.Zero, Color.Black);
            }
            /*else
            {
                spriteBatch.Draw(background, Vector2.Zero, Color.White);
                spriteBatch.Draw(loadBarBack, new Vector2(140, 495), Color.White);
                spriteBatch.Draw(loadBar, new RectangleF(150, 500, currentMesh / (float)meshes * 500, 25), Color.White);
                spriteBatch.DrawString(font, "Loading...", new Vector2(340, 450), Color.White);
            }*/
        }

        private void OnPlayerCreate(object sender, EventArgs args)
        {
            view = Cam.View;
            test.OnCreated -= OnPlayerCreate;
        }

        /// <summary>
        /// Sets weight for AI pathfinding.
        /// </summary>
        private void SetPosWeight()
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int l = 0; l < rooms[i].Obstacles.Count; l++)
                {
                    for (int j = (int)lowerWeightBound.X; j <= upperWeightBound.X; j++)
                    {
                        for (int o = (int)lowerWeightBound.Z; o <= upperWeightBound.Z; o++)
                        {
                            BoundingBox box = rooms[i].Obstacles[l];
                            Vector3 point = new Vector3(j, -8.2f, o);
                            if (weight[NodeHelper.CheckSector(point)][Math.Abs(j), Math.Abs(o)] != (byte)1) weight[NodeHelper.CheckSector(point)][Math.Abs(j), Math.Abs(o)] = (Collision.BoxContainsPoint(ref box, ref point) == ContainmentType.Contains) ? (byte)1 : (byte)0;
                        }
                    }
                    currentMesh++;
                }
            }
            complete = true;
        }
    }
}