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
        public string room;
        private Vector3 floorPos;

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

        public Model Floor
        {
            get { return floor; }
        }
        #endregion

        public Room(Level level, string roomTitle, Vector3 pos)
        {
            this.level = level; 
            room = roomTitle;
            floorPos = pos;
            trans = Matrix.Translation(floorPos);
        }

        public void LoadContent()
        {
            try
            {
                floor = Main.GameContent.Load<Model>("Models/Rooms/Floor");
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
