using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Animation;

namespace Pipeline
{
    [ContentTypeWriter]
    public class ClipWriter : ContentTypeWriter<Clip>
    {
        protected override void Write(ContentWriter output, Clip clip)
        {
            output.Write(clip.Name);
            output.Write(clip.Duration);
            output.Write(clip.Bones.Count);
            foreach (Clip.Bone bone in clip.Bones)
            {
                output.Write(bone.Name);
                output.Write(bone.Keyframes.Count);
                foreach (Clip.Keyframe keyframe in bone.Keyframes)
                {
                    output.Write(keyframe.Time);
                    output.Write(keyframe.Rotation);
                    output.Write(keyframe.Translation);
                }
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(ClipReader).AssemblyQualifiedName;
        }
    }
}
