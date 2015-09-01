using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Underlord.Logic
{
    static class Player
    {
        static int gold = 100, mana = 100, food = 100, score = 0;

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
        public static int Food
        {
            get { return food; }
            set { food = value; }
        }
        public static int Score
        {
            get { return score; }
            set { score = value; }
        }
        #endregion

        public static bool enoughFood(int foodneeded)
        {
            if (foodneeded <= food) return true;
            else return false;
        }

        public static bool enoughGold(int goldneeded)
        {
            if (goldneeded <= gold) return true;
            else return false;
        }

        public static bool enoughMana(int mananeeded)
        {
            if (mananeeded <= mana) return true;
            else return false;
        }
    }
}
