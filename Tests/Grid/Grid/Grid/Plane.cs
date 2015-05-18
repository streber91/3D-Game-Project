using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Grid
{
    class Plane
    {
        List<Hexagon> plane = new List<Hexagon>();

        public Plane(int sidelength, float hexagonsidelength)
        {
            for (int i = 0; i < sidelength; ++i)
            {
                for (int j = 0; j < sidelength; ++j)
                {
                    /*plane.Add(new Hexagon(new Microsoft.Xna.Framework.Vector3(i * (hexagonsidelength + hexagonsidelength * (float)Math.Sqrt(2)) + hexagonsidelength / 2 + hexagonsidelength / (float)Math.Sqrt(2),
                                                                              hexagonsidelength / (float)Math.Sqrt(2), 0), hexagonsidelength, Color.White));*/

                    /*if (j % 2 == 0) plane.Add(new Hexagon(new Microsoft.Xna.Framework.Vector3(i * 3 * hexagonsidelength + hexagonsidelength,
                                                                                    j * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.White));
                    else plane.Add(new Hexagon(new Microsoft.Xna.Framework.Vector3(i * 3 * hexagonsidelength + 2.5f*hexagonsidelength,
                                                                                    j * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.White));*/

                    if (i % 2 == 0) plane.Add(new Hexagon(new Microsoft.Xna.Framework.Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                                                                    j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.White));
                    else plane.Add(new Hexagon(new Microsoft.Xna.Framework.Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                                                                    j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.White));
     ;          }
            }
        }

        public void Draw(GameTime gameTime, GraphicsDevice graphics)
        {
            for (int i = 0; i < plane.Count; ++i)
            {
                plane[i].Draw(gameTime, graphics);
            }
        }
    }
}
