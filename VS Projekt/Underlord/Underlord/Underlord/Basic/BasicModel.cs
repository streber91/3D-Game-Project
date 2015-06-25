﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Underlord.Renderer;

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
        protected Texture2D modelTexture = null;

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

        /// <summary>
        /// Constructor. Creates the model from an XNA model.
        /// </summary>
        /// <param name="assetName">The name of the asset for this model</param>
        public BasicModel(string assetName)
        {
            this.assetName = assetName;

        }

        /// <summary>
        /// Load the model asset from content.
        /// </summary>
        /// <param name="content">The basic ContentManager</param>
        public void LoadContent(ContentManager content)
        {
            this.model = content.Load<Model>(assetName);
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

            Matrix[] boneTransforms = new Matrix[this.model.Bones.Count];

            // Draw the model.
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (BasicEffect basicEffect in modelMesh.Effects)
                {
                    basicEffect.World = boneTransforms[modelMesh.ParentBone.Index] * world;


                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;

                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    basicEffect.EnableDefaultLighting();

                    basicEffect.PreferPerPixelLighting = true;

                    if (this.modelTexture != null)
                    {
                        basicEffect.Texture = this.modelTexture;
                    }

                    basicEffect.AmbientLightColor = new Vector3(this.modelColor.R, this.modelColor.G, this.modelColor.B);
                }
                modelMesh.Draw();
            }
        }

        #endregion
    }
}
