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
        Vector2 indexNumber = new Vector2(0,0);
        int sidelength;

        public Plane(int sidelength, float hexagonsidelength)
        {
            this.sidelength = sidelength;
            for (int i = 0; i < sidelength; ++i)
            {
                for (int j = 0; j < sidelength; ++j)
                {
                    Vector2[] neighbors = new Vector2[6];
                    if (i % 2 == 0)
                    {
                        float xValue = (--indexNumber.X + sidelength) % sidelength;
                        float yUpValue = ++indexNumber.Y % sidelength;
                        float yDownValue = (--indexNumber.Y + sidelength) % sidelength;

                        neighbors[0] = new Vector2(indexNumber.X, yUpValue);
                        neighbors[1] = new Vector2(++indexNumber.X, indexNumber.Y);
                        neighbors[2] = new Vector2(++indexNumber.X, yDownValue);
                        neighbors[3] = new Vector2(indexNumber.X, yDownValue);
                        neighbors[4] = new Vector2(xValue, yDownValue);
                        neighbors[5] = new Vector2(xValue, indexNumber.Y);

                        if (i >= 5 && j >= 5) plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Blue, indexNumber, neighbors));
                        else if (i >= 5) plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Green, indexNumber, neighbors));
                        else if (j >= 5) plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Red, indexNumber, neighbors));
                        else plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Yellow, indexNumber, neighbors));
                    }
                    else
                    {
                        float xValue = ++indexNumber.X % sidelength;
                        float yUpValue = ++indexNumber.Y % sidelength;
                        float yDownValue = (--indexNumber.Y + sidelength) % sidelength;

                        neighbors[0] = new Vector2(indexNumber.X, yUpValue);
                        neighbors[1] = new Vector2(xValue, yUpValue);
                        neighbors[2] = new Vector2(xValue, indexNumber.Y);
                        neighbors[3] = new Vector2(indexNumber.X, yDownValue);
                        neighbors[4] = new Vector2(--indexNumber.X, indexNumber.Y);
                        neighbors[5] = new Vector2(--indexNumber.X, yUpValue);

                        if (i >= 5 && j >= 5) plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Blue, indexNumber, neighbors));
                        else if (i >= 5) plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Green, indexNumber, neighbors));
                        else if (j >= 5) plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Red, indexNumber, neighbors));
                        else plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Yellow, indexNumber, neighbors));
                    }
                    ++indexNumber.Y;
     ;          }
                ++indexNumber.X;
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
