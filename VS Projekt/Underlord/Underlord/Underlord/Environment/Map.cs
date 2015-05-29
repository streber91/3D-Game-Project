using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Entity;
using System.IO;

namespace Underlord.Environment
{
    class Map
    {
            String name = "";
            Hexagon[][] map;
            Boolean loaded = false;
           

        public Map(int x, int y, String name, Boolean loaded)
        {
            this.loaded = loaded;

            if (loaded == false)
            {
                this.name = name;

                for (int i = 0; i < y; i++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        map[j][i]= new Hexagon(Vars_Func.TypHex.Default, j, i);

                    }
                }
            }
            else
            {
                // insert the savegame here.
                load(null);
            }
        }
        public void save()
        {
        }

        public void move(Creature crea)
        {
        }
        public void remove(Thing thing)
        {
            // determine position and set map[x][y] = null
        }

        private void load(File savegame)
        {

        }

    }
}
