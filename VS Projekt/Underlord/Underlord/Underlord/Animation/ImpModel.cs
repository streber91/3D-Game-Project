using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using AnimationAux;

namespace Underlord.Animation
{
    class ImpModel : AnimationModel
    {
        #region Fields

        /// <summary>
        /// Encapsulated AnimationClips in AnimationModels.
        /// </summary>
        private AnimationModel idle, walk, dig; 

        #endregion

        #region Construction 

        /// <summary>
        /// Default Constructor. Creates the model from an XNA model
        /// </summary>
        /// <param name="assetName">The name of the asset for this model</param>
        public ImpModel(Model model) : base(model)
        {
         
        }

        /// <summary>
        /// Default Constructor. Creates the model from an XNA model
        /// </summary>
        /// <param name="assetName">The name of the asset for this model</param>
        public ImpModel(Model model, AnimationModel idle, AnimationModel walk, AnimationModel dig)  : base(model)
        {
            this.idle = idle;
            this.walk = walk;
            this.dig = dig;
        }

        #endregion

        #region Updating

        /// <summary>
        /// Play a job animation specievied by the job.
        /// </summary>
        /// <param name="gameTime"></param>
        public void PlayJobAnimation(GameTime gameTime, Logic.Job job)
        {
            switch (job.getJobTyp())
            {
                case Logic.Vars_Func.ImpJob.Harvest:

                    break;
                case Logic.Vars_Func.ImpJob.Mine:
                case Logic.Vars_Func.ImpJob.MineDiamonds:
                    
                    AnimationPlayer player = this.PlayClip(dig.Clips[0]);
                    player.Looping = true;
                    break;
            }
            
            this.Update(gameTime);
        }

        /// <summary>
        /// Play the default move animation.
        /// </summary>
        /// <param name="gameTime"></param>
        public void PlayMoveAnimation(GameTime gameTime)
        {
            AnimationPlayer player = this.PlayClip(walk.Clips[0]);
            player.Looping = true;

            this.Update(gameTime);
        }

        #endregion


    }
}
