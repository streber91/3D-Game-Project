﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Animation;
using Underlord.Renderer;
using Underlord.Animation;

namespace Underlord.Basic
{
    public class BasicModel
    {
        #region Fields

        protected string assetName = "";
        protected Model model = null;
        protected Texture2D modelTexture;
        protected Color modelColor;

        #endregion

        #region Properties

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

        #region Construction 
        public BasicModel(Model model)
        {
            this.model = model;
        }        
        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            /// TODO: Update something.

        }

        #endregion

        #region Draw

        public void Draw(Camera camera, Matrix world, bool drawAmbient, bool isEnlightend, float lightPower)
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
                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                    this.SetBasicEffects(basicEffect, drawAmbient, isEnlightend, lightPower);

                    if (this.modelTexture != null && this.modelTexture != basicEffect.Texture)
                    {
                        basicEffect.Texture = this.modelTexture;
                    }
                }
                modelMesh.Draw();
            }
        }

        public void DrawTexture(Camera camera, Matrix world, Texture2D texture, bool drawAmbient, bool isEnlightend, float lightPower)
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
                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                    this.SetBasicEffects(basicEffect, drawAmbient, isEnlightend, lightPower);

                    if (this.modelTexture != null && this.modelTexture != basicEffect.Texture)
                    {
                        basicEffect.Texture = this.modelTexture;
                    }
                    if (texture != null)
                    {
                        basicEffect.Texture = texture;
                    }
                }
                modelMesh.Draw();
            }
        }

        protected void SetBasicEffects(BasicEffect basicEffect, bool drawAmbient, bool isEnlightend, float lightPower)
        {
            basicEffect.EnableDefaultLighting();
            if (drawAmbient)
            {
                basicEffect.PreferPerPixelLighting = false;
                basicEffect.AmbientLightColor = new Vector3(modelColor.R, modelColor.G, modelColor.B);
                basicEffect.EmissiveColor = new Vector3(0, 0, 0);
                basicEffect.SpecularColor = new Vector3(0, 0, 0);
                basicEffect.SpecularPower = 0;
            }
            else
            {
                basicEffect.PreferPerPixelLighting = true;
                if (isEnlightend)
                {
                    basicEffect.EmissiveColor = new Vector3(lightPower, lightPower, lightPower);
                    basicEffect.SpecularColor = new Vector3(lightPower, lightPower, lightPower);
                    basicEffect.SpecularPower = lightPower;
                }
                else
                {
                    basicEffect.EmissiveColor = new Vector3(0, 0, 0);
                    basicEffect.SpecularColor = new Vector3(0, 0, 0);
                    basicEffect.SpecularPower = 0;
                }
                basicEffect.DiffuseColor = new Vector3(0.7f, 0.4f, 0.1f);
                float brightness = (float)(Logic.Setting_GUI.getBrigthness())/10;
                basicEffect.AmbientLightColor = new Vector3(brightness, brightness, brightness);
                //basicEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                basicEffect.LightingEnabled = true;
                basicEffect.DirectionalLight0.Enabled = true;
                basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.4f, 0.1f, 0.2f);
                basicEffect.DirectionalLight0.Direction = new Vector3(1, 0, 1);
            }
        }
        #endregion
    }
}
