﻿using System;
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
            List<Logic.Vars_Func.WallTyp> specials = new List<Logic.Vars_Func.WallTyp>();
            specials.Add(Logic.Vars_Func.WallTyp.HQ);
            specials.Add(Logic.Vars_Func.WallTyp.EN);
            Random rand = new Random();
            Vector2 EN = new Vector2();
            int dia = diamond;
            int go = gold;
            //determine special walls
            for (int i = 0 ; i < Math.Pow((size/5),2) - 2; ++i)
            {
                if (dia > 0)
                {
                    specials.Add(Logic.Vars_Func.WallTyp.Diamond);
                    --dia;
                }
                else if (go > 0)
                {
                    specials.Add(Logic.Vars_Func.WallTyp.Gold);
                    --go;
                }
                else
                {
                    specials.Add(Logic.Vars_Func.WallTyp.Stone);
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
                        if (specials[randnum] == Logic.Vars_Func.WallTyp.EN)
                            new Logic.Nest(Logic.Vars_Func.NestTyp.Entrance, new Vector2(i, j), map.getHexagonAt(i, j), map, map.HQPosition);
                        // if HQ place HQ
                        else if(specials[randnum] == Logic.Vars_Func.WallTyp.HQ)
                            new Logic.Creature(Logic.Vars_Func.CreatureTyp.HQCreatur, new Vector2(i, j), null, Logic.Vars_Func.ThingTyp.HQCreature, map);
                        //place specila wall if not entrance or HQ
                        else
                            new Logic.Wall(new Vector2(i, j), specials[randnum], 300, map);
  
                        if (specials[randnum] == Logic.Vars_Func.WallTyp.Diamond || Logic.Vars_Func.WallTyp.Gold == specials[randnum])
                        {
                            foreach (Vector2 hex in map.getHexagonAt(i, j).Neighbors)
                            {
                                new Logic.Wall(new Vector2(hex.X, hex.Y), Underlord.Logic.Vars_Func.WallTyp.Gold, 300, map);
                            }
                        }
                        if (specials[randnum] == Logic.Vars_Func.WallTyp.HQ) map.HQPosition = new Vector2(i, j);
                        if (specials[randnum] == Logic.Vars_Func.WallTyp.EN) EN = new Vector2(i, j);
                        specials.RemoveAt(randnum);
                    }
                    // fill rest with normal walls
                    else if(map.getHexagonAt(i, j).Obj == null) new Logic.Wall(new Vector2(i, j), Underlord.Logic.Vars_Func.WallTyp.Stone, 300, map);
                }
            }
            // set entrance target correct
            ((Logic.Nest)map.getHexagonAt(EN).Obj).TargetPosition = map.HQPosition;
            // room for HQ
            foreach (Vector2 hex in map.getHexagonAt(map.HQPosition.X, map.HQPosition.Y).Neighbors)
            {
                map.getHexagonAt(hex.X, hex.Y).Obj = null;
            }
            // room for entrance
            foreach (Vector2 hex in map.getHexagonAt(EN.X, EN.Y).Neighbors)
            {
                map.getHexagonAt(hex.X, hex.Y).Obj = null;
            }
            // build path from first entrance to HQ
            while (!map.getHexagonAt(map.HQPosition.X, map.HQPosition.Y).Neighbors.Contains(EN))
            {
                if (map.HQPosition.X < EN.X)
                {
                    map.getHexagonAt(EN.X - 1, EN.Y).Obj = null;
                    --EN.X; 
                }
                else if (map.HQPosition.X > EN.X)
                {
                    map.getHexagonAt(EN.X + 1, EN.Y).Obj = null;
                    ++EN.X; 
                }
                if (map.HQPosition.Y < EN.Y)
                {
                    map.getHexagonAt(EN.X, EN.Y - 1).Obj = null;
                    --EN.Y;
                }
                else if (map.HQPosition.Y > EN.Y)
                {
                    map.getHexagonAt(EN.X, EN.Y + 1).Obj = null;
                    ++EN.Y;
                }
            }
        }
    }
}
