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
        List<Room> rooms = new List<Room>();
        List<Nest> nests = new List<Nest>();
        List<Hexagon> map = new List<Hexagon>();
        int planeSidelength; // drawWidth, drawHeight, planeside = map size
        float hexagonSideLength;

        #region Properties
        public List<Room> Rooms
        {
            get { return rooms; }
        }
        public List<Nest> Nests
        {
            get { return nests; }
        }
        #endregion

        #region Constructor
        public Map(int sidelength, Entity.Vars_Func.HexTyp typ, Boolean newGame, float hexagonSideLength)
        {
            //drawHeight = 2; //how many hexagons are drawn up and down of the middle (+1)
            //drawWidth = 5; //how many hexagons are drawn left and right of the middle (+1)
            this.hexagonSideLength = hexagonSideLength;
            if (newGame)
            {
                Vector2 indexNumber = new Vector2(0, 0);
                this.planeSidelength = sidelength;
                //creates a map with proportions sidelength * sidelength
                for (int i = 0; i < sidelength; ++i)
                {
                    for (int j = 0; j < sidelength; ++j)
                    {
                        Vector2[] neighbors = new Vector2[6];
                        //neighbor indices for i even
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
                            map.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonSideLength + hexagonSideLength * 1.5f, j * 2 * hexagonSideLength * 7 / 8 + hexagonSideLength * 7 / 8, 0), indexNumber, neighbors, typ));
                        }
                        //neighbor indices for i uneven
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
                            map.Add(new Hexagon(new Vector3(i * 3 / 2 * hexagonSideLength + hexagonSideLength * 1.5f, j * 2 * hexagonSideLength * 7 / 8 + hexagonSideLength * 7 / 8 * 2, 0), indexNumber, neighbors, typ));
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
        #endregion

        public List<Hexagon> getMapHexagons() { return map; }
        public Hexagon getHexagonAt(float X, float Y) { return map[(int)(X * planeSidelength + Y)]; }
        public Hexagon getHexagonAt(Vector2 pos) { return map[(int)(pos.X * planeSidelength + pos.Y)]; }
        public int getPlanelength() { return planeSidelength; }
        //TODO implement list for things AND implement konsistens in all remove and constructors
        //TODO
        public void saveGame()
        {

        }
        //TODO
        public void move(Imp imp)
        {

        }

        public void move(Creature creature)
        {
            if (getHexagonAt(creature.Path.Peek()) == null)
            {
                getHexagonAt(creature.Position).Obj = null;
                creature.Position = creature.Path.Pop();
                getHexagonAt(creature.Position).Obj = creature;
                creature.ActionTimeCounter -= 1000 / creature.getSpeed();
            }
        }
        //TODO
        public void remove(Thing thing)
        {
            // delet thing from list
            // determine position and set map[x][y] = null
        }
        //TODO
        //private void loadSavegame(File savegame)
        //{

        //}

        public void DrawModel(Camera camera, Vector2 indexOfMiddleHexagon, Vector3 cameraTarget, int drawWidth)
        {
            Vector2 tmp = indexOfMiddleHexagon;
            Vector3 drawposition = getHexagonAt(indexOfMiddleHexagon).get3DPosition();
            if (drawposition.X - cameraTarget.X <= 1.5f * hexagonSideLength) drawposition += Vector3.UnitX * 1.5f * hexagonSideLength * planeSidelength;
            if (drawposition.X - cameraTarget.X >= 1.5f * hexagonSideLength) drawposition -= Vector3.UnitX * 1.5f * hexagonSideLength * planeSidelength;
            if (drawposition.Y - cameraTarget.Y <= 1.5f * hexagonSideLength) drawposition += Vector3.UnitY * 1.75f * hexagonSideLength * planeSidelength;
            if (drawposition.Y - cameraTarget.Y >= 1.5f * hexagonSideLength) drawposition -= Vector3.UnitY * 1.75f * hexagonSideLength * planeSidelength;

            //breadth-first search
            Queue<Vector2> queue = new Queue<Vector2>();
            Queue<Vector3> vecQueue = new Queue<Vector3>();
            queue.Enqueue(tmp);
            vecQueue.Enqueue(drawposition);
            getHexagonAt(tmp).Visited = true;
            //enqueue dummys
            queue.Enqueue(new Vector2(getPlanelength() + drawWidth, 0));
            vecQueue.Enqueue(new Vector3(getPlanelength() + drawWidth, 0, 0));

            while (queue.Count != 1)
            {
                tmp = queue.Dequeue();
                drawposition = vecQueue.Dequeue();

                if (tmp.X >= getPlanelength())
                {
                    // stop if drawWidth is reached
                    if (tmp.X <= getPlanelength()) break;

                    queue.Enqueue(new Vector2(tmp.X - 1, 0));
                    vecQueue.Enqueue(new Vector3(tmp.X - 1, 0, 0));
                    continue;
                }

                //draw the Hexagon
                getHexagonAt(tmp).DrawModel(camera, drawposition);

                //enqueue neigbors
                if (!getHexagonAt(getHexagonAt(tmp).getNeighbors()[0]).Visited)
                {
                    getHexagonAt(getHexagonAt(tmp).getNeighbors()[0]).Visited = true;
                    queue.Enqueue(getHexagonAt(tmp).getNeighbors()[0]);
                    vecQueue.Enqueue(drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
                }
                if (!getHexagonAt(getHexagonAt(tmp).getNeighbors()[1]).Visited)
                {
                    getHexagonAt(getHexagonAt(tmp).getNeighbors()[1]).Visited = true;
                    queue.Enqueue(getHexagonAt(tmp).getNeighbors()[1]);
                    vecQueue.Enqueue(drawposition + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
                }
                if (!getHexagonAt(getHexagonAt(tmp).getNeighbors()[2]).Visited)
                {
                    getHexagonAt(getHexagonAt(tmp).getNeighbors()[2]).Visited = true;
                    queue.Enqueue(getHexagonAt(tmp).getNeighbors()[2]);
                    vecQueue.Enqueue(drawposition + (-Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
                }
                if (!getHexagonAt(getHexagonAt(tmp).getNeighbors()[3]).Visited)
                {
                    getHexagonAt(getHexagonAt(tmp).getNeighbors()[3]).Visited = true;
                    queue.Enqueue(getHexagonAt(tmp).getNeighbors()[3]);
                    vecQueue.Enqueue(drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
                }
                if (!getHexagonAt(getHexagonAt(tmp).getNeighbors()[4]).Visited)
                {
                    getHexagonAt(getHexagonAt(tmp).getNeighbors()[4]).Visited = true;
                    queue.Enqueue(getHexagonAt(tmp).getNeighbors()[4]);
                    vecQueue.Enqueue(drawposition + (-Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
                }
                if (!getHexagonAt(getHexagonAt(tmp).getNeighbors()[5]).Visited)
                {
                    getHexagonAt(getHexagonAt(tmp).getNeighbors()[5]).Visited = true;
                    queue.Enqueue(getHexagonAt(tmp).getNeighbors()[5]);
                    vecQueue.Enqueue(drawposition + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
                }
            }

            //clear Hexmap for next search
            for (int i = 0; i < getPlanelength(); ++i)
            {
                for (int j = 0; j < getPlanelength(); ++j)
                {
                    getHexagonAt(i, j).Visited = false;
                }
            }
        }

        #region trydraw
        //public void DrawModel(Camera camera, Vector2 indexOfMiddleHexagon, Vector3 cameraTarget, int drawWidth)
        //{
        //    if (drawWidth > 0)
        //    {
        //        Hexagon tmp = this.getHexagonAt(indexOfMiddleHexagon);
        //        Vector3 drawposition = tmp.get3DPosition();
        //        if (drawposition.X - cameraTarget.X <= 1.5f * hexagonSideLength) drawposition += Vector3.UnitX * 1.5f * hexagonSideLength * planeSidelength;
        //        if (drawposition.X - cameraTarget.X >= 1.5f * hexagonSideLength) drawposition -= Vector3.UnitX * 1.5f * hexagonSideLength * planeSidelength;
        //        if (drawposition.Y - cameraTarget.Y <= 1.5f * hexagonSideLength) drawposition += Vector3.UnitY * 1.75f * hexagonSideLength * planeSidelength;
        //        if (drawposition.Y - cameraTarget.Y >= 1.5f * hexagonSideLength) drawposition -= Vector3.UnitY * 1.75f * hexagonSideLength * planeSidelength;
        //        //draw first hexagon and all neigbors
        //        tmp.DrawModel(camera, drawposition);
        //        recDrawModel(camera, tmp.getNeighbors()[0], drawposition + Vector3.UnitY * 1.75f * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[1], drawposition + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[2], drawposition + (-Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[3], drawposition - Vector3.UnitY * 1.75f * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[4], drawposition + (-Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[5], drawposition + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength, drawWidth - 1);

        //        foreach (Hexagon hex in this.map)
        //        {
        //            hex.AllreadyDrawn = false;
        //        }
        //    }
        //}
        //private void recDrawModel(Camera camera, Vector2 indexOfMiddleHexagon, Vector3 drawposition, int drawWidth)
        //{
        //    if (drawWidth > 0)
        //    {
        //        Hexagon tmp = this.getHexagonAt(indexOfMiddleHexagon);
        //        //draw hexagon that hasen't drawn yet
        //        if(!tmp.AllreadyDrawn) tmp.DrawModel(camera, drawposition);
        //        //draws all Neigbors
        //        recDrawModel(camera, tmp.getNeighbors()[0], drawposition + Vector3.UnitY * 1.75f * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[1], drawposition + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[2], drawposition + (-Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[3], drawposition - Vector3.UnitY * 1.75f * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[4], drawposition + (-Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength, drawWidth - 1);
        //        recDrawModel(camera, tmp.getNeighbors()[5], drawposition + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength, drawWidth - 1);

        //    }
        //}
        #endregion

        #region olddraw
        //public void DrawModel(Camera camera, Vector2 indexOfMiddleHexagon, Vector3 cameraTarget)
        //{
        //    Hexagon middle = map[(int)(indexOfMiddleHexagon.X * planeSidelength + indexOfMiddleHexagon.Y)];
        //    Vector3 middleDrawPosition = middle.get3DPosition();
        //    if (middleDrawPosition.X - cameraTarget.X <= 1.5f * hexagonSideLength) middleDrawPosition += Vector3.UnitX * 1.5f * hexagonSideLength * planeSidelength;
        //    if (middleDrawPosition.X - cameraTarget.X >= 1.5f * hexagonSideLength) middleDrawPosition -= Vector3.UnitX * 1.5f * hexagonSideLength * planeSidelength;
        //    if (middleDrawPosition.Y - cameraTarget.Y <= 1.5f * hexagonSideLength) middleDrawPosition += Vector3.UnitY * 1.75f * hexagonSideLength * planeSidelength;
        //    if (middleDrawPosition.Y - cameraTarget.Y >= 1.5f * hexagonSideLength) middleDrawPosition -= Vector3.UnitY * 1.75f * hexagonSideLength * planeSidelength;

        //    middle.DrawModel(camera, middleDrawPosition);
        //    rekRightDrawModel(camera, middle.getNeighbors()[1], drawWidth, middleDrawPosition + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
        //    rekLeftDrawModel(camera, middle.getNeighbors()[5], drawWidth, middleDrawPosition + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
        //    rekUpDrawModel(camera, middle.getNeighbors()[0], drawHeight, middleDrawPosition + Vector3.UnitY * 1.75f * hexagonSideLength);
        //    rekDownDrawModel(camera, middle.getNeighbors()[3], drawHeight, middleDrawPosition - Vector3.UnitY * 1.75f * hexagonSideLength);
        //}

        //private void rekRightDrawModel(Camera camera, Vector2 position, int counter, Vector3 drawposition)
        //{
        //    Hexagon me = map[(int)(position.X * planeSidelength + position.Y)];
        //    me.DrawModel(camera, drawposition);
        //    rekUpDrawModel(camera, me.getNeighbors()[0], drawHeight, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
        //    rekDownDrawModel(camera, me.getNeighbors()[3], drawHeight, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
        //    if (counter > 0)
        //    {
        //        if (counter % 2 == 0) rekRightDrawModel(camera, me.getNeighbors()[1], counter - 1, drawposition + (Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
        //        else rekRightDrawModel(camera, me.getNeighbors()[2], counter - 1, drawposition + (-Vector3.UnitY * 0.875f + Vector3.UnitX * 1.5f) * hexagonSideLength);
        //    }
        //}

        //private void rekLeftDrawModel(Camera camera, Vector2 position, int counter, Vector3 drawposition)
        //{
        //    Hexagon me = map[(int)(position.X * planeSidelength + position.Y)];
        //    me.DrawModel(camera, drawposition);
        //    rekUpDrawModel(camera, me.getNeighbors()[0], drawHeight, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
        //    rekDownDrawModel(camera, me.getNeighbors()[3], drawHeight, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
        //    if (counter > 0)
        //    {
        //        if (counter % 2 == 0) rekLeftDrawModel(camera, me.getNeighbors()[5], counter - 1, drawposition + (Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
        //        else rekLeftDrawModel(camera, me.getNeighbors()[4], counter - 1, drawposition + (-Vector3.UnitY * 0.875f - Vector3.UnitX * 1.5f) * hexagonSideLength);
        //    }
        //}

        //private void rekUpDrawModel(Camera camera, Vector2 position, int counter, Vector3 drawposition)
        //{
        //    Hexagon me = map[(int)(position.X * planeSidelength + position.Y)];
        //    me.DrawModel(camera, drawposition);
        //    if (counter > 0) rekUpDrawModel(camera, me.getNeighbors()[0], counter - 1, drawposition + Vector3.UnitY * 1.75f * hexagonSideLength);
        //}

        //private void rekDownDrawModel(Camera camera, Vector2 position, int counter, Vector3 drawposition)
        //{
        //    Hexagon me = map[(int)(position.X * planeSidelength + position.Y)];
        //    me.DrawModel(camera, drawposition);
        //    if (counter > 0) rekDownDrawModel(camera, me.getNeighbors()[3], counter - 1, drawposition - Vector3.UnitY * 1.75f * hexagonSideLength);
        //}
        #endregion

        public void update(GameTime gameTime, float timeSinceLastUpdate)
        {
            foreach (Nest n in nests)
            {
                n.update(gameTime, this);
            }
        }
    }
}
