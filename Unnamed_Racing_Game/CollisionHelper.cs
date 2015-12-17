using SharpDX;

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
            for (int i = 0; i < level.WorldModel.Meshes.Count; i++)
            {
                sphere2 = TransformBoundingSphere(level.translation, level.WorldModel.Meshes[i].BoundingSphere);
                if (Collision.SphereIntersectsSphere(ref sphere1, ref sphere2))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Moves a BoundingSphere by a given world matrix.
        /// </summary>
        /// <param name="m">Matrix to translate BoundingSphere by.</param>
        /// <param name="b">BoundingSphere to translate.</param>
        /// <returns></returns>
        private static BoundingSphere TransformBoundingSphere(Matrix m, BoundingSphere b)
        {
            var worldCenter = Vector3.Transform(b.Center, m);

            return new BoundingSphere(new Vector3(worldCenter.X, worldCenter.Y, worldCenter.Z), b.Radius);
        }
    }
}