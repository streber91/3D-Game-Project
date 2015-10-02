using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animation
{
    //Contains the animation data attached to the model 
    public class AnimationData
    {
        #region Fields
        
        private List<int> skeleton = new List<int>();

        public List<Clip> clips = new List<Clip>();

        #endregion

        #region Properties

        public List<int> Skeleton { get { return skeleton; } set { skeleton = value; } }

        public List<Clip> Clips { get { return clips; } set { clips = value; } }

        #endregion
    }
}
