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
        private Matrix translation;
        private Model walls, floor;
        private string room;
        private Vector3 position;

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
            floor = Main.GameContent.Load<Model>("" + room + " floor");
            walls = Main.GameContent.Load<Model>("" + room + " walls");
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            Walls.Draw(graphicsDevice, translation, level.view, level.projection, level.Effect);
            floor.Draw(graphicsDevice, translation, level.view, level.projection, level.Effect);
        }
    }
}
