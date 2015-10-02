using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Animation;

namespace Underlord.Animation
{
    class ImpModel : CharacterModel
    {
        #region Fields

        // Encapsulated Clips in CharacterModels.
        private CharacterModel idle, walk, dig; 

        #endregion

        #region Construction 

        public ImpModel(Model model) : base(model)
        {
         
        }

        public ImpModel(Model model, CharacterModel idle, CharacterModel walk, CharacterModel dig)
            : base(model)
        {
            this.idle = idle;
            this.walk = walk;
            this.dig = dig;
        }

        #endregion

        #region Update

        public void PlayMoveAnimation(GameTime gameTime)
        {

        }

        #endregion
    }
}
