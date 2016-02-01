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

        public static int CheckSector(Vector3 coord)
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

        public static IEnumerable<Vector3> GetNeighborNodes(Vector3 node, List<byte[,,]> Weight)
        {
            int sector, sectorF, sectorR, sectorB, sectorL;
            sector = CheckSector(node);
            var nodes = new List<Vector3>();

            sectorF = CheckSector(new Vector3(node.X, node.Y, node.Z - 1));
            sectorR = CheckSector(new Vector3(node.X + 1, node.Y, node.Z));
            sectorB = CheckSector(new Vector3(node.X, node.Y, node.Z + 1));
            sectorL = CheckSector(new Vector3(node.X - 1, node.Y, node.Z));

            // forward
            if (Weight[sectorF][(int)node.X, (int)node.Y, (int)node.Z - 1] > 0) nodes.Add(new Vector3(node.X, node.Y, node.Z - 1));

            // right
            if (Weight[sectorR][(int)node.X + 1, (int)node.Y, (int)node.Z] > 0) nodes.Add(new Vector3(node.X + 1, node.Y, node.Z));

            // backward
            if (Weight[sectorB][(int)node.X, (int)node.Y, (int)node.Z + 1] > 0) nodes.Add(new Vector3(node.X, node.Y, node.Z + 1));

            // left
            if (Weight[sectorL][(int)node.X - 1, (int)node.Y, (int)node.Z] > 0) nodes.Add(new Vector3(node.X - 1, node.Y, node.Z));

            return nodes;
        }
    }
}
