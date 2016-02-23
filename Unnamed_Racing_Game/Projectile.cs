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
    public enum ProjectileType { Red, Blue, Green }

    class Projectile
    {
        BasicEffect effect;
        public bool broken;
        float frameTime, velocity, angle;
        public int worth;
        Level level;
        Matrix origin, world, view, projection;
        Model model;
        ProjectileType type;
        Vector3 pos, startLocation, forward;

        public Model Model
        {
            get { return model; }
        }

        public Matrix World
        {
            get { return world; }
        }

        public Projectile(ProjectileType type, Model sphere, Vector3 position, Vector3 forward, float angle, Level host)
        {
            this.type = type;
            level = host;
            origin = Matrix.Translation(startLocation);
            this.forward = forward;
            pos = position;
            startLocation = position;
            model = sphere;
            effect = new BasicEffect(Main.Graphics.GraphicsDevice);
            if (this.type == ProjectileType.Red)
            {
                effect.LightingEnabled = true;
                effect.DirectionalLight0.DiffuseColor = Color.Red.ToVector3();
                effect.DirectionalLight0.Direction = new Vector3(1, 0, -1);
                effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);
                worth = 10;
                velocity = 100;
            }
            else if (this.type == ProjectileType.Green)
            {
                effect.LightingEnabled = true;
                effect.DirectionalLight0.DiffuseColor = Color.Green.ToVector3();
                effect.DirectionalLight0.Direction = new Vector3(1, 0, -1);
                effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);
                worth = 25;
                velocity = 120;
            }
            else
            {
                effect.LightingEnabled = true;
                effect.DirectionalLight0.DiffuseColor = Color.Blue.ToVector3();
                effect.DirectionalLight0.Direction = new Vector3(1, 0, -1);
                effect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0);
                worth = 100;
                velocity = 150;
            }
        }

        public void Update(GameTime gameTime, Matrix eye, Matrix proj)
        {
            view = eye;
            projection = proj;
            frameTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;

            world = Matrix.Translation(pos);

            broken = Vector3.Distance(pos, level.AI.position) < 5;

            origin = Matrix.RotationY((float)Math.Atan2(level.AI.position.X - pos.X, level.AI.position.Z - pos.Z));

            pos -= origin.Forward * (velocity * frameTime);
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            model.Draw(graphicsDevice, world, view, projection, effect);
        }
    }
}
