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
    class Minimap : Microsoft.Xna.Framework.Game
    {
        int hexsize;
        Map map;
        Vector2 position, dimension;
        SpriteBatch sb;
        Texture2D test;

        public Minimap(Map map, Vector2 pos, Vector2 dimen, SpriteBatch spriteBatch)
        {
            this.map = map;
            this.position = pos;
            this.dimension = dimen;

            this.hexsize = (int)(dimension.X / map.getPlanelength());

            this.sb = spriteBatch;
            test = Content.Load<Texture2D>("TEST");
        }

        public void drawMinimap()
        {
            Hexagon temp;

            Rectangle bg = new Rectangle((int)position.X, (int)position.Y, (int)dimension.X, (int)dimension.Y);
            sb.Draw(test, bg, Color.Black);

            for (int i = 0; i < map.getMapHexagons().Count; i++)
            {
                temp = map.getMapHexagons()[i];
                
                if(temp.Obj.getThingTyp().Equals(Vars_Func.ThingTyp.Wall))
                {
                    drawHex(temp.getIndexNumber(), Color.Orange);
                }
            }
        }

        public void drawHex(Vector2 pos, Color c)
        {
            Rectangle rec = new Rectangle((int)(position.X + pos.X * hexsize + (pos.X % 2) * (0.5f * hexsize)), (int)(position.Y + pos.Y * hexsize), hexsize, hexsize);
            sb.Draw(test, rec, c);
        }
        
    }
}
