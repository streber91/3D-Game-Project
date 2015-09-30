using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using AnimationAux;
using Underlord.Basic;
using Underlord.Renderer;

namespace Underlord.Animation
{
    public class AnimationModel : BasicModel
    {
        #region Fields
        /// <summary>
        /// Extra data associated with the XNA model
        /// </summary>
        protected ModelExtra modelExtra = null;

        /// <summary>
        /// The model bones
        /// </summary>
        protected List<Bone> bones = new List<Bone>();

        /// <summary>
        /// An associated animation clip player
        /// </summary>
        protected AnimationPlayer player = null;

        /// <summary>
        /// 
        /// </summary>
        protected List<AnimationClip> clips = new List<AnimationClip>();
        #endregion

        #region Properties
        /// <summary>
        /// The underlying bones for the model
        /// </summary>
        public List<Bone> Bones { get { return bones; } }

        /// <summary>
        /// The model animation clips
        /// </summary>
        public List<AnimationClip> Clips { get { return modelExtra.Clips; } }

        /// <summary>
        /// 
        /// </summary>
        public Model Model { get { return model; } }

        /// <summary>
        /// 
        /// </summary>
        public List<AnimationClip> AnimationClip { set { clips = value; } get { return clips; } }
        #endregion

        #region Construction
        /// <summary>
        /// Constructor. Creates the model from an XNA model
        /// </summary>
        /// <param name="assetName">The name of the asset for this model</param>
        public AnimationModel(Model model) : base(model) 
        {
            modelExtra = model.Tag as ModelExtra;
            System.Diagnostics.Debug.Assert(modelExtra != null);
            ObtainBones();

            this.clips.Add(Clips[0]);
        }

        /// <summary>
        /// Constructor. Creates the model from an XNA model
        /// </summary>
        /// <param name="assetName">The name of the asset for this model</param>
        public AnimationModel(Model model, List<AnimationClip> clips) : this(model)
        {
            this.clips = clips;
        }
        #endregion

        #region Bones Management

        /// <summary>
        /// Get the bones from the model and create a bone class object for
        /// each bone. We use our bone class to do the real animated bone work.
        /// </summary>
        private void ObtainBones()
        {
            bones.Clear();
            foreach (ModelBone bone in model.Bones)
            {
                // Create the bone object and add to the heirarchy
                Bone newBone = new Bone(bone.Name, bone.Transform, bone.Parent != null ? bones[bone.Parent.Index] : null);

                // Add to the bones for this model
                bones.Add(newBone);
            }
        }

        /// <summary>
        /// Find a bone in this model by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Bone FindBone(string name)
        {
            foreach (Bone bone in Bones)
            {
                if (bone.Name == name)
                    return bone;
            }

            return null;
        }

        #endregion

        #region Animation Management
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void AddClip(AnimationModel model)
        {
            this.clips.Add(model.Clips[0]);
        }

        /// <summary>
        /// Play an animation clip
        /// </summary>
        /// <param name="clip">The clip to play</param>
        /// <returns>The player that will play this clip</returns>
        public AnimationPlayer PlayClip(AnimationClip clip, float speed)
        {
            // Create a clip player and assign it to this model
            player = new AnimationPlayer(clip, this);
            player.Speed = speed;
            return player;
        }
        #endregion
        #region Updating

        /// <summary>
        /// Update animation for the model.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (player != null)
            {
                player.Update(gameTime);
            }
        }
        #endregion

