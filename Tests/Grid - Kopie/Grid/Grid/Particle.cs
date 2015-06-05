using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Grid
{
    class Particle
    {
        #region Fields
        private Model model;
        private GraphicsDevice device;
        private Vector3 position;
        private Vector3 speed;
        private Vector3 rotation;
        private Vector3 rotationSpeed;

        private float scale = 0.02f;
        private Matrix[] boneTransforms;

        private float zOffset = 0;
        private float lifeTime;
        private float diffTime;

        private ParticleGenerator generator;
        private int generatorID;
        #endregion

        #region Properties
        public Model Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
            }
        }
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public Vector3 Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }
        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                this.rotation.X = MathHelper.WrapAngle(value.X);
                this.rotation.Y = MathHelper.WrapAngle(value.Y);
                this.rotation.Y = MathHelper.WrapAngle(value.Z);
            }
        }
        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }
        public float LifeTime
        {
            get
            {
                return lifeTime;
            }
            set
            {
                lifeTime = value;
            }
        }
        public float OffSetZ
        {
            get
            {
                return zOffset;
            }
        }
        #endregion

         #region Constructor
        public Particle(GraphicsDevice device, Model model, Vector3 position, Vector3 rotation, Vector3 speed, Vector3 rotationSpeed, float lifeTime, float scale, float zOffSet, ParticleGenerator generator, int id)
        {
            this.device = device;
            this.model = model;
            this.zOffset = zOffset;
            this.position =  new Vector3(position.X, position.Y, position.Z + this.zOffset);
    
            this.speed = speed;

            this.rotation = rotation;
            this.rotationSpeed = rotationSpeed;

            this.lifeTime = lifeTime;
            this.diffTime = lifeTime;
            this.scale = scale;

            this.generator = generator;
            this.generatorID = id;

            boneTransforms = new Matrix[model.Bones.Count];
        }
        #endregion

        public void update(float time)
        {
            this.lifeTime -= time;

            if (this.lifeTime <= 0.1)
                this.delete();
        }


        public void delete()
        {
            this.generator.deleteParticle(this.generatorID);
        }

        #region Draw
        public void Draw(Camera camera)
        {
            model.Root.Transform = Matrix.Identity * 
            Matrix.CreateScale(scale) *
            Matrix.CreateRotationX(this.rotation.X) * 
            Matrix.CreateRotationY(this.rotation.Y) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(this.position);
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            float alpha = MathHelper.Clamp(((this.lifeTime) / this.diffTime), 0, 1);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {

                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                    basicEffect.Alpha = alpha;
                    basicEffect.EnableDefaultLighting();
                    basicEffect.World = boneTransforms[mesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;
                    
                }
                mesh.Draw();
            }
        }
        #endregion

        #region Move
        public void Move(float elapsed, float turpoSpeed)
        {
            this.position += speed * elapsed * turpoSpeed;
        }
        #endregion
    }
}
