using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
        
        public static void saveScore()
        {
            int[] highscores = new int[10];
            int tmpscore;

            using (StreamReader sr = new StreamReader("Content/Highscore.txt", Encoding.UTF7, false))
            {
                for (int i = 0; i < 10; ++i)
                {
                    highscores[i] = Int32.Parse(sr.ReadLine());
                }
            }

            if (score > highscores[9])
            {
                highscores[9] = score;
                for (int i = 9; i > 0; --i)
                {
                    if (highscores[i] > highscores[i - 1])
                    {
                        tmpscore = highscores[i - 1];
                        highscores[i - 1] = highscores[i];
                        highscores[i] = tmpscore;
                    }
                }
            }

            TextWriter writer = new StreamWriter("Content/Highscore.txt");

            for (int i = 0; i < 10; ++i)
            {
                writer.WriteLine(highscores[i].ToString());
                writer.Flush();
            }
            writer.Close();
        }

        public static String[] loadScore()
        {
            String[] result = new String[10];

            using (StreamReader sr = new StreamReader("Content/Highscore.txt", Encoding.UTF7, false))
            {
                for (int i = 0; i < 10; ++i)
                {
                    result[i] = sr.ReadLine();
                }
            }

            return result;
        }

        /*
          Char[] tmp;
            
                using (StreamReader sr = new StreamReader("Content/Highscore.txt", Encoding.UTF7, false))
                {
                    tmp = new Char[sr.BaseStream.Length];
                    sr.ReadBlock(tmp, 0, (int)sr.BaseStream.Length);
                }
                int highscore = Int32.Parse(new String(tmp));
                if (score > highscore)
                {
                    TextWriter writer = new StreamWriter("Content/Highscore.txt");
                    writer.Write(score);
                    writer.Flush();
                    writer.Close();
                }
        */

        public static String loadString(String filename)
        {
            Char[] result;
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF7, false))
            {
                result = new Char[sr.BaseStream.Length];
                sr.ReadBlock(result, 0, (int)sr.BaseStream.Length);
            }

            return new String(result);
        }

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