        #region Drawing
        /// <summary>
        /// Draw the model
        /// </summary>
        /// <param name="graphics">The graphics device to draw on</param>
        /// <param name="camera">A camera to determine the view</param>
        /// <param name="world">A world matrix to place the model</param>
        public void Draw(Camera camera, Matrix world, bool drawAmbient, bool isEnlightend, float lightPower)
        {
            if (model == null)
                return;
            //
            // Compute all of the bone absolute transforms
            //
            Matrix[] boneTransforms = new Matrix[bones.Count];
            for (int i = 0; i < bones.Count; i++)
            {
                Bone bone = bones[i];
                bone.ComputeAbsoluteTransform();

                boneTransforms[i] = bone.AbsoluteTransform;
            }
            //
            // Determine the skin transforms from the skeleton
            //
            Matrix[] skeleton = new Matrix[modelExtra.Skeleton.Count];
            for (int s = 0; s < modelExtra.Skeleton.Count; s++)
            {
                Bone bone = bones[modelExtra.Skeleton[s]];
                skeleton[s] = bone.SkinTransform * bone.AbsoluteTransform;
            }
            // Draw the model.
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (Effect effect in modelMesh.Effects)
                {
                    if (effect is BasicEffect)
                    {
                        BasicEffect basicEffect = effect as BasicEffect;
                        basicEffect.World = boneTransforms[modelMesh.ParentBone.Index] * world;
                        basicEffect.View = camera.View;
                        basicEffect.Projection = camera.Projection;
                        basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                        basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                        this.SetBasicEffects(basicEffect, drawAmbient, isEnlightend, lightPower);       
                        if (this.modelTexture != null)
                        {
                            basicEffect.Texture = this.modelTexture;
                        }
                    }

                    if (effect is SkinnedEffect)
                    {
                        SkinnedEffect skinndedEffect = effect as SkinnedEffect;
                        skinndedEffect.World = boneTransforms[modelMesh.ParentBone.Index] * world;
                        skinndedEffect.View = camera.View;
                        skinndedEffect.Projection = camera.Projection;
                        skinndedEffect.EnableDefaultLighting();
                        skinndedEffect.PreferPerPixelLighting = true;
                        skinndedEffect.SetBoneTransforms(skeleton);
                    }
                }
                modelMesh.Draw();
            }
        }
        //public void Draw(Camera camera, Matrix world)
        //{
        //    if (model == null)
        //        return;

        //    //
        //    // Compute all of the bone absolute transforms
        //    //

        //    Matrix[] boneTransforms = new Matrix[bones.Count];

        //    for (int i = 0; i < bones.Count; i++)
        //    {
        //        Bone bone = bones[i];
        //        bone.ComputeAbsoluteTransform();

        //        boneTransforms[i] = bone.AbsoluteTransform;
        //    }

        //    //
        //    // Determine the skin transforms from the skeleton
        //    //

        //    Matrix[] skeleton = new Matrix[modelExtra.Skeleton.Count];
        //    for (int s = 0; s < modelExtra.Skeleton.Count; s++)
        //    {
        //        Bone bone = bones[modelExtra.Skeleton[s]];
        //        skeleton[s] = bone.SkinTransform * bone.AbsoluteTransform;
        //    }

        //    // Draw the model.
        //    foreach (ModelMesh modelMesh in model.Meshes)
        //    {
        //        foreach (Effect effect in modelMesh.Effects)
        //        {
        //            if (effect is BasicEffect)
        //            {
        //                BasicEffect beffect = effect as BasicEffect;
        //                beffect.World = boneTransforms[modelMesh.ParentBone.Index] * world;
        //                beffect.View = camera.View;
        //                beffect.Projection = camera.Projection;
        //                beffect.EnableDefaultLighting();
        //                beffect.PreferPerPixelLighting = true;

        //                if (this.modelTexture != null)
        //                {
        //                    beffect.Texture = this.modelTexture;
        //                }

        //            }

        //            if (effect is SkinnedEffect)
        //            {
        //                SkinnedEffect seffect = effect as SkinnedEffect;
        //                seffect.World = boneTransforms[modelMesh.ParentBone.Index] * world;
        //                seffect.View = camera.View;
        //                seffect.Projection = camera.Projection;
        //                seffect.EnableDefaultLighting();
        //                seffect.PreferPerPixelLighting = true;
        //                seffect.SetBoneTransforms(skeleton);
        //            }
        //        }
        //        modelMesh.Draw();
        //    }
        //}
       #endregion
    }
}
