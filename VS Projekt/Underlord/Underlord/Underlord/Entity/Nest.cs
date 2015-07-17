using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Underlord.Logic
{
    class Nest : Thing
    {
        Vars_Func.NestTyp typ;
        Upgrade[] upgrades;
        List<Vector2> upgradePos, nestHexagons, possibleNextNestHexagons;
        float size, nutrition, maxNutrition, growcounter, timeCounter, spawnCounter;
        Boolean undead;
        Vector2 targetPosition, position;

        #region Properties
        public Boolean Undead
        {
            get { return undead; }
            set { undead = value; }
        }
        public Vector2 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public List<Vector2> NestHexagons
        {
            get { return nestHexagons; }
        }
        public Upgrade[] Upgrades
        {
            get { return upgrades; }
        }
        public List<Vector2> UpgradePos
        {
            get { return upgradePos; }
        }
        #endregion

        #region Constructor
        public Nest(Vars_Func.NestTyp typ, Vector2 position, Environment.Hexagon hex, Environment.Map map, Vector2 targetPosition)
        {
            possibleNextNestHexagons = new List<Vector2>();
            nestHexagons = new List<Vector2>();
            nestHexagons.Add(position);
            switch (typ)
            {
                case Vars_Func.NestTyp.Entrance:
                    hex.Building = true;
                    hex.Nest = true;
                    map.Entrances.Add(this);
                    break;
                case Vars_Func.NestTyp.Beetle:
                    hex.Typ = Vars_Func.HexTyp.BeetleNest;
                    hex.Building = true;
                    hex.Nest = true;
                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.Neighbors[i];
                        nestHexagons.Add(neighbor);
                        map.getHexagonAt(neighbor).Typ = Vars_Func.HexTyp.BeetleNest;
                        map.getHexagonAt(neighbor).Building = true;
                        map.getHexagonAt(neighbor).Nest = true;
                        for (int j = 0; j < 6; ++j)
                        {
                            Vector2 nextNeighbor = hex.Neighbors[j];
                            if (!map.getHexagonAt(nextNeighbor).Nest && map.getHexagonAt(nextNeighbor).RoomNumber == map.getHexagonAt(neighbor).RoomNumber && !possibleNextNestHexagons.Contains(nextNeighbor))
                            {
                                possibleNextNestHexagons.Add(nextNeighbor);
                            }
                        }
                    }
                    map.Nests.Add(this);
                    break;
            }
            this.typ = typ;
            this.position = position;
            this.targetPosition = targetPosition;
            upgradePos = new List<Vector2>();
            undead = false;
            size = 1;
            maxNutrition = 450f;
            nutrition = 250f;
            hex.Obj = this;
            thingTyp = Vars_Func.ThingTyp.Nest;
        }
        #endregion

        override public void update(GameTime gameTime, Environment.Map map)
        {
            timeCounter += gameTime.ElapsedGameTime.Milliseconds;
            spawnCounter += gameTime.ElapsedGameTime.Milliseconds;
            //update a nest
            if (this.typ != Vars_Func.NestTyp.Entrance)
            {
                if (timeCounter > 1000)
                {
                    if (possibleNextNestHexagons.Count != 0)
                    {
                        Random rand = new Random();
                        int tmp = rand.Next(possibleNextNestHexagons.Count);
                        Vector2 pos = possibleNextNestHexagons[tmp];
                        nestHexagons.Add(pos);
                        Environment.Hexagon hex = map.getHexagonAt(pos);
                        hex.Typ = Vars_Func.HexTyp.BeetleNest;
                        hex.Nest = true;
                        for (int i = 0; i < 6; ++i)
                        {
                            Vector2 nextNeighbor = map.getHexagonAt(pos).Neighbors[i];
                            if (!map.getHexagonAt(nextNeighbor).Nest && map.getHexagonAt(nextNeighbor).RoomNumber == hex.RoomNumber && !possibleNextNestHexagons.Contains(nextNeighbor))
                            {
                                possibleNextNestHexagons.Add(nextNeighbor);
                            }
                        }
                        possibleNextNestHexagons.RemoveAt(tmp);
                    }
                    timeCounter = 0;
                }
                //timer to spawn creatures
                if (spawnCounter > 10000)
                {
                    spawnCreature(map);
                    spawnCounter = 0;
                }
            }
            //update an entrance
            else
            {
                //timer to spawn heroes
                if (spawnCounter > 5000)
                {
                    spawnCreature(map);
                    spawnCounter = 0;
                }
            }
        }

        override public void DrawModel(Renderer.Camera camera, Vector3 drawPosition, Color drawColor)
        {
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(1) *
            Matrix.CreateRotationX(0) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            Logic.Vars_Func.getNestModell(typ).Color = drawColor;
            Logic.Vars_Func.getNestModell(typ).Draw(camera, modelMatrix);
        }

        public void spawnCreature(Environment.Map map)
        {
            Vector2 tmp = map.getHexagonAt(this.position).Neighbors[3];
            Queue<Vector2> queue = new Queue<Vector2>();
            switch(typ)
            {
                case Vars_Func.NestTyp.Entrance:
                    //find free position for the new creature through a broad-first-search
                    queue.Enqueue(tmp);
                    map.getHexagonAt(tmp).Visited = true;
                    while (queue.Count != 0)
                    {
                        tmp = queue.Dequeue();
                        if (map.getHexagonAt(tmp).Obj == null) break;
                        //for all neighbors

                        for (int i = 0; i < 6; ++i)
                        {
                            Vector2 neighbor = map.getHexagonAt(tmp).Neighbors[i];
                            //which weren't visited already
                            if (map.getHexagonAt(neighbor).Visited == false)
                            {
                                map.getHexagonAt(neighbor).Visited = true; //set visited at true
                                queue.Enqueue(neighbor); //add the neighbor to the queue
                            }
                        }
                    }
                    //set visited for all hexagon at false (for the next use of searching)
                    foreach (Environment.Hexagon hex in map.getMapHexagons())
                    {
                        if (hex.Visited == true) hex.Visited = false;
                    }
                    new Creature(Vars_Func.CreatureTyp.Knight, tmp, this, Vars_Func.ThingTyp.HeroCreature, map);
                    break;
                case Vars_Func.NestTyp.Beetle:
                    //find free position for the new creature through a broad-first-search
                    queue.Enqueue(tmp);
                    map.getHexagonAt(tmp).Visited = true;
                    while (queue.Count != 0)
                    {
                        tmp = queue.Dequeue();
                        if (map.getHexagonAt(tmp).Obj == null) break;
                        //for all neighbors
                        
                        for (int i = 0; i < 6; ++i)
                        {
                            Vector2 neighbor = map.getHexagonAt(tmp).Neighbors[i];
                            //which weren't visited already
                            if (map.getHexagonAt(neighbor).Visited == false)
                            {
                                map.getHexagonAt(neighbor).Visited = true; //set visited at true
                                queue.Enqueue(neighbor); //add the neighbor to the queue
                            }
                        }
                    }
                    //set visited for all hexagon at false (for the next use of searching)
                    foreach (Environment.Hexagon hex in map.getMapHexagons())
                    {
                        if (hex.Visited == true) hex.Visited = false;
                    }
                    new Creature(Vars_Func.CreatureTyp.Beetle, tmp, this, Vars_Func.ThingTyp.DungeonCreature, map);
                    break;
            }
        }

        public void addUpgrade(Upgrade upgrade, Vector2 upgradePosition)
        {
            //upgrades.Add(upgrade);
            upgradePos.Add(upgradePosition);
        }
        public void increaseNutrition(float d)
        {
            if (nutrition + d <= maxNutrition)
            {
                nutrition += d;
            }
            else
            {
                nutrition = maxNutrition;
            }
        }
        public void decreaseNutrition(float d)
        {
            nutrition -= d;

            if (nutrition <= 0)
            {
                nutrition = 0;
                Undead = true;
            }
        }
    }
}
