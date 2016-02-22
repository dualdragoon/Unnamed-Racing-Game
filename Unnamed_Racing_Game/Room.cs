using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace Kross_Kart
{
    class Room
    {
        private BoundingBox cell;
        private float yValue;
        private Level level;
        private List<BoundingBox> obstacles;
        public Matrix trans;
        private Model walls, floor;
        public string room;
        private Vector3 floorPos;
        private XmlDocument read;

        #region Properties
        public BoundingBox Cell
        {
            get { return cell; }
        }

        public float YValue
        {
            get 
            {
                yValue = floorPos.Y + 1.5f;
                return yValue;
            }
        }

        public List<BoundingBox> Obstacles
        {
            get { return obstacles; }
        }

        public Model Walls
        {
            get { return walls; }
        }

        public Model Floor
        {
            get { return floor; }
        }

        public Vector3 Position
        {
            get { return floorPos; }
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

            Vector3 cellMin = Position - new Vector3(39.84f, 0, 39.84f);
            Vector3 cellMax = Position + new Vector3(39.84f, 20, 39.84f);

            cell = new BoundingBox(cellMin, cellMax);

            obstacles = new List<BoundingBox>();
            read = new XmlDocument();
            read.Load(string.Format("Content/Models/Rooms/XML/{0}.xml", room));

            for (int i = 0; i < walls.Meshes.Count; i++)
            {
                XmlNode t = read.SelectSingleNode(string.Format("/Meshes/{0}", walls.Meshes[i].Name));
                Vector3 center = new Vector3(float.Parse(t.Attributes["X"].Value),
                    float.Parse(t.Attributes["Y"].Value),
                    float.Parse(t.Attributes["Z"].Value));

                Vector3 min = (center - new Vector3(float.Parse(t.Attributes["Radius"].Value))) + Position;
                Vector3 max = (center + new Vector3(float.Parse(t.Attributes["Radius"].Value))) + Position;

                obstacles.Insert(i, new BoundingBox(min, max));
            }
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
