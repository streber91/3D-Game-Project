using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Underlord.Renderer;
using Underlord.Animation;
using Underlord.Logic;

namespace Underlord.Basic
{
    class FireBallModel : BasicModel
    {
        float speed, zPosition, alpha, scale, rotation;
        float timeCounter;
        float rotationSpeed1, rotationSpeed2, rotationSpeed3;
        float position1, position2, position3;
        bool releaseFire = false;
        Random randomValue;

        List<FireBallModel> fires = new List<FireBallModel>();

        #region Properties
        public float Z
        {
            get { return zPosition; }
            set { zPosition = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public float RotationZ
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public List<FireBallModel> Fires
        {
            get { return fires; }
        }

        public bool ReleaseFire
        {
            set { releaseFire = value; }
        }

        public void resetFireBall()
        {
            speed = 0.6f;
            zPosition = 0;
            alpha = 1;
            scale = 1;
            timeCounter = 0;
            randomValue = new Random();
        }
        #endregion

        #region Constructor
        public FireBallModel(Model model)
            : base(model)
        {
            speed = 0.6f;
            zPosition = 0;
            alpha = 1;
            scale = 1;
            timeCounter = 0;
            randomValue = new Random();
        }
        #endregion

        #region Update
        /// <summary>
        /// Update the model.
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        public void Update(GameTime gameTime)
        {
            if (releaseFire)
            {
                timeCounter += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

                zPosition = MathHelper.Lerp(5f, 0f, timeCounter * speed);
                float value = (float)(0.5f * Math.Cos(timeCounter / 1000 * speed + MathHelper.PiOver2)) + 0.5f;
                scale = 1 - (value / 2);
                rotation += (timeCounter / 500) * 10;

                position1 += (timeCounter / 1000) * 8;
                fires[0].Z = position1;
                fires[0].Scale = MathHelper.Lerp(1f, 0f, MathHelper.Clamp(position1 / 10, 0, 1));

                position2 += (timeCounter / 1000) * 6;
                fires[1].Z = position2;
                fires[1].Scale = MathHelper.Lerp(1f, 0f, MathHelper.Clamp(position1 / 12, 0, 1));

                position3 += (timeCounter / 1000) * 4;
                fires[2].Z = position3;
                fires[2].Scale = MathHelper.Lerp(1f, 0f, MathHelper.Clamp(position1 / 15, 0, 1));
            }

        }
        #endregion

        #region Drawing
        /// <summary>
        /// Draw the model
        /// </summary>
        /// <param name="camera">A camera to determine the view</param>
        /// <param name="world">A world matrix to place the model</param>
        public void Draw(Camera camera, Matrix world)
        {
            if (model == null)
                return;
            //
            // Compute all of the bone absolute transforms
            //
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.Root.Transform = world;
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            // Draw the model.
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (BasicEffect basicEffect in modelMesh.Effects)
                {
                    basicEffect.World = boneTransforms[modelMesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;
                    basicEffect.Alpha = alpha;

                    basicEffect.EmissiveColor = new Vector3(1, 1, 1);
                    basicEffect.SpecularColor = new Vector3(1, 1, 1);
                    basicEffect.SpecularPower = 0.8f;
                    basicEffect.DiffuseColor = new Vector3(0.7f, 0.4f, 0.1f);
                    basicEffect.AmbientLightColor = new Vector3(1, 1, 1);
                    basicEffect.LightingEnabled = true;
                    basicEffect.DirectionalLight0.Enabled = true;
                    basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.4f, 0.1f, 0.2f);
                    basicEffect.DirectionalLight0.Direction = new Vector3(1, 0, 1);
                }
                modelMesh.Draw();
            }

            foreach (FireBallModel f in fires)
            {
                f.Draw(camera, world);
            }
        }
        #endregion
    }
}
