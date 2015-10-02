using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Animation
{
    public class ClipReader : ContentTypeReader<Clip>
    {
        protected override Clip Read(ContentReader input, Clip existingInstance)
        {
            Clip clip = new Clip();
            clip.Name = input.ReadString();
            clip.Duration = input.ReadDouble();

            int boneCnt = input.ReadInt32();
            for (int i = 0; i < boneCnt; i++)
            {
                Clip.Bone bone = new Clip.Bone();
                clip.Bones.Add(bone);

                bone.Name = input.ReadString();

                int cnt = input.ReadInt32();

                for (int j = 0; j < cnt; j++)
                {
                    Clip.Keyframe keyframe = new Clip.Keyframe();
                    keyframe.Time = input.ReadDouble();
                    keyframe.Rotation = input.ReadQuaternion();
                    keyframe.Translation = input.ReadVector3();

                    bone.Keyframes.Add(keyframe);

                }

            }

            return clip;
        }
    }
}
