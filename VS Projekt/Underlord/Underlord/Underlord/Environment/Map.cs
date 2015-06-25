using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Underlord.Entity;
using Underlord.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Underlord.Environment
{
    class Map
    {
        //String name = "";
        List<Room> rooms = new List<Room>();
        List<Hexagon> map = new List<Hexagon>();
        int sidelength, drawHeight, drawWidth;
        float hexagonSideLength;

        public Map(int sidelength, Model model, Boolean newGame, float hexagonSideLength)
        {
            drawHeight = 2;
            drawWidth = 5;
            this.hexagonSideLength = hexagonSideLength;
            if (newGame)
            {
                Vector2 indexNumber = new Vector2(0, 0);
                this.sidelength = sidelength;
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
                            map.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonSideLength + hexagonSideLength * 1.5f, j * 2 * hexagonSideLength * 7 / 8 + hexagonSideLength * 7 / 8, 0), indexNumber, neighbors, model));
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
                            map.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonSideLength + hexagonSideLength * 1.5f, j * 2 * hexagonSideLength * 7 / 8 + hexagonSideLength * 7 / 8 * 2, 0), indexNumber, neighbors, model));
                        }
                        
                        ++indexNumber.Y;
                        ;
                    }
                    indexNumber.Y = 0;
                    ++indexNumber.X;
                }
            }
            else
            {
                // insert the savegame here.
                //load(null);
            }
        }

        #region Properties
        public List<Room> Rooms
        {
            get { return rooms; }
        }
        #endregion

        public List<Hexagon> getMapHexagons() { return map; }
        public Hexagon getHexagonAt(float X, float Y) { return map[(int)(X * sidelength + Y)]; }




        public void saveGame()
        {
        }

        public void move(Creature crea)
        {
        }
        public void remove(Thing thing)
        {
            // determine position and set map[x][y] = null
        }

        /*private void loadSavegame(File savegame)
        {

        }*/





        public void DrawModel(Camera camera, Vector2 indexOfMiddleHexagon, Vector3 cameraTarget)
        {
            Hexagon middle = map[(int)(indexOfMiddleHexagon.X * sidelength + indexOfMiddleHexagon.Y)];
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
            Hexagon me = map[(int)(position.X * sidelength + position.Y)];
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
            Hexagon me = map[(int)(position.X * sidelength + position.Y)];
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
            Hexagon me = map[(int)(position.X * sidelength + position.Y)];
            me.DrawModel(camera, drawposition);
            if (counter > 0) rekUpDrawModel(camera, me.getNeighbors()[0], counter - 1, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
        }

        private void rekDownDrawModel(Camera camera, Vector2 position, int counter, Vector3 drawposition)
        {
            Hexagon me = map[(int)(position.X * sidelength + position.Y)];
            me.DrawModel(camera, drawposition);
            if (counter > 0) rekDownDrawModel(camera, me.getNeighbors()[3], counter - 1, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
        }
    }
}
