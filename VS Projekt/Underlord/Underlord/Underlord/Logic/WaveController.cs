using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    class WaveController
    {
        float timeCounter, interWaveTime;
        int waveCounter;

        #region Properties

        #endregion

        #region Constructor
        public WaveController(float interWaveTime)
        {
            this.interWaveTime = interWaveTime;
            timeCounter = 0;
        }
        #endregion

        public void update(GameTime gameTime, Environment.Map map)
        {
            timeCounter += gameTime.ElapsedGameTime.Milliseconds;
            if (timeCounter >= interWaveTime)
            {
                ++waveCounter;
                timeCounter -= interWaveTime;
                map.Waves.Add(new Wave(waveCounter * 10, (int)(5 + waveCounter / 3)));
            }
        }
    }
}
