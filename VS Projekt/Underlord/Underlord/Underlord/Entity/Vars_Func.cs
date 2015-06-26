using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Underlord.Basic;

namespace Underlord.Entity
{
   static class Vars_Func
    {
       public enum CreatureTyp {Beetle, Knight, length };
       public enum NestTyp { Beetle, length };
       public enum UpgradeTyp {Arcane, Training, length };
       public enum WallTyp { Sand, Gold, Diamond, Entrance, HQ, length };
       public enum HexTyp { Sand, length };
       public enum GameState { MainMenue, Ingame, Save, Load, CreateRoom, Build, Mine, length };
       public enum ThingTyp { Wall, Upgrade, Nest, DungeonCreature, HeroCreature, NeutralCreature, length };

       static List<Model> CreatureModels;
       static List<Model> NestModels;
       static List<Model> UpgradeModels;
       //static List<Model> WallModels;
       static List<BasicModel> WallModels;
       //static List<Model> HexagonModels;
       static List<BasicModel> HexagonModels;

       static Texture2D pixel;

       public static Model getCreatureModell(CreatureTyp typ) { return CreatureModels[(int)typ]; }
       public static Model getNestModell(NestTyp typ) { return NestModels[(int)typ]; }
       public static Model getUpgradeModell(UpgradeTyp typ) { return UpgradeModels[(int)typ]; }
       //public static Model getWallModell(WallTyp typ) { return WallModels[(int)typ]; }
       public static BasicModel getWallModell(WallTyp typ) { return WallModels[(int)typ]; }
       //public static Model getHexagonModell(HexTyp typ) { return HexagonModels[(int)typ]; }
       public static BasicModel getHexagonModell(HexTyp typ) { return HexagonModels[(int)typ]; }

       public static Texture2D getPixel() { return pixel; }

       public static void loadContent(ContentManager Content)
       {
           CreatureModels = new List<Model>();
           NestModels = new List<Model>();
           UpgradeModels = new List<Model>();

          // WallModels = new List<Model>();
           WallModels = new List<BasicModel>();

         //  HexagonModels = new List<Model>();
           HexagonModels = new List<BasicModel>();

           pixel = Content.Load<Texture2D>("TEST");
           //WallModels.Add(Content.Load<Model>("Models//sandWall_HEX_02"));
           //WallModels.Add(Content.Load<Model>("Models//sandWall_HEX_02"));
           //WallModels.Add(Content.Load<Model>("Models//sandWall_HEX_02"));
           //WallModels.Add(Content.Load<Model>("Models//sandWall_HEX_02"));
           //WallModels.Add(Content.Load<Model>("Models//sandWall_HEX_02"));

           WallModels.Add(new BasicModel("Models//sandWall_HEX_02"));
           WallModels.Add(new BasicModel("Models//sandWall_HEX_02"));
           WallModels.Add(new BasicModel("Models//sandWall_HEX_02"));
           WallModels.Add(new BasicModel("Models//sandWall_HEX_02"));
           WallModels.Add(new BasicModel("Models//sandWall_HEX_02"));

          // HexagonModels.Add(Content.Load<Model>("Models//floorSand_HEX_03"));
           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));

           foreach (BasicModel bWall in WallModels)
           {
               bWall.LoadContent(Content);
           }   
           
           
     
       }

       public static Vector3 mousepos(GraphicsDevice graphics, MouseState mousestate, Matrix projection, Matrix view)
       {
           // calculate 3D mouspose out of an 2D Mosepos with Z = 0
           // represents the mousover in 3D space
           Vector3 vec1 = graphics.Viewport.Unproject(new Vector3(mousestate.X, mousestate.Y, 1), projection, view, Matrix.Identity);
           Vector3 vec2 = graphics.Viewport.Unproject(new Vector3(mousestate.X, mousestate.Y, 0), projection, view, Matrix.Identity);
           float a = -vec1.Z / (vec2.Z - vec1.Z);
           Vector3 mousepos = new Vector3(a * (vec2.X - vec1.X) + vec1.X, a * (vec2.Y - vec1.Y) + vec1.Y, 0.0f);

           return mousepos;
       }

       public static int computeDistance(Vector2 pos1, Vector2 pos2, Environment.Map map)
       {
           //return statement
            int distanz = 0;

            //breadth-first search
            Vector2 tmp = new Vector2();
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(pos1);
            map.getHexagonAt(pos1).Visited = true;
            queue.Enqueue(new Vector2(map.getPlanelength(), 0));

            while (queue.Count != 1)
            {
                tmp = queue.Dequeue();
                if (tmp == pos2) break;
                if (tmp.X == map.getPlanelength())
                {
                    ++distanz;
                    queue.Enqueue(tmp);
                    continue;
                }
                foreach (Vector2 hex in map.getHexagonAt(tmp).getNeighbors())
                {
                    if (!map.getHexagonAt(hex).Visited)
                    {
                        queue.Enqueue(hex);
                        map.getHexagonAt(hex).Visited = true;
                    }
                }
            }
            //clear Hexmap for next search
            for (int i = 0; i < map.getPlanelength(); ++i)
            {
                for (int j = 0; j < map.getPlanelength(); ++j)
                {
                    map.getHexagonAt(i, j).Visited = false;
                }
            }

            return distanz;
       }
       // TODO write comment
       public static Vector2 gridColision(Vector3 position, int planeLength, float hexagonSideLength)
       {
           //magic 
           float positionX = position.X;
           float positionY = position.Y;
           int X = 0;
           int Y = 0;
           while (positionX < 0) positionX = positionX + (planeLength * 1.5f * hexagonSideLength);
           while (positionY < 0) positionY = positionY + (planeLength * 2 * 0.875f * hexagonSideLength);
           positionX = positionX % (planeLength * 1.5f * hexagonSideLength);
           positionY = positionY % (planeLength * 2 * 0.875f * hexagonSideLength);

           while (positionX >= 1.5f * hexagonSideLength)
           {
               positionX -= 1.5f;
               ++X;
           }
           while (positionY >= 0.875f * hexagonSideLength)
           {
               positionY -= 0.875f;
               ++Y;
           }

           if (positionX <= hexagonSideLength / 2)
           {
               if ((X + Y) % 2 == 0)
               {
                   if (0.875 >= positionX * 1.75 + positionY) --X;
                   if (X < 0) X += planeLength;
               }
               else
               {
                   if (0 >= positionX * 1.75 - positionY) --X;
                   if (X < 0) X += planeLength;
               }
           }
           if (X % 2 != 0)
           {
               --Y;
               if (Y < 0) Y += planeLength * 2;
           }

           return new Vector2(X, (int)(Y / 2));
       }
    }
}
