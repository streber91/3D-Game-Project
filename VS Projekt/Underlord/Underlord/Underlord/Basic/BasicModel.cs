using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using AnimationAux;
using Underlord.Renderer;
using Underlord.Animation;

namespace Underlord.Basic
{
    public class BasicModel
    {
        #region Fields

        /// <summary>
        /// The model asset name.
        /// </summary>
        protected string assetName = "";

        /// <summary>
        /// The actual underlying XNA model.
        /// </summary>
        protected Model model = null;

        /// <summary>
        /// The texture of the model.
        /// </summary>
        protected Texture2D modelTexture;

        /// <summary>
        /// The color of the model.
        /// </summary>
        protected Color modelColor;

        #endregion

        #region Properties

        /// <summary>
        /// The model property.
        /// </summary>
        public Model Model
        {
            get { return this.model; }
        }

        public Texture2D Texture
        {
            set { this.modelTexture = value; }
            get { return modelTexture; }
        }

        public Color Color
        {
            set { this.modelColor = value; }
        }

        #endregion

        #region Construction and Loading

        public BasicModel(Model model)
        {
            this.model = model;
        }
        
        #endregion

        #region Updating

        /// <summary>
        /// Update the model.
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        public void Update(GameTime gameTime)
        {
            /// TODO: Update something.

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

            //
            // Determine the skin transforms from the skeleton
            //

            // Draw the model.
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (BasicEffect basicEffect in modelMesh.Effects)
                {
                    basicEffect.World = boneTransforms[modelMesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;
                    
                    basicEffect.EnableDefaultLighting();
                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                    basicEffect.AmbientLightColor = new Vector3(modelColor.R, modelColor.G, modelColor.B);

                    if (this.modelTexture != basicEffect.Texture)
                    {
                        basicEffect.Texture = this.modelTexture;
                    }
                }
                modelMesh.Draw();
            }
        }

        #endregion
    }
}
