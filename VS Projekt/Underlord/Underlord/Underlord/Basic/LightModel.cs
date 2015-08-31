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
    class LightModel : BasicModel
    {
        float timeCounter;
        float scale;
        float zPosition;
        float zRotation;
        float alpha;
        Random randomValue;

        #region Properties
        public float Scale { get { return scale; } }
        public float PositionZ { get { return zPosition; } }
        public float RotationZ { get { return zRotation; } }
        #endregion

        #region Constructor
        public LightModel(Model model)
            : base(model)
        {
            scale = 1f;
            alpha = 0.6f;
            zPosition = 0.3f;
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
            timeCounter += (float)gameTime.ElapsedGameTime.Milliseconds;

            float value = (float)(0.5f * Math.Cos(timeCounter / 2000 + MathHelper.PiOver2)) + 0.5f;
            zRotation = timeCounter / 10000;
            alpha = 0.7f - (value / 4f); ;
            //scale = 2.5f - value;
            //zPosition = 1f -(value/4f);

            //alpha -= 4*(timeCounter / 1000) * speed;
            //zPosition += 0.3f*(timeCounter / 1000) * speed;
            //if (alpha < 0)
            //{
            //    zPosition = 0;
            //    alpha = 1;
            //    timeCounter = 0;
            //    float randomBonus = randomValue.Next(0, 100);
            //    speed = 0.01f + randomBonus/3000;
            //}
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
                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    basicEffect.World = boneTransforms[modelMesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;
                    basicEffect.Alpha = alpha;

                    basicEffect.EmissiveColor = new Vector3(0.5f, 0.5f, 0.5f);
                    basicEffect.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);
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
        }
        #endregion
    }
}
