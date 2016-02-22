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
        /// <summary>
        /// Returns the sector of a coordinate in a 3-Dimensional Grid.
        /// </summary>
        /// <param name="coord">Coordinate to check.</param>
        /// <returns></returns>
        public static int CheckSector(Vector3 coord)
        {
            bool[] sector = new bool[8];
            sector[0] = (coord.X > 0 && coord.Y > 0 && coord.Z > 0);
            sector[1] = (coord.X < 0 && coord.Y > 0 && coord.Z > 0);
            sector[2] = (coord.X < 0 && coord.Y > 0 && coord.Z < 0);
            sector[3] = (coord.X > 0 && coord.Y > 0 && coord.Z < 0);
            sector[4] = (coord.X > 0 && coord.Y < 0 && coord.Z > 0);
            sector[5] = (coord.X < 0 && coord.Y < 0 && coord.Z > 0);
            sector[6] = (coord.X < 0 && coord.Y < 0 && coord.Z < 0);
            sector[7] = (coord.X > 0 && coord.Y < 0 && coord.Z < 0);

            for (int i = 0; i < sector.Length; i++)
            {
                if (sector[i]) return i++;
            }
            return 0;
        }

        /// <summary>
        /// Gets neighboring nodes to a coordinate.
        /// </summary>
        /// <param name="node">Coordinate to check neighbors.</param>
        /// <param name="Weight">List with info on whether a node in passable.</param>
        /// <returns></returns>
        public static IEnumerable<Vector3> GetNeighborNodes(Vector3 node, byte[][,] Weight)
        {
            int sector, sectorF, sectorR, sectorB, sectorL;
            sector = CheckSector(node);
            var nodes = new List<Vector3>();

            sectorF = CheckSector(new Vector3(node.X, -8.2f, node.Z - 1));
            sectorR = CheckSector(new Vector3(node.X + 1, -8.2f, node.Z));
            sectorB = CheckSector(new Vector3(node.X, -8.2f, node.Z + 1));
            sectorL = CheckSector(new Vector3(node.X - 1, -8.2f, node.Z));

            // forward
            if (Weight[sectorF][Math.Abs((int)node.X), Math.Abs((int)node.Z - 1)] > 0) nodes.Add(new Vector3(node.X, -8.2f, node.Z - 1));

            // right
            if (Weight[sectorR][Math.Abs((int)node.X + 1), Math.Abs((int)node.Z)] > 0) nodes.Add(new Vector3(node.X + 1, -8.2f, node.Z));

            // backward
            if (Weight[sectorB][Math.Abs((int)node.X), Math.Abs((int)node.Z + 1)] > 0) nodes.Add(new Vector3(node.X, -8.2f, node.Z + 1));

            // left
            if (Weight[sectorL][Math.Abs((int)node.X - 1), Math.Abs((int)node.Z)] > 0) nodes.Add(new Vector3(node.X - 1, -8.2f, node.Z));

            return nodes;
        }
    }
}
