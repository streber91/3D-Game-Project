using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Underlord.Entity;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Underlord.Environment
{
    class Minimap
    {
        int hexsize;
        Map map;
        Vector2 position, dimension;

        public Minimap(Map map, Vector2 pos, Vector2 dimen)
        {
            this.map = map;
            this.position = pos;
            this.dimension = dimen;

            this.hexsize = (int)(dimension.X / map.getPlanelength());
        }

        public void drawMinimap(SpriteBatch sb)
        {
            Hexagon temp;

            Rectangle bg = new Rectangle((int)position.X, (int)position.Y, (int)dimension.X, (int)dimension.Y);
            sb.Draw(Vars_Func.getPixel(), bg, Color.Black);

            for (int i = 0; i < map.getMapHexagons().Count; i++)
            {
                temp = map.getMapHexagons()[i];

                if (temp.Obj != null && temp.Obj.getThingTyp().Equals(Vars_Func.ThingTyp.Wall))
                {
                    drawHex(temp.getIndexNumber(), Color.Orange , sb);
                }
            }
        }

        public void drawHex(Vector2 pos, Color c , SpriteBatch sb)
        {
            Rectangle rec = new Rectangle((int)(position.X + pos.X * hexsize), (int)(position.Y + (hexsize * map.getPlanelength() - pos.Y * hexsize -1) - (pos.X % 2) * (0.5f * hexsize)), hexsize, hexsize);
            sb.Draw(Vars_Func.getPixel(), rec, c);
        }
        
    }
}
