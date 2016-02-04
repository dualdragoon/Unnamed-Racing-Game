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
        private float yValue;
        private Level level;
        public Matrix translation;
        private Model walls, floor;
        private string room;
        private Vector3 position = new Vector3(0, -10, 0);

        #region Properties
        public float YValue
        {
            get 
            {
                yValue = position.Y + 1.5f;
                return yValue;
            }
        }

        public Model Walls
        {
            get { return walls; }
        }
        #endregion

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
                floor = Main.GameContent.Load<Model>(string.Format("Test/{0} floor", room));
                walls = Main.GameContent.Load<Model>(string.Format("Test/{0} walls", room));
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
