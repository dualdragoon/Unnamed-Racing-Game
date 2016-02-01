using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace Kross_Kart
{
    class Room
    {
        private Level level;
        public Matrix translation;
        private Model walls, floor;
        private string room;
        private Vector3 position = new Vector3(0, -10, 0);

        public Model Walls
        {
            get { return walls; }
        }

        public Room(Level level, string roomTitle)
        {
            this.level = level; 
            room = roomTitle;
            translation = Matrix.Translation(position);
        }

        public void LoadContent()
        {
            try
            {
                floor = Main.GameContent.Load<Model>("" + room + " floor");
                walls = Main.GameContent.Load<Model>("" + room + " walls");
            }
            catch { walls = Main.GameContent.Load<Model>(string.Format("Test/{0}", room)); }
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            try
            {
                Walls.Draw(graphicsDevice, translation, level.view, level.projection, level.Effect);
                floor.Draw(graphicsDevice, translation, level.view, level.projection, level.Effect);
            }
            catch {}
        }
    }
}
