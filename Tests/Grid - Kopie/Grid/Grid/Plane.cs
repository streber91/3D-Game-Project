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
        int sidelength;
        float hexagonSideLength;
        int drawHeight = 2;
        int drawWidth = 5;   

        public Plane(int sidelength, float hexagonsidelength, Model model)
        {
            Vector2 indexNumber = new Vector2(0, 0);
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

                        plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength, j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Green, indexNumber, neighbors, model));
                        /*if (i >= 5 && j >= 5) plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Blue, indexNumber, neighbors, model));
                        else if (i >= 5) plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Green, indexNumber, neighbors, model));
                        else if (j >= 5) plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Red, indexNumber, neighbors, model));
                        else plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Yellow, indexNumber, neighbors, model));*/
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

                        plane.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonsidelength + hexagonsidelength, j * 2 * hexagonsidelength * 7 / 8 + hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Green, indexNumber, neighbors, model));
                        /*if (i >= 5 && j >= 5) plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Blue, indexNumber, neighbors, model));
                        else if (i >= 5) plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Green, indexNumber, neighbors, model));
                        else if (j >= 5) plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Red, indexNumber, neighbors, model));
                        else plane.Add(new Hexagon(new Vector3((i - 1) * 3 / 2 * hexagonsidelength + 2.5f * hexagonsidelength,
                                                                    j * 2 * hexagonsidelength * 7 / 8 + 2 * hexagonsidelength * 7 / 8, 0), hexagonsidelength, Color.Yellow, indexNumber, neighbors, model));*/
                    }
                    ++indexNumber.Y;
     ;          }
                indexNumber.Y = 0;
                ++indexNumber.X;
            }
        }

        public List<Hexagon> getPlaneHexagons() { return plane; }
        public Hexagon getHexagonAt(float X, float Y) { return plane[(int)(X * sidelength + Y)]; }

        public void Draw(GraphicsDevice graphics, Vector2 indexOfMiddleHexagon, Vector3 cameraTarget)
        {
            /*for (int i = 0; i < plane.Count; ++i)
            {
                plane[i].Draw(gameTime, graphics);
            }*/
            Hexagon middle = plane[(int)(indexOfMiddleHexagon.X * sidelength + indexOfMiddleHexagon.Y)];
            Vector3 middleDrawPosition = middle.get3DPosition();
            if (middleDrawPosition.X - cameraTarget.X <= 1.5f * hexagonSideLength) middleDrawPosition += Vector3.UnitX * 1.5f * hexagonSideLength * sidelength;
            if (middleDrawPosition.X - cameraTarget.X >= 1.5f * hexagonSideLength) middleDrawPosition -= Vector3.UnitX * 1.5f * hexagonSideLength * sidelength;
            if (middleDrawPosition.Y - cameraTarget.Y <= 1.5f * hexagonSideLength) middleDrawPosition += Vector3.UnitY * 1.75f * hexagonSideLength * sidelength;
            if (middleDrawPosition.Y - cameraTarget.Y >= 1.5f * hexagonSideLength) middleDrawPosition -= Vector3.UnitY * 1.75f * hexagonSideLength * sidelength;

            middle.Draw(graphics, middleDrawPosition);
            rekRightDraw(graphics, middle.getNeighbors()[1], drawWidth, middleDrawPosition + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
            rekLeftDraw(graphics, middle.getNeighbors()[5], drawWidth, middleDrawPosition + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
            rekUpDraw(graphics, middle.getNeighbors()[0], drawHeight, middleDrawPosition + Vector3.UnitY * 1.75f * hexagonSideLength);
            rekDownDraw(graphics, middle.getNeighbors()[3], drawHeight, middleDrawPosition - Vector3.UnitY * 1.75f * hexagonSideLength);
        }

        public void DrawModel(Camera camera, Vector2 indexOfMiddleHexagon, Vector3 cameraTarget)
        {
            /*for (int i = 0; i < plane.Count; ++i)
            {
                plane[i].Draw(gameTime, graphics);
            }*/
            Hexagon middle = plane[(int)(indexOfMiddleHexagon.X * sidelength + indexOfMiddleHexagon.Y)];
            Vector3 middleDrawPosition = middle.get3DPosition();
            if (middleDrawPosition.X - cameraTarget.X <= 1.5f * hexagonSideLength) middleDrawPosition += Vector3.UnitX * 1.5f * hexagonSideLength * sidelength;
            if (middleDrawPosition.X - cameraTarget.X >= 1.5f * hexagonSideLength) middleDrawPosition -= Vector3.UnitX * 1.5f * hexagonSideLength * sidelength;
            if (middleDrawPosition.Y - cameraTarget.Y <= 1.5f * hexagonSideLength) middleDrawPosition += Vector3.UnitY * 1.75f * hexagonSideLength * sidelength;
            if (middleDrawPosition.Y - cameraTarget.Y >= 1.5f * hexagonSideLength) middleDrawPosition -= Vector3.UnitY * 1.75f * hexagonSideLength * sidelength;

            middle.DrawModel(camera, middleDrawPosition);
            rekRightDrawModel(camera, middle.getNeighbors()[1], drawWidth, middleDrawPosition + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
            rekLeftDrawModel(camera, middle.getNeighbors()[5], drawWidth, middleDrawPosition + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
            rekUpDrawModel(camera, middle.getNeighbors()[0], drawHeight, middleDrawPosition + Vector3.UnitY * 1.75f * hexagonSideLength);
            rekDownDrawModel(camera, middle.getNeighbors()[3], drawHeight, middleDrawPosition - Vector3.UnitY * 1.75f * hexagonSideLength);
        }

        private void rekRightDrawModel(Camera camera, Vector2 position, int counter, Vector3 drawposition)
        {
            Hexagon me = plane[(int)(position.X * sidelength + position.Y)];
            me.DrawModel(camera, drawposition);
            rekUpDrawModel(camera, me.getNeighbors()[0], drawHeight, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
            rekDownDrawModel(camera, me.getNeighbors()[3], drawHeight, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
            if (counter > 0)
            {
                if (counter % 2 == 0) rekRightDrawModel(camera, me.getNeighbors()[1], counter - 1, drawposition + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
                else rekRightDrawModel(camera, me.getNeighbors()[2], counter - 1, drawposition + (-Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
            }
        }

        private void rekLeftDrawModel(Camera camera, Vector2 position, int counter, Vector3 drawposition)
        {
            Hexagon me = plane[(int)(position.X * sidelength + position.Y)];
            me.DrawModel(camera, drawposition);
            rekUpDrawModel(camera, me.getNeighbors()[0], drawHeight, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
            rekDownDrawModel(camera, me.getNeighbors()[3], drawHeight, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
            if (counter > 0)
            {
                if (counter % 2 == 0) rekLeftDrawModel(camera, me.getNeighbors()[5], counter - 1, drawposition + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
                else rekLeftDrawModel(camera, me.getNeighbors()[4], counter - 1, drawposition + (-Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
            }
        }

        private void rekUpDrawModel(Camera camera, Vector2 position, int counter, Vector3 drawposition)
        {
            Hexagon me = plane[(int)(position.X * sidelength + position.Y)];
            me.DrawModel(camera, drawposition);
            if (counter > 0) rekUpDrawModel(camera, me.getNeighbors()[0], counter - 1, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
        }

        private void rekDownDrawModel(Camera camera, Vector2 position, int counter, Vector3 drawposition)
        {
            Hexagon me = plane[(int)(position.X * sidelength + position.Y)];
            me.DrawModel(camera, drawposition);
            if (counter > 0) rekDownDrawModel(camera, me.getNeighbors()[3], counter - 1, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
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
