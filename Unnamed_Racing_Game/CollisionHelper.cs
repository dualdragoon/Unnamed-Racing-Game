using SharpDX;
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
            BoundingSphere bs1 = CreateBoundingSphere(kart.Model, kart.World);
            //BoundingSphere bs2 = CreateBoundingSphere(level.WorldModel, level.translation);

            for (int i = 0; i < level.WorldModel.Meshes.Count; i++)
            {
                if (bs1.Intersects(TransformBoundingSphere(level.translation, level.WorldModel.Meshes[i].BoundingSphere)))
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

        private static BoundingSphere CreateBoundingSphere(Model model, Matrix world)
        {
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            BoundingSphere meshSphere, boundingSphere = new BoundingSphere();

            for (int i = 0; i < model.Meshes.Count; i++)
            {
                meshSphere = TransformBoundingSphere(boneTransforms[i], model.Meshes[i].BoundingSphere);
                boundingSphere = BoundingSphere.Merge(boundingSphere, meshSphere);
            }
            return TransformBoundingSphere(world, boundingSphere);
        }
    }
}