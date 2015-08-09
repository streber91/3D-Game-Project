using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Underlord.Animation;
using Underlord.Basic;

namespace Underlord.Logic
{
   static class Vars_Func
    {
       public enum ThingTyp { Wall, Upgrade, Nest, DungeonCreature, HeroCreature, NeutralCreature, HQCreature, Imp, Farm, Temple, length };
       public enum CreatureTyp {Beetle, Knight, HQCreatur, length };
       public enum NestTyp { Beetle, Entrance, length };
       public enum UpgradeTyp {Damage, Life, Speed, length };
       public enum WallTyp { Stone, Gold, Diamond, HQ, EN, length };
       public enum HexTyp { Sand, Stone, BeetleNest, length };
       public enum SpellType { SummonImp, Fireball, length  }

       public enum GameState { MainMenue, Ingame, Save, Load, CreateRoom, Build, Mine, MergeRooms, DeleteRoom, BuildUpgrade, Spellcasting, PlaceFarm, PlaceTemple, length };

       public enum ImpJob { Idle, Harvest, Feed, Mine, MineDiamonds, MineGold, length };

       public enum GUI_ElementTyp { Mine, Room, MergeRoom, DeleteRoom, Build, PlaceFarm, PlaceTemple,
									DamageUpgrade, LifeUpgrade, SpeedUpgrade,
                                    LeftHUD, RightHUD, BottomHUD, TopHUD, RessoucesHUD, InfoHUD,
                                    Menu, StartGame, QuitGame, Highscore, length };

       public enum ImpState { Walking, Digging, Nothing, length };
       public enum CreatureState { Walking, Fightling, Nothing, length };

       static List<AnimationModel> CreatureModels;
       static List<BasicModel> NestModels;
       static List<BasicModel> UpgradeModels;
       static List<BasicModel> WallModels;
       static List<BasicModel> HexagonModels;
       static ImpModel ImpModel;

       static Texture2D pixel;
       static List<Texture2D> GUI_Elements;

       static Vector3[] CreaturParamters = { new Vector3(0,1,0), new Vector3(0.5f, 0.1f, MathHelper.PiOver2), new Vector3(0,1,0), new Vector3(0,1,0) };

       public static AnimationModel getCreatureModell(CreatureTyp typ) { return CreatureModels[(int)typ]; }
       public static BasicModel getNestModell(NestTyp typ) { return NestModels[(int)typ]; }
       public static BasicModel getUpgradeModell(UpgradeTyp typ) { return UpgradeModels[(int)typ]; }
       public static BasicModel getWallModell(WallTyp typ) { return WallModels[(int)typ]; }
       public static BasicModel getHexagonModell(HexTyp typ) { return HexagonModels[(int)typ]; }
       public static ImpModel getImpModell() { return ImpModel; }

       public static Texture2D getPixel() { return pixel; }
       public static Texture2D getGUI_ElementTextures(GUI_ElementTyp typ) { return GUI_Elements[(int)typ]; }

       public static Vector3 getCreatureParams(CreatureTyp typ) { return CreaturParamters[(int)typ]; }

       public static void loadContent(ContentManager Content)
       {
           CreatureModels = new List<AnimationModel>();
           NestModels = new List<BasicModel>();
           UpgradeModels = new List<BasicModel>();
           WallModels = new List<BasicModel>();
           HexagonModels = new List<BasicModel>();
           GUI_Elements = new List<Texture2D>();

           pixel = Content.Load<Texture2D>("TEST");

           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));

