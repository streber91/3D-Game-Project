using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    static class WaveController
    {
        static float timeCounter, interWaveTime = 60000 * 1;
        static int waveCounter;

        #region Properties
        public static int  WaveCounter
        {
            get { return waveCounter; }
        }
        public static int TimeToNextWave
        {
            get { return (int)((interWaveTime - timeCounter)/1000); }
        }
        #endregion

        public static void restart()
        {
            timeCounter = 0;
            waveCounter = 0;
        }

        public static void update(GameTime gameTime, Environment.Map map)
        {
            timeCounter += gameTime.ElapsedGameTime.Milliseconds;
            if (timeCounter >= interWaveTime)
            {   
                map.Waves.Add(new Wave(waveCounter * 10, (int)(5 + waveCounter / 3)));
                ++waveCounter;
                timeCounter -= interWaveTime;
            }
        }
    }
}
