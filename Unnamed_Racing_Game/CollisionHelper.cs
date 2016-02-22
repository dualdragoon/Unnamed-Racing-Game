using System;
using System.Xml;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Kross_Kart
{
    static class CollisionHelper
    {
        /// <summary>
        /// Moves a BoundingSphere by a given world matrix.
        /// </summary>
        /// <param name="m">Matrix to translate BoundingSphere by.</param>
        /// <param name="b">BoundingSphere to translate.</param>
        /// <returns></returns>
        public static BoundingSphere TransformBoundingSphere(Matrix m, BoundingSphere b)
        {
            var worldCenter = Vector3.Transform(b.Center, m);

            return new BoundingSphere(new Vector3(worldCenter.X, worldCenter.Y, worldCenter.Z), b.Radius);
        }

        /// <summary>
        /// Creates a BoundingSphere from a given model and world matrix.
        /// </summary>
        /// <param name="model">Model to create BoundingSphere from.</param>
        /// <param name="world">Matrix to place BoundingSphere with.</param>
        /// <returns></returns>
        public static BoundingSphere CreateBoundingSphere(Model model, Matrix world)
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

        /// <summary>
        /// Creates a BoundingSphere from a given ModelMesh.
        /// </summary>
        /// <param name="mesh">ModelMesh to create BoundingSphere from.</param>
        /// <param name="roomNum">0-based room number.</param>
        /// <param name="modelPos">Position of Model in world space.</param>
        /// <returns></returns>
        public static BoundingSphere CreateBoundingSphere(ModelMesh mesh, string roomNum, Vector3 modelPos)
        {
            BoundingSphere b;
            XmlDocument read = new XmlDocument();
            XmlNode l;
            Matrix world;
            Vector3 pos;
            /*Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);*/

            read.Load(string.Format("Content/Models/Rooms/XML/{0}.xml", roomNum));
            l = read.SelectSingleNode(string.Format("/Meshes/{0}", mesh.Name));

            pos = new Vector3(float.Parse(l.Attributes["X"].Value),
                float.Parse(l.Attributes["Y"].Value),
                float.Parse(l.Attributes["Z"].Value));

            pos += new Vector3(modelPos.X, 0, modelPos.Z);

            world = Matrix.Translation(pos);

            b = new BoundingSphere(pos, float.Parse(l.Attributes["Radius"].Value));

            /*for (int i = 0; i < model.Meshes.Count; i++)
            {
                meshSphere = TransformBoundingSphere(boneTransforms[i], model.Meshes[i].BoundingSphere);
                boundingSphere = BoundingSphere.Merge(boundingSphere, meshSphere);
            }*/
            return TransformBoundingSphere(world, mesh.BoundingSphere);
        }
    }
}