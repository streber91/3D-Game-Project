using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    static class Mapgenerator
    {
        public static List<Entity.Thing> generateMap(Environment.Map hexmap, int size = 20 )
        {
            List<Entity.Thing> mapObjekts  = new List<Entity.Thing>();

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    mapObjekts.Add(new Entity.Wall(new Vector2(i, j), Entity.Vars_Func.WallTyp.Sand, 300, hexmap));
                }
            }

            return mapObjekts;
        }
    }
}
