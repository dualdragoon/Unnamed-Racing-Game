using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Kross_Kart
{
    static class NodeHelper
    {

        public int CheckSector(Vector3 coord)
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

        private IEnumerable<Vector3> GetNeighborNodes(Vector3 node, List<byte[,,]> Weight)
        {
            int sector, sectorF, sectorR, sectorB, sectorL;
            sector = CheckSector(node);
            var nodes = new List<Vector3>();
            
            sectorF = ((node.Z - 1) < 0) ? 

            // forward
            if (Weight[sector][(int)node.X, (int)node.Y, (int)node.Z - 1] > 0) nodes.Add(new Vector3(node.X, node.Y, node.Z - 1));

            // right
            if (Weight[sector][(int)node.X + 1, (int)node.Y, (int)node.Z] > 0) nodes.Add(new Vector3(node.X + 1, node.Y, node.Z));

            // backward
            if (Weight[sector][(int)node.X, (int)node.Y, (int)node.Z + 1] > 0) nodes.Add(new Vector3(node.X, node.Y, node.Z + 1));

            // left
            if (Weight[sector][(int)node.X - 1, (int)node.Y, (int)node.Z] > 0) nodes.Add(new Vector3(node.X - 1, node.Y, node.Z));

            return nodes;
        }
    }
}
