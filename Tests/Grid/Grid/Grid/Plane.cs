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
        int indexNumber = 0;
        int sidelength;

        public Plane(int sidelength, float hexagonsidelength)
        {
            this.sidelength = sidelength;
            for (int i = 0; i < sidelength; ++i)
            {
                for (int j = 0; j < sidelength; ++j)
                {
                    /*plane.Add(new Hexagon(new Microsoft.Xna.Framework.Vector3(i * (hexagonsidelength + hexagonsidelength * (float)Math.Sqrt(2)) + hexagonsidelength / 2 + hexagonsidelength / (float)Math.Sqrt(2),
                                                                              hexagonsidelength / (float)Math.Sqrt(2), 0), hexagonsidelength, Color.White));*/

                    /*if (j % 2 == 0) plane.Add(new Hexagon(new Vector3(i * 3 * hexagonsidelength + hexagonsidelength,
                                                                        j * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.White));
                    else plane.Add(new Hexagon(new Vector3(i * 3 * hexagonsidelength + 2.5f*hexagonsidelength,
                                                           j * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.White));*/

                    if (i % 2 == 0)
                    {
                        if (i >= 5 && j >= 5)
                        {
                            plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                   j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Blue, indexNumber));
                            ++indexNumber;
                        }
                        else if (i >= 5)
                        {
                            plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                   j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Green, indexNumber));
                            ++indexNumber;
                        }
                        else if (j >= 5)
                        {
                            plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                   j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Red, indexNumber));
                            ++indexNumber;
                        }
                        else
                        {
                            plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                   j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Yellow, indexNumber));
                            ++indexNumber;
                        }
                    }
                    else
                    {
                        if (i >= 5 && j >= 5)
                        {
                            plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                   j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Blue, indexNumber));
                            ++indexNumber;
                        }
                        else if (i >= 5)
                        {
                            plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                   j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Green, indexNumber));
                            ++indexNumber;
                        }
                        else if (j >= 5)
                        {
                            plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                   j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Red, indexNumber));
                            ++indexNumber;
                        }
                        else
                        {
                            plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                   j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Yellow, indexNumber));
                            ++indexNumber;
                        }
                    }
     ;          }
            }
        }

        public int getSideLength() { return sidelength; }

        public List<Hexagon> getPlaneHexagons() { return plane; }

        public void Draw(GameTime gameTime, GraphicsDevice graphics)
        {
            for (int i = 0; i < plane.Count; ++i)
            {
                plane[i].Draw(gameTime, graphics);
            }
        }
    }
}
