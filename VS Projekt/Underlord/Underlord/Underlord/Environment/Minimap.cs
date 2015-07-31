using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Underlord.Entity;
using Underlord.Logic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Underlord.Environment
{
    class Minimap
    {
        int hexsize;
        Map map;
        Vector2 position, dimension;

        public Minimap(Map map, Vector2 position, Vector2 dimension)
        {
            this.hexsize = (int)(dimension.X / map.getPlanelength());
            this.map = map;
            this.position = position;
            this.dimension = dimension;

            
        }

        public void drawMinimap(SpriteBatch spritebatch, Vector2 cameraPosition)
        {
            Hexagon temp;

            //Rectangle background = new Rectangle((int)position.X, (int)position.Y, (int)dimension.X, (int)dimension.Y);
            //spritebatch.Draw(Vars_Func.getPixel(), background, Color.Black);

            for (int i = 0; i < map.getMapHexagons().Length; i++)
            {
                temp = map.getMapHexagons()[i];

                if (temp.Obj != null && temp.Obj.getThingTyp().Equals(Vars_Func.ThingTyp.Wall))
                {
                    //abfrage welche art von Wall vorliegt
                    Wall tmp = (Wall)temp.Obj;
                    if (tmp.Typ.Equals(Vars_Func.WallTyp.Stone)) drawHex(temp.IndexNumber, Color.Orange, spritebatch);
                    if (tmp.Typ.Equals(Vars_Func.WallTyp.Gold)) drawHex(temp.IndexNumber, Color.Yellow, spritebatch);
                    if (tmp.Typ.Equals(Vars_Func.WallTyp.Diamond)) drawHex(temp.IndexNumber, Color.Green, spritebatch);
                }
            }
            drawHex(cameraPosition, Color.Purple, spritebatch);
        }

        public void drawHex(Vector2 position, Color color , SpriteBatch spritebatch)
        {
            Rectangle pixelRectangle = new Rectangle((int)(this.position.X + position.X * hexsize),
                                                        (int)(this.position.Y + (hexsize * map.getPlanelength() - position.Y * hexsize -1) - (position.X % 2) * (0.5f * hexsize)), hexsize, hexsize);
            spritebatch.Draw(Vars_Func.getPixel(), pixelRectangle, color);
        }

    }
}
