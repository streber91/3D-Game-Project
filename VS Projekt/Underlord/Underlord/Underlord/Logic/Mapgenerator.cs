using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    static class Mapgenerator
    {
        public static void generateMap(Environment.Map map, int size, int diamond,int gold)
        {
            List<Entity.Vars_Func.WallTyp> specials = new List<Entity.Vars_Func.WallTyp>();
            specials.Add(Entity.Vars_Func.WallTyp.HQ);
            specials.Add(Entity.Vars_Func.WallTyp.EN);
            Random rand = new Random();
            Vector2 HQ = new Vector2();
            Vector2 EN = new Vector2();
            int dia = diamond;
            int go = gold;
            //determine special walls
            for (int i = 0 ; i < Math.Pow((size/5),2) - 2; ++i)
            {
                if (dia > 0)
                {
                    specials.Add(Entity.Vars_Func.WallTyp.Diamond);
                    --dia;
                }
                else if (go > 0)
                {
                    specials.Add(Entity.Vars_Func.WallTyp.Gold);
                    --go;
                }
                else
                {
                    specials.Add(Entity.Vars_Func.WallTyp.Stone);
                }
                
            }
            // build map
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    int randnum = rand.Next(specials.Count);
                    // special wall
                    if (j % 5 == 0 && i % 5 == 0)
                    {
                        // if entrance place entrance
                        if (specials[randnum] == Entity.Vars_Func.WallTyp.EN)
                            new Entity.Nest(Entity.Vars_Func.NestTyp.Entrance, new Vector2(i, j), map.getHexagonAt(i, j), map, HQ);
                        // if HQ place HQ
                        else if(specials[randnum] == Entity.Vars_Func.WallTyp.HQ)
                            new Entity.Creature(Entity.Vars_Func.CreatureTyp.HQCreatur, null, new Vector2(i, j), null, Entity.Vars_Func.ThingTyp.HQCreature, map);
                        //place specila wall if not entrance or HQ
                        else
                            new Entity.Wall(new Vector2(i, j), specials[randnum], 300, map);
  
                        if (specials[randnum] == Entity.Vars_Func.WallTyp.Diamond || Entity.Vars_Func.WallTyp.Gold == specials[randnum])
                        {
                            foreach (Vector2 hex in map.getHexagonAt(i, j).Neighbors)
                            {
                                new Entity.Wall(new Vector2(hex.X, hex.Y), Underlord.Entity.Vars_Func.WallTyp.Gold, 300, map);
                            }
                        }
                        if (specials[randnum] == Entity.Vars_Func.WallTyp.HQ) HQ = new Vector2(i, j);
                        if (specials[randnum] == Entity.Vars_Func.WallTyp.EN) EN = new Vector2(i, j);
                        specials.RemoveAt(randnum);
                    }
                    // fill rest with normal walls
                    else if(map.getHexagonAt(i, j).Obj == null) new Entity.Wall(new Vector2(i, j), Underlord.Entity.Vars_Func.WallTyp.Stone, 300, map);
                }
            }
            // set entrance target correct
            ((Entity.Nest)map.getHexagonAt(EN).Obj).TargetPos = HQ;
            // room for HQ
            foreach (Vector2 hex in map.getHexagonAt(HQ.X, HQ.Y).Neighbors)
            {
                map.getHexagonAt(hex.X, hex.Y).Obj = null;
            }
            // room for entrance
            foreach (Vector2 hex in map.getHexagonAt(EN.X, EN.Y).Neighbors)
            {
                map.getHexagonAt(hex.X, hex.Y).Obj = null;
            }
            // build path from first entrance to HQ
            while( !map.getHexagonAt(HQ.X, HQ.Y).Neighbors.Contains(EN))
            {
                if (HQ.X < EN.X)
                {
                    map.getHexagonAt(EN.X - 1, EN.Y).Obj = null;
                    --EN.X; 
                }
                else if (HQ.X > EN.X)
                {
                    map.getHexagonAt(EN.X + 1, EN.Y).Obj = null;
                    ++EN.X; 
                }
                if (HQ.Y < EN.Y)
                {
                    map.getHexagonAt(EN.X, EN.Y - 1).Obj = null;
                    --EN.Y;
                }
                else if (HQ.Y > EN.Y)
                {
                    map.getHexagonAt(EN.X, EN.Y + 1).Obj = null;
                    ++EN.Y;
                }
            }
        }
    }
}
