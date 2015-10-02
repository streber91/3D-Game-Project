using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.ComponentModel;

using Animation;

namespace Underlord.Animation
{
    // Th clip player which maps the clip onto the character model
    public class ClipPlayer
    {
        #region Fields

        private float speed = 1f; 
        private float position = 0;
        private Clip clip = null;
        private PlayerLogic[] boneInfos;
        private int boneCnt;

        private CharacterModel model = null;

        private bool looping = false;

        #endregion

        #region Properties

        public float Speed
        {
            set{ speed = value; }
        }

        public float Position
        {
            get { return position; }
            set
            {
                if (value > Duration)
                    value = Duration;

                position = value;
                foreach (PlayerLogic bone in boneInfos)
                {
                    bone.SetPosition(position);
                }
            }
        }

        public Clip Clip { get { return clip; } }

        public float Duration { get { return (float)clip.Duration; } }

        public CharacterModel Model { get { return model; } }

        public bool Looping { get { return looping; } set { looping = value; } }

        #endregion

        #region Construction

        public ClipPlayer(Clip clip, CharacterModel model)
        {
            this.clip = clip;
            this.model = model;

            // Create the bone information classes
            boneCnt = clip.Bones.Count;
            boneInfos = new PlayerLogic[boneCnt];

            for (int b = 0; b < boneInfos.Length; b++)
            {
                // Create it
                boneInfos[b] = new PlayerLogic(clip.Bones[b]);

                // Assign it to a model bone
                boneInfos[b].SetModel(model);
            }

            Position = 0;
        }

        #endregion

        #region Update 

        public void Update(GameTime gameTime)
        {
            Position = Position + (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            if (looping && Position >= Duration)
                Position = 0;
        }

        #endregion

        #region PlayerLogic class

        private class PlayerLogic
        {
            #region Fields

            private int currentKeyframe = 0;
            private Bone assignedBone = null;
            public bool valid = false;
            private Quaternion rotation;
            public Vector3 translation;
            public Clip.Keyframe Keyframe1;
            public Clip.Keyframe Keyframe2;

            #endregion

            #region Properties

            public Clip.Bone ClipBone { get; set; }
            public Bone ModelBone { get { return assignedBone; } }

            #endregion

            #region Constructor

            public PlayerLogic(Clip.Bone bone)
            {
                this.ClipBone = bone;
                SetKeyframes();
                SetPosition(0);
            }

            #endregion

            #region Position

            public void SetPosition(float position)
            {
                List<Clip.Keyframe> keyframes = ClipBone.Keyframes;
                if (keyframes.Count == 0)
                    return;

                while (position < Keyframe1.Time && currentKeyframe > 0)
                {
                    // We need to move backwards in time
                    currentKeyframe--;
                    SetKeyframes();
                }

                while (position >= Keyframe2.Time && currentKeyframe < ClipBone.Keyframes.Count - 2)
                {
                    // We need to move forwards in time
                    currentKeyframe++;
                    SetKeyframes();
                }

                if (Keyframe1 == Keyframe2)
                {
                    rotation = Keyframe1.Rotation;
                    translation = Keyframe1.Translation;
                }
                else
                {
                    // Interpolate between keyframes
                    float t = (float)((position - Keyframe1.Time) / (Keyframe2.Time - Keyframe1.Time));
                    rotation = Quaternion.Slerp(Keyframe1.Rotation, Keyframe2.Rotation, t);
                    translation = Vector3.Lerp(Keyframe1.Translation, Keyframe2.Translation, t);
                }

                valid = true;
                if (assignedBone != null)
                {
                    // Send to the model
                    // Make it a matrix first
                    Matrix m = Matrix.CreateFromQuaternion(rotation);
                    m.Translation = translation;
                    assignedBone.SetCompleteTransform(m);
                }
            }
            #endregion

            #region Keyframe

            private void SetKeyframes()
            {
                if (ClipBone.Keyframes.Count > 0)
                {
                    Keyframe1 = ClipBone.Keyframes[currentKeyframe];
                    if (currentKeyframe == ClipBone.Keyframes.Count - 1)
                        Keyframe2 = Keyframe1;
                    else
                        Keyframe2 = ClipBone.Keyframes[currentKeyframe + 1];
                }
                else
                {
                    // If there are no keyframes, set both to null
                    Keyframe1 = null;
                    Keyframe2 = null;
                }
            }
            #endregion
            
            #region Model

            public void SetModel(CharacterModel model)
            {
                // Find this bone
                assignedBone = model.FindBone(ClipBone.Name);
            }

            #endregion
        }

        #endregion

    }
}
