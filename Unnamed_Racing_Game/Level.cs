using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace Kross_Kart
{
    class Level
    {
        private BasicEffect effect;
        private Camera cam;
        private List<byte[, ,]> weight = new List<byte[, ,]>(8);
        private List<Model> walls;
        public Matrix translation, view, projection;
        private Model model;
        private SpriteFont font;
        private Test test;
        private Vector3 position = new Vector3(0, -10, 0), lowerWeightBound, upperWeightBound;

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

        public Model WorldModel
        {
            get { return model; }
            set { model = value; }
        }

        public Test Player
        {
            get { return test; }
            set { test = value; }
        }

        #endregion

        public Level()
        {
            Cam = new Camera(this);
            Player = new Test(this);
            test.OnCreated += OnPlayerCreate;
        }

        public void LoadContent()
        {
            model = Main.GameContent.Load<Model>("Test/Test2");
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

        public void Update(GameTime gameTime)
        {
            translation = Matrix.Translation(position);

            Cam.Update(gameTime);
            Player.Update(gameTime, view, projection);

            view = Cam.View;
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            model.Draw(graphicsDevice, translation, view, projection, effect);
            test.Draw(graphicsDevice);

            spriteBatch.DrawString(font, CollisionHelper.IsCollision(Player, this).ToString(), Vector2.Zero, Color.Black);
        }

        private void OnPlayerCreate(object sender, EventArgs args)
        {
            view = Cam.View;
            test.OnCreated -= OnPlayerCreate;
        }

        private void SetPosWeight()
        {
            for (int i = 0; i < walls.Count; i++)
            {

            }
        }

        private int CheckSector(Vector3 coord)
        {
            List<bool> sector = new List<bool>(8);
            sector[0] = (coord.X > 0 && coord.Y > 0 && coord.Z > 0);
            sector[1] = (coord.X < 0 && coord.Y > 0 && coord.Z > 0);
            sector[2] = (coord.X < 0 && coord.Y > 0 && coord.Z < 0);
            sector[3] = (coord.X > 0 && coord.Y > 0 && coord.Z < 0);
            sector[4] = (coord.X > 0 && coord.Y < 0 && coord.Z > 0);
            sector[5] = (coord.X < 0 && coord.Y < 0 && coord.Z > 0);
            sector[6] = (coord.X < 0 && coord.Y < 0 && coord.Z < 0);
            sector[7] = (coord.X > 0 && coord.Y < 0 && coord.Z < 0);

            for (int i = 0; i < sector.Count; i++)
            {
                if (sector[i]) return i++;
            }
            return 0;
        }
    }
}
