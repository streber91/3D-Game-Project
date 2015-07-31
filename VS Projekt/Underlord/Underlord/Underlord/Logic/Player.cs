using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Underlord.Logic
{
    static class Player
    {
        static int gold = 0, mana = 0;

        #region Properties
        public static int Gold
        {
            get { return gold; }
            set { gold = value; }
        }
        public static int Mana
        {
            get { return mana; }
            set { mana = value; }
        }
        #endregion
    }
}
