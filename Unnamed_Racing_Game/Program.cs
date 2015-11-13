using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;

namespace Kross_Kart
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Kross Kart Debug";

            using (Main game = new Main())
            {
                game.Run();
            }
        }
    }
}
