using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    static class Mapgenerator
    {
        //TODO: test generator with new textures
        public static void generateMap(Environment.Map hexmap, int size, int diamond,int gold)
        {
            List<Entity.Vars_Func.WallTyp> specials = new List<Entity.Vars_Func.WallTyp>();
            specials.Add(Entity.Vars_Func.WallTyp.HQ);
            specials.Add(Entity.Vars_Func.WallTyp.Entrance);
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
                    //special wall
                    if (j % 5 == 0 && i % 5 == 0)
                    {
                        new Entity.Wall(new Vector2(i, j), specials[randnum], 300, hexmap);
                        if (specials[randnum] == Entity.Vars_Func.WallTyp.Diamond || Entity.Vars_Func.WallTyp.Gold == specials[randnum])
                        {
                            foreach (Vector2 hex in hexmap.getHexagonAt(i, j).getNeighbors())
                            {
                                new Entity.Wall(new Vector2(hex.X, hex.Y), Underlord.Entity.Vars_Func.WallTyp.Gold, 300, hexmap);
                            }
                        }
                        if (specials[randnum] == Entity.Vars_Func.WallTyp.HQ) HQ = new Vector2(i, j);
                        if (specials[randnum] == Entity.Vars_Func.WallTyp.Entrance) EN = new Vector2(i, j);
                        specials.RemoveAt(randnum);
                    }
                    // fill rest with normal walls
                    else if(hexmap.getHexagonAt(i, j).Obj == null) new Entity.Wall(new Vector2(i, j), Underlord.Entity.Vars_Func.WallTyp.Stone, 300, hexmap);
                }
            }
            // room for HQ
            foreach (Vector2 hex in hexmap.getHexagonAt(HQ.X, HQ.Y).getNeighbors())
            {
                hexmap.getHexagonAt(hex.X, hex.Y).Obj = null;
            }
            // build path from first entry to HQ
            while( !hexmap.getHexagonAt(HQ.X, HQ.Y).getNeighbors().Contains(EN))
            {
                if (HQ.X < EN.X)
                {
                    hexmap.getHexagonAt(EN.X - 1, EN.Y).Obj = null;
                    --EN.X; 
                }
                else if (HQ.X > EN.X)
                {
                    hexmap.getHexagonAt(EN.X + 1, EN.Y).Obj = null;
                    ++EN.X; 
                }
                if (HQ.Y < EN.Y)
                {
                    hexmap.getHexagonAt(EN.X, EN.Y - 1).Obj = null;
                    --EN.Y;
                }
                else if (HQ.Y > EN.Y)
                {
                    hexmap.getHexagonAt(EN.X, EN.Y + 1).Obj = null;
                    ++EN.Y;
                }
            }
        }
    }
}
