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
        public Matrix translation, view, projection;
        private Model model;
        private Test test;
        private Vector3 position = new Vector3(0, -10, 0);

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
            test = new Test(this);
            test.OnCreated += OnPlayerCreate;
        }

        public void LoadContent()
        {
            model = Main.GameContent.Load<Model>("Test/Test Room");

            effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            /*effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            effect.DirectionalLight0.Direction = new Vector3(1, -10, -1);
            effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);*/
            effect.EnableDefaultLighting();

            test.LoadContent();

            projection = Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(45), 800f / 480f, .1f, 100f);
        }

        public void Update(GameTime gameTime)
        {
            translation = Matrix.Translation(position);

            test.Update(gameTime, view, projection);

            try
            {
                view = Cam.View;
            }
            catch
            {}
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            model.Draw(graphicsDevice, translation, view, projection, effect);
            test.Draw(graphicsDevice);
        }

        private void OnPlayerCreate(object sender, EventArgs args)
        {
            view = Cam.View;
            test.OnCreated -= OnPlayerCreate;
        }
    }
}
