using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Underlord.Entity
{
   static class Vars_Func
    {
       public enum Typ {Beetle, Knight, length };
       public enum UpgradeTyp {Arcane, Training, length };
       public enum TypWall {Stone, Gold, Diamond, length };

       static List<Model> CreatureModels;
       static List<Model> NestModels;
       static List<Model> UpgradeModels;
       static List<Model> WallModels;
       static List<Model> HexagonModels;

       public static Vector3 mousepos(GraphicsDevice graphics, MouseState mousestate, Matrix projection, Matrix view)
       {
           Vector3 vec1 = graphics.Viewport.Unproject(new Vector3(mousestate.X, mousestate.Y, 1), projection, view, Matrix.Identity);
           Vector3 vec2 = graphics.Viewport.Unproject(new Vector3(mousestate.X, mousestate.Y, 0), projection, view, Matrix.Identity);
           float a = -vec1.Z / (vec2.Z - vec1.Z);
           Vector3 mousepos = new Vector3(a * (vec2.X - vec1.X) + vec1.X, a * (vec2.Y - vec1.Y) + vec1.Y, 0.0f);

           return mousepos;
       }

       public static Vector2 gridColision(Vector3 position, int planeLength, float hexagonSideLength)
       {
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
