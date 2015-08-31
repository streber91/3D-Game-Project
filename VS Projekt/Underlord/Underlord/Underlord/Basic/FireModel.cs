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
    class FireModel : BasicModel
    {
        float speed;
        float timeCounter;
        float zPosition;
        float alpha;
        Random randomValue;

        #region Properties
        public float Z { get { return zPosition; } }
        #endregion

        #region Constructor
        public FireModel(Model model, float speed)
            : base(model)
        {
            this.speed = speed;
            zPosition = 0;
            alpha = 1;
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

            //float value = (float)(0.5f * Math.Cos(timeCounter / 1000 * speed + MathHelper.PiOver2)) + 0.5f;
            //alpha = 0.8f - (value / 2f); ;
            //zPosition = value;

            alpha -= 4 * (timeCounter / 1000) * speed;
            zPosition += 0.3f * (timeCounter / 1000) * speed;
            if (alpha < 0)
            {
                zPosition = 0;
                alpha = 1;
                timeCounter = 0;
                float randomBonus = randomValue.Next(0, 100);
                speed = 0.01f + randomBonus / 3000;
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
                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    basicEffect.World = boneTransforms[modelMesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;
                    basicEffect.Alpha = alpha;
                }
                modelMesh.Draw();
            }
        }
        #endregion
    }
}
