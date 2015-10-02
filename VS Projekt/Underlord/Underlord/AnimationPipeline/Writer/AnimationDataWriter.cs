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
    public class AnimationDataWriter : ContentTypeWriter<AnimationData>
    {
        protected override void Write(ContentWriter output, AnimationData data)
        {
            output.WriteObject(data.Skeleton);
            output.WriteObject(data.Clips);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AnimationDataReader).AssemblyQualifiedName;
        }
    }
}
