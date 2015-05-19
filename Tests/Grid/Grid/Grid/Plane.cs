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
        float hexagonSideLength;
        int drawHeight = 2;
        int drawWidth = 5;   

        public Plane(int sidelength, float hexagonsidelength)
        {
            this.sidelength = sidelength;
            hexagonSideLength = hexagonsidelength;
            for (int i = 0; i < sidelength; ++i)
            {
                for (int j = 0; j < sidelength; ++j)
                {
                    Vector2[] neighbors = new Vector2[6];
                    if (i % 2 == 0)
                    {
                        float xValue = (indexNumber.X - 1 + sidelength) % sidelength;
                        float yUpValue = (indexNumber.Y + 1) % sidelength;
                        float yDownValue = (indexNumber.Y - 1 + sidelength) % sidelength;

                        neighbors[0] = new Vector2(indexNumber.X, yUpValue);
                        neighbors[1] = new Vector2(indexNumber.X + 1, indexNumber.Y);
                        neighbors[2] = new Vector2(indexNumber.X + 1, yDownValue);
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
                        float xValue = (indexNumber.X + 1) % sidelength;
                        float yUpValue = (indexNumber.Y + 1) % sidelength;
                        float yDownValue = (indexNumber.Y - 1 + sidelength) % sidelength;

                        neighbors[0] = new Vector2(indexNumber.X, yUpValue);
                        neighbors[1] = new Vector2(xValue, yUpValue);
                        neighbors[2] = new Vector2(xValue, indexNumber.Y);
                        neighbors[3] = new Vector2(indexNumber.X, yDownValue);
                        neighbors[4] = new Vector2(indexNumber.X - 1, indexNumber.Y);
                        neighbors[5] = new Vector2(indexNumber.X - 1, yUpValue);

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
                indexNumber.Y = 0;
                ++indexNumber.X;
            }
        }

        public List<Hexagon> getPlaneHexagons() { return plane; }

        public void Draw(GraphicsDevice graphics, Vector2 indexOfMiddleHexagon)
        {
            /*for (int i = 0; i < plane.Count; ++i)
            {
                plane[i].Draw(gameTime, graphics);
            }*/
            Hexagon middle = plane[(int)(indexOfMiddleHexagon.X * sidelength + indexOfMiddleHexagon.Y)];
            middle.Draw(graphics, middle.get3DPosition());
            rekRightDraw(graphics, middle.getNeighbors()[1], drawWidth, middle.get3DPosition() + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
            rekLeftDraw(graphics, middle.getNeighbors()[5], drawWidth, middle.get3DPosition() + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
            rekUpDraw(graphics, middle.getNeighbors()[0], drawHeight, middle.get3DPosition() + Vector3.UnitY * 1.75f * hexagonSideLength);
            rekDownDraw(graphics, middle.getNeighbors()[3], drawHeight, middle.get3DPosition() - Vector3.UnitY * 1.75f * hexagonSideLength);
        }

        private void rekRightDraw(GraphicsDevice graphics, Vector2 position, int counter, Vector3 drawposition)
        {
            Hexagon me = plane[(int)(position.X * sidelength + position.Y)];
            me.Draw(graphics, drawposition);
            rekUpDraw(graphics, me.getNeighbors()[0], drawHeight, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
            rekDownDraw(graphics, me.getNeighbors()[3], drawHeight, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
            if (counter > 0)
            {
                if (counter % 2 == 0) rekRightDraw(graphics, me.getNeighbors()[1], counter - 1, drawposition + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
                else rekRightDraw(graphics, me.getNeighbors()[2], counter - 1, drawposition + (-Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
            }
        }

        private void rekLeftDraw(GraphicsDevice graphics, Vector2 position, int counter, Vector3 drawposition)
        {
            Hexagon me = plane[(int)(position.X * sidelength + position.Y)];
            me.Draw(graphics, drawposition);
            rekUpDraw(graphics, me.getNeighbors()[0], drawHeight, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
            rekDownDraw(graphics, me.getNeighbors()[3], drawHeight, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
            if (counter > 0)
            {
                if (counter % 2 == 0) rekLeftDraw(graphics, me.getNeighbors()[5], counter - 1, drawposition + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
                else rekLeftDraw(graphics, me.getNeighbors()[4], counter - 1, drawposition + (-Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
            }
        }

        private void rekUpDraw(GraphicsDevice graphics, Vector2 position, int counter, Vector3 drawposition)
        {
            Hexagon me = plane[(int)(position.X * sidelength + position.Y)];
            me.Draw(graphics, drawposition);
            if (counter > 0) rekUpDraw(graphics, me.getNeighbors()[0], counter - 1, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
        }

        private void rekDownDraw(GraphicsDevice graphics, Vector2 position, int counter, Vector3 drawposition)
        {
            Hexagon me = plane[(int)(position.X * sidelength + position.Y)];
            me.Draw(graphics, drawposition);
            if (counter > 0) rekDownDraw(graphics, me.getNeighbors()[3], counter - 1, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
        }
    }
}