           GUI_Elements.Add(Content.Load<Texture2D>("Left_HUD_Test"));
           GUI_Elements.Add(Content.Load<Texture2D>("Right_HUD_Test"));
           GUI_Elements.Add(Content.Load<Texture2D>("Bottom_HUD_Test"));
           GUI_Elements.Add(Content.Load<Texture2D>("Top_HUD_Test"));
           GUI_Elements.Add(Content.Load<Texture2D>("Ressouces_HUD_Test"));
           GUI_Elements.Add(Content.Load<Texture2D>("Info_HUD_Test"));

           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));

           WallModels.Add(new BasicModel(Content.Load<Model>("Models//sandWall_HEX_02")));
           WallModels.Add(new BasicModel(Content.Load<Model>("Models//sandWall_HEX_02")));
           WallModels.Add(new BasicModel(Content.Load<Model>("Models//sandWall_HEX_02")));
           WallModels.Add(new BasicModel(Content.Load<Model>("Models//sandWall_HEX_02")));

           WallModels[(int)WallTyp.Stone].Texture = Content.Load<Texture2D>("Textures//wall_rock_TEXT");
           WallModels[(int)WallTyp.Gold].Texture = Content.Load<Texture2D>("Textures//wall_gold_TEXT");
           WallModels[(int)WallTyp.Diamond].Texture = Content.Load<Texture2D>("Textures//wall_diamond_TEXT");

           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));
           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));
           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));

           HexagonModels[(int)HexTyp.Sand].Texture = Content.Load<Texture2D>("Textures//floor_stone_TEXT");
           HexagonModels[(int)HexTyp.Stone].Texture = Content.Load<Texture2D>("Textures//floor_stone_TEXT");
           HexagonModels[(int)HexTyp.BeetleNest].Texture = Content.Load<Texture2D>("Textures//nest_orange_TEXT");

           NestModels.Add(new BasicModel(Content.Load<Model>("Models//nest_HEX_01")));
           NestModels.Add(new BasicModel(Content.Load<Model>("Models//tempel_HQ_GEO_01")));

           NestModels[(int)NestTyp.Beetle].Texture = Content.Load<Texture2D>("Textures//nest_orange_TEXT");
           //NestModels[(int)NestTyp.Entrance].Texture = Content.Load<Texture2D>("Textures//nest_orange_TEXT");

           UpgradeModels.Add(new BasicModel(Content.Load<Model>("Models//flag_Deg_GEO_01")));
           UpgradeModels.Add(new BasicModel(Content.Load<Model>("Models//flag_Lve_GEO_01")));
           UpgradeModels.Add(new BasicModel(Content.Load<Model>("Models//flag_Spd_GEO_01")));

           CreatureModels.Add(new AnimationModel(Content.Load<Model>("AnimationModels//ant_GEO_01")));
           CreatureModels.Add(new AnimationModel(Content.Load<Model>("AnimationModels//knight_&_sword_ANI_01")/*, 0.2f, MathHelper.PiOver2*/));
           CreatureModels.Add(new AnimationModel(Content.Load<Model>("AnimationModels//ant_GEO_01")));

           ImpModel = new ImpModel(Content.Load<Model>("AnimationModels//minion_ANI_walk_simple_02"));
           ImpModel.AddClip(new AnimationModel (Content.Load<Model>("AnimationModels//minion_ANI_grabbling_01")));
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
               foreach (Vector2 hex in map.getHexagonAt(tmp).Neighbors)
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

       public static void resetHexagonColors(Environment.Map map)
       {
           foreach (Environment.Hexagon hex in map.getMapHexagons())
            {
                hex.Color = Color.White;
            }
           //colors the room-hexagons in CreateRoom mode, MergeRooms mode, DeleteRoom mode, Build  mode, BuildUpgrade mode
           if(Interaction.GameState == Vars_Func.GameState.CreateRoom ||
               Interaction.GameState == Vars_Func.GameState.MergeRooms ||
               Interaction.GameState == Vars_Func.GameState.DeleteRoom ||
               Interaction.GameState == Vars_Func.GameState.Build ||
               Interaction.GameState == Vars_Func.GameState.BuildUpgrade)
           {
               foreach (Environment.Hexagon hex in map.getMapHexagons())
               {
                   if (hex.RoomNumber == 0) { }
                   else if (hex.RoomNumber % 10 == 0) hex.Color = Color.Red;
                   else if (hex.RoomNumber % 10 == 1) hex.Color = Color.Yellow;
                   else if (hex.RoomNumber % 10 == 2) hex.Color = Color.Blue;
                   else if (hex.RoomNumber % 10 == 3) hex.Color = Color.Black;
                   else if (hex.RoomNumber % 10 == 4) hex.Color = Color.Green;
                   else if (hex.RoomNumber % 10 == 5) hex.Color = Color.Purple;
                   else if (hex.RoomNumber % 10 == 6) hex.Color = Color.Lerp(Color.Black, Color.Green, 0.5f);
                   else if (hex.RoomNumber % 10 == 7) hex.Color = Color.Lerp(Color.Black, Color.Green, 0.5f);
                   else if (hex.RoomNumber % 10 == 8) hex.Color = Color.Lerp(Color.Black, Color.Green, 0.5f);
                   else if (hex.RoomNumber % 10 == 9) hex.Color = Color.Lerp(Color.Black, Color.Green, 0.5f);
               }
           }
           //colors the hexagons which are in the mineJobs list
           else if (Interaction.GameState == Vars_Func.GameState.Mine)
           {
               foreach(Vector2 pos in map.MineJobs)
               {
                   map.getHexagonAt(pos).Color = Color.Purple;
               }
           }
       }
    }
}
