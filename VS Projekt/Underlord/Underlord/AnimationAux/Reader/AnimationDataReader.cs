using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Animation
{
    public class AnimationDataReader : ContentTypeReader<AnimationData>
    {
        protected override AnimationData Read(ContentReader input, AnimationData existingData)
        {
            AnimationData data = new AnimationData();
            data.Skeleton = input.ReadObject<List<int>>();
            data.Clips = input.ReadObject<List<Clip>>();

            return data;
        }
    }
}
