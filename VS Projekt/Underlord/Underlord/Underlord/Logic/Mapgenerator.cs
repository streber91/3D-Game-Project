using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Underlord.Entity;

namespace Underlord.Logic
{
    static class Mapgenerator
    {
        public static int counter = 0;
        public static void generateMap(Environment.Map map, int size, int diamond, int gold)
        {
            List<Vars_Func.WallTyp> specials = new List<Vars_Func.WallTyp>();
            specials.Add(Vars_Func.WallTyp.HQ);
            specials.Add(Vars_Func.WallTyp.EN);
            Random rand = new Random();
            Vector2 EN = new Vector2();
            int dia = diamond;
            int go = gold;
            //determine special walls
            for (int i = 0 ; i < Math.Pow((size/5),2) - 2; ++i)
            {
                if (dia > 0)
                {
                    specials.Add(Vars_Func.WallTyp.Diamond);
                    --dia;
                }
                else if (go > 0)
                {
                    specials.Add(Vars_Func.WallTyp.Gold);
                    --go;
                }
                else
                {
                    specials.Add(Vars_Func.WallTyp.Stone);
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
                        if (specials[randnum] == Vars_Func.WallTyp.EN)
                            new Nest(Vars_Func.NestTyp.Entrance, new Vector2(i, j), map, map.HQPosition);
                        // if HQ place HQ
                        else if(specials[randnum] == Vars_Func.WallTyp.HQ)
                            new Creature(Vars_Func.CreatureTyp.HQCreatur, new Vector2(i, j), null, Vars_Func.ThingTyp.HQCreature, map, new int[3]);
                        //place specila wall if not entrance or HQ
                        else
                            new Wall(new Vector2(i, j), specials[randnum], 300, map);
  
                        if (specials[randnum] == Vars_Func.WallTyp.Diamond || Vars_Func.WallTyp.Gold == specials[randnum])
                        {
                            foreach (Vector2 hex in map.getHexagonAt(i, j).Neighbors)
                            {
                                new Wall(new Vector2(hex.X, hex.Y), Vars_Func.WallTyp.Gold, 150, map);
                            }
                        }
                        else if (specials[randnum] == Vars_Func.WallTyp.HQ) map.HQPosition = new Vector2(i, j);
                        else if (specials[randnum] == Vars_Func.WallTyp.EN) EN = new Vector2(i, j);
                        specials.RemoveAt(randnum);
                    }
                    // fill rest with normal walls
                    else if(map.getHexagonAt(i, j).Obj == null) new Wall(new Vector2(i, j), Vars_Func.WallTyp.Stone, 40, map);
                }
            }
            // set entrance target correct
            ((Nest)map.getHexagonAt(EN).Obj).TargetPosition = map.HQPosition;
            // room for HQ
            foreach (Vector2 hex in map.getHexagonAt(map.HQPosition.X, map.HQPosition.Y).Neighbors)
            {
                map.getHexagonAt(hex.X, hex.Y).Obj = null;
            }
            //creat start imps
            Spells.castSpell(Vars_Func.SpellType.SummonImp, map.getHexagonAt(map.HQPosition).Neighbors[3],map);
            Spells.castSpell(Vars_Func.SpellType.SummonImp, map.getHexagonAt(map.HQPosition).Neighbors[3], map);
            Spells.castSpell(Vars_Func.SpellType.SummonImp, map.getHexagonAt(map.HQPosition).Neighbors[3], map); 
            // room for entrance
            foreach (Vector2 hex in map.getHexagonAt(EN.X, EN.Y).Neighbors)
            {
                map.getHexagonAt(hex.X, hex.Y).Obj = null;
            }
            // build path from first entrance to HQ
            while (!map.getHexagonAt(map.HQPosition.X, map.HQPosition.Y).Neighbors.Contains(EN))
            {
                Vector2 tempPos = Vector2.Zero;
                if (map.HQPosition.X < EN.X)
                {
                    map.getHexagonAt(EN.X - 1, EN.Y).Obj = null;
                    tempPos = new Vector2(EN.X - 1, EN.Y);
                    counter++;
                    --EN.X;
                }
                else if (map.HQPosition.X > EN.X)
                {
                    map.getHexagonAt(EN.X + 1, EN.Y).Obj = null;
                    tempPos = new Vector2(EN.X + 1, EN.Y);
                    counter++;
                    ++EN.X;
                }
                if (map.HQPosition.Y < EN.Y && !map.getHexagonAt(map.HQPosition.X, map.HQPosition.Y).Neighbors.Contains(EN))
                {
                    map.getHexagonAt(EN.X, EN.Y - 1).Obj = null;
                    tempPos = new Vector2(EN.X, EN.Y - 1);
                    counter++;
                    --EN.Y;
                }
                else if (map.HQPosition.Y > EN.Y && !map.getHexagonAt(map.HQPosition.X, map.HQPosition.Y).Neighbors.Contains(EN))
                {
                    map.getHexagonAt(EN.X, EN.Y + 1).Obj = null;
                    tempPos = new Vector2(EN.X, EN.Y + 1);
                    counter++;
                    ++EN.Y;
                }
                if (tempPos != Vector2.Zero)
                {
                    if (counter == 8)
                    {
                        map.getHexagonAt(tempPos).EnlightendHexagon(map);
                        counter = 0;
                    }
                }
            }
        }
    }
}
