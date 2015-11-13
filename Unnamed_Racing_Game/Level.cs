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
        public BasicEffect effect;
        public Matrix translation, view, projection;
        public Model model;
        public Vector3 position = new Vector3(0, -10, 0);

        public void LoadContent()
        {
            model = Main.GameContent.Load<Model>("Test/Test Room");

            effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            /*effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            effect.DirectionalLight0.Direction = new Vector3(1, -10, -1);
            effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);*/
            effect.EnableDefaultLighting();
        }

        public void Update(GameTime gameTime, Matrix view, Matrix projection)
        {
            this.view = view;
            this.projection = projection;

            translation = Matrix.Translation(position);
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            model.Draw(graphicsDevice, translation, view, projection, effect);
        }
    }
}
