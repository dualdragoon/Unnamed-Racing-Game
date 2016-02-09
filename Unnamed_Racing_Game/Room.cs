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
        public Matrix trans;
        private Model walls, floor;
        private string room;
        private Vector3 floorPos = new Vector3(0, -10, 0);

        #region Properties
        public float YValue
        {
            get 
            {
                yValue = floorPos.Y + 1.5f;
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
            trans = Matrix.Translation(floorPos);
        }

        public void LoadContent()
        {
            try
            {
                floor = Main.GameContent.Load<Model>(string.Format("Models/Rooms/{0} floor", room));
                walls = Main.GameContent.Load<Model>(string.Format("Models/Rooms/{0} walls", room));
            }
            catch { walls = Main.GameContent.Load<Model>(string.Format("Models/Rooms/{0}", room)); }
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            try
            {
                Walls.Draw(graphicsDevice, trans, level.view, level.projection, level.Effect);
                floor.Draw(graphicsDevice, trans, level.view, level.projection, level.Effect);
            }
            catch {}
        }
    }
}
