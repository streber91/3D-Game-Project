using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    class Wave
    {
        int startage, numberEnemys;
        float spawncounter;

        #region Properties

        #endregion

        #region Constructor
        public Wave(int startage, int numberEnemys)
        {
            this.startage = startage;
            this.numberEnemys = numberEnemys;
        }
        #endregion

        public void update(GameTime gameTime, Environment.Map map)
        {
            spawncounter += gameTime.ElapsedGameTime.Milliseconds;

            if (spawncounter >= 1500)
            {
                foreach (Entity.Nest e in map.Entrances)
                {
                    e.spawnCreature(map, startage);
                }
                --numberEnemys;
                spawncounter = 0;

                if (numberEnemys <= 0)
                {
                    map.Waves.Remove(this);
                }
            }
        }
    }
}
