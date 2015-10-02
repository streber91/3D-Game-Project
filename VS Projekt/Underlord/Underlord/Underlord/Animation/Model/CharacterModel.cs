using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Animation;
using Underlord.Basic;
using Underlord.Renderer;

namespace Underlord.Animation
{
    public class CharacterModel : BasicModel
    {
        #region Fields

        protected AnimationData modelExtra = null;
        protected List<Bone> bones = new List<Bone>();
        protected ClipPlayer player = null;
        protected List<Clip> clips = new List<Clip>();

        #endregion

        #region Properties

        public List<Bone> Bones { get { return bones; } }
        public List<Clip> Clips { get { return modelExtra.Clips; } }
        public Model Model { get { return model; } }
        public List<Clip> AnimationClip { set { clips = value; } get { return clips; } }

        #endregion

        #region Construction

        public CharacterModel(Model model) : base(model) 
        {
            modelExtra = model.Tag as AnimationData;
            System.Diagnostics.Debug.Assert(modelExtra != null);
            ObtainBones();

            this.clips.Add(Clips[0]);
        }

        public CharacterModel(Model model, List<Clip> clips)
            : this(model)
        {
            this.clips = clips;
        }
        #endregion

        #region Bones Management

        private void ObtainBones()
        {
            bones.Clear();
            foreach (ModelBone bone in model.Bones)
            {
                // Create the bone object and add to the heirarchy
                Bone newBone = new Bone(bone.Name, bone.Transform, bone.Parent != null ? bones[bone.Parent.Index] : null);
                bones.Add(newBone);
            }
        }

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

        public void AddClip(CharacterModel model)
        {
            this.clips.Add(model.Clips[0]);
        }

        public ClipPlayer PlayClip(Clip clip, float speed)
        {
            // Create a clip player and assign it to this model
            player = new ClipPlayer(clip, this);
            player.Speed = speed;
            return player;
        }
        #endregion
       
        #region Update

        public void Update(GameTime gameTime)
        {
            if (player != null)
            {
                player.Update(gameTime);
            }
        }
        #endregion

        #region Drawing

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

       #endregion
    }
}
