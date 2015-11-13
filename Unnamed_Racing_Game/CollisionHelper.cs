using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Kross_Kart
{
    static class CollisionHelper
    {
        public static bool IsCollision(KartEntity kart1, KartEntity kart2)
        {
            BoundingSphere sphere1 = kart1.Model.CalculateBounds(kart1.World), sphere2 = kart2.Model.CalculateBounds(kart2.World);

            return (Collision.SphereIntersectsSphere(ref sphere1, ref sphere2));
        }

        public static bool IsCollision(KartEntity kart, Level level)
        {
            BoundingSphere sphere1 = kart.Model.CalculateBounds(kart.World), sphere2;
            for (int i = 0; i < level.model.Meshes.Count; i++)
            {
                sphere2 = TransformBoundingSphere(level.translation, level.model.Meshes[i].BoundingSphere);
                if (Collision.SphereIntersectsSphere(ref sphere1, ref sphere2))
                {
                    return true;
                }
            }
            //Console.WriteLine(sphere2.Center.ToString() + "//" + sphere2.Radius.ToString());
            return false;
        }

        private static BoundingSphere TransformBoundingSphere(Matrix m, BoundingSphere b)
        {
            var worldCenter = Vector3.Transform(b.Center, m);

            return new BoundingSphere(new Vector3(worldCenter.X, worldCenter.Y, worldCenter.Z), b.Radius);
        }
    }
}
