using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Underlord.Logic;

namespace Underlord.Entity
{
    class Nest : Thing
    {
        Vars_Func.NestTyp typ;
        List<Upgrade> upgrades;
        int[] upgradeCount = new int[3];
        List<Vector2> nestHexagons, possibleNextNestHexagons;
        float nutrition, maxNutrition, growCounter, spawnCounter, foodCounter, rotationSpeed;
        int food, nextUpgradeCost;
        Boolean getsFeeded, getsHarvested;
        Vector2 targetPosition, position;

        #region Properties
        public int NextUpgradeCost
        {
            get { return nextUpgradeCost; }
            set { nextUpgradeCost = value; }
        }
        public List<Vector2> PossibleNextNestHexagons
        {
            get { return possibleNextNestHexagons; }
        }
        public Vars_Func.NestTyp Typ
        {
            get { return typ; }
            set { typ = value; }
        }
        public float Nutrition
        {
            get { return nutrition; }
            set { nutrition = value; }
        }
        public float MaxNutrition
        {
            get { return maxNutrition; }
            set { maxNutrition = value; }
        }
        public Boolean GetsFedded
        {
            get { return getsFeeded; }
            set { getsFeeded = value; }
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
        public int[] UpgradeCount
        {
            get { return upgradeCount; }
        }
        public int Food
        {
            get { return food; }
            set { food = value; }
        }
        public Boolean GetsHarvested
        {
            get { return getsHarvested; }
            set { getsHarvested = value; }
        }
        #endregion

        #region Constructor
        public Nest(Vars_Func.NestTyp typ, Vector2 position, Environment.Map map, Vector2 targetPosition)
        {
            nextUpgradeCost = 100;
            Environment.Hexagon hex = map.getHexagonAt(position);
            if (typ != Vars_Func.NestTyp.Entrance)
            {
                map.Rooms.ElementAt(map.getHexagonAt(position).RoomNumber - 1).NestType = typ;
                map.Rooms.ElementAt(map.getHexagonAt(position).RoomNumber - 1).RoomObject = this;
            }
            possibleNextNestHexagons = new List<Vector2>();
            nestHexagons = new List<Vector2>();
            nestHexagons.Add(position);
            switch (typ)
            {
                case Vars_Func.NestTyp.Entrance:
                    hex.Building = true;
                    hex.Nest = true;
                    map.Entrances.Add(this);
                    hex.IsEntrance = true;
                    hex.EnlightendHexagon(map);
                    map.Light = Vars_Func.getEntranceRayModel();
                    thingTyp = Vars_Func.ThingTyp.Nest;
                    break;

                case Vars_Func.NestTyp.Beetle:
                    hex.Typ = Vars_Func.HexTyp.BeetleNest;
                    hex.Building = true;
                    hex.Nest = true;
                    getsFeeded = false;
                    maxNutrition = 500f;
                    nutrition = 0f;
                    upgrades = new List<Upgrade>();
                    thingTyp = Vars_Func.ThingTyp.Nest;

                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.Neighbors[i];
                        nestHexagons.Add(neighbor);
                        map.getHexagonAt(neighbor).Typ = Vars_Func.HexTyp.BeetleNest;
                        map.getHexagonAt(neighbor).Building = true;
                        map.getHexagonAt(neighbor).Nest = true;
                    }
                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.Neighbors[i];
                        for (int j = 0; j < 6; ++j)
                        {
                            Vector2 nextNeighbor = hex.Neighbors[j];
                            if (!map.getHexagonAt(nextNeighbor).Nest &&
                                map.getHexagonAt(nextNeighbor).RoomNumber == map.getHexagonAt(neighbor).RoomNumber &&
                                !possibleNextNestHexagons.Contains(nextNeighbor))
                            {
                                possibleNextNestHexagons.Add(nextNeighbor);
                            }
                        }
                    }
                    map.Nests.Add(this);
                    break;

                case Vars_Func.NestTyp.Skeleton:
                    hex.Typ = Vars_Func.HexTyp.Graveyard;
                    hex.Building = true;
                    hex.Nest = true;
                    getsFeeded = false;
                    maxNutrition = 500f;
                    nutrition = 0f;
                    upgrades = new List<Upgrade>();
                    thingTyp = Vars_Func.ThingTyp.Nest;

                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.Neighbors[i];
                        nestHexagons.Add(neighbor);
                        map.getHexagonAt(neighbor).Typ = Vars_Func.HexTyp.Graveyard;
                        map.getHexagonAt(neighbor).Building = true;
                        map.getHexagonAt(neighbor).Nest = true;
                        map.getHexagonAt(neighbor).GrowObject = Vars_Func.GrowObject.Graveyard;
                    }
                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.Neighbors[i];
                        for (int j = 0; j < 6; ++j)
                        {
                            Vector2 nextNeighbor = hex.Neighbors[j];
                            if (!map.getHexagonAt(nextNeighbor).Nest &&
                                map.getHexagonAt(nextNeighbor).RoomNumber == map.getHexagonAt(neighbor).RoomNumber &&
                                !possibleNextNestHexagons.Contains(nextNeighbor))
                            {
                                possibleNextNestHexagons.Add(nextNeighbor);
                            }
                        }
                    }
                    map.Nests.Add(this);
                    break;

                case Vars_Func.NestTyp.Farm:
                    hex.Typ = Vars_Func.HexTyp.Farm;
                    hex.Building = true;
                    hex.Nest = true;
                    nutrition = 1;
                    maxNutrition = 1;
                    thingTyp = Vars_Func.ThingTyp.Nest;
                    food = 0;
                    getsHarvested = false;
                    foodCounter = 0;

                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.Neighbors[i];
                        nestHexagons.Add(neighbor);
                        map.getHexagonAt(neighbor).Typ = Vars_Func.HexTyp.Farm;
                        map.getHexagonAt(neighbor).Building = true;
                        map.getHexagonAt(neighbor).Nest = true;
                        map.getHexagonAt(neighbor).GrowObject = Vars_Func.GrowObject.Farm;
                    }
                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.Neighbors[i];
                        for (int j = 0; j < 6; ++j)
                        {
                            Vector2 nextNeighbor = hex.Neighbors[j];
                            if (!map.getHexagonAt(nextNeighbor).Nest &&
                                map.getHexagonAt(nextNeighbor).RoomNumber == map.getHexagonAt(neighbor).RoomNumber &&
                                !possibleNextNestHexagons.Contains(nextNeighbor))
                            {
                                possibleNextNestHexagons.Add(nextNeighbor);
                            }
                        }
                    }
                    map.Farms.Add(this);
                    break;

                case Vars_Func.NestTyp.Temple:
                    hex.Typ = Vars_Func.HexTyp.Temple;
                    hex.Building = true;
                    hex.Nest = true;
                    nutrition = 1;
                    maxNutrition = 1;
                    thingTyp = Vars_Func.ThingTyp.Nest;
                    foodCounter = 0;

                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.Neighbors[i];
                        nestHexagons.Add(neighbor);
                        map.getHexagonAt(neighbor).Typ = Vars_Func.HexTyp.Temple;
                        map.getHexagonAt(neighbor).Building = true;
                        map.getHexagonAt(neighbor).Nest = true;
                        map.getHexagonAt(neighbor).GrowObject = Vars_Func.GrowObject.Temple;
                    }
                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.Neighbors[i];
                        for (int j = 0; j < 6; ++j)
                        {
                            Vector2 nextNeighbor = hex.Neighbors[j];
                            if (!map.getHexagonAt(nextNeighbor).Nest &&
                                map.getHexagonAt(nextNeighbor).RoomNumber == map.getHexagonAt(neighbor).RoomNumber &&
                                !possibleNextNestHexagons.Contains(nextNeighbor))
                            {
                                possibleNextNestHexagons.Add(nextNeighbor);
                            }
                        }
                    }
                    map.Temples.Add(this);
                    break;
            }
            this.typ = typ;
            this.position = position;
            this.targetPosition = targetPosition;
            hex.Obj = this;
        }
        #endregion

        override public void update(GameTime gameTime, Environment.Map map)
        {
            map.getHexagonAt(this.position).Obj = this;
            growCounter += gameTime.ElapsedGameTime.Milliseconds;
            spawnCounter += gameTime.ElapsedGameTime.Milliseconds;
            foodCounter += gameTime.ElapsedGameTime.Milliseconds;
            //update a nest
            if (this.typ != Vars_Func.NestTyp.Entrance)
            {
                //timer for growth of the nest
                if (growCounter > 10000 && nutrition > 0)
                {
                    if (possibleNextNestHexagons.Count != 0)
                    {
                        Random rand = new Random();
                        int tmp = rand.Next(possibleNextNestHexagons.Count);
                        Vector2 pos = possibleNextNestHexagons[tmp];
                        nestHexagons.Add(pos);
                        Environment.Hexagon hex = map.getHexagonAt(pos);
                        switch(typ)
                        {
                            case Vars_Func.NestTyp.Beetle:
                                hex.Typ = Vars_Func.HexTyp.BeetleNest;
                                break;
                            case Vars_Func.NestTyp.Skeleton:
                                hex.Typ = Vars_Func.HexTyp.Graveyard;
                                hex.GrowObject = Vars_Func.GrowObject.Graveyard;
                                break;
                            case Vars_Func.NestTyp.Farm:
                                hex.Typ = Vars_Func.HexTyp.Farm;
                                hex.GrowObject = Vars_Func.GrowObject.Farm;
                                break;
                            case Vars_Func.NestTyp.Temple:
                                hex.Typ = Vars_Func.HexTyp.Temple;
                                hex.GrowObject = Vars_Func.GrowObject.Temple;
                                break;
                        }
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
                    growCounter = 0;
                }
                if (this.typ == Vars_Func.NestTyp.Temple)
                {
                    foodCounter += gameTime.ElapsedGameTime.Milliseconds;

                    if (foodCounter > 10000 / this.nestHexagons.Count)
                    {
                        Player.Mana++;
                        foodCounter = 0;
                    }
                }
                else if (this.typ == Vars_Func.NestTyp.Farm)
                {
                    foodCounter += gameTime.ElapsedGameTime.Milliseconds;

                    if (foodCounter > 10000 / this.nestHexagons.Count)
                    {
                        food++;
                        foodCounter = 0;
                        if (food >= 100 && getsHarvested == false)
                        {
                            getsHarvested = true;
                            map.JobsWaiting.Enqueue(new Logic.Job(Logic.Vars_Func.ImpJob.Harvest, position));
                        }
                    } 
                }
                else
                {
                    //timer to decrease the nutrition of the nest
                    if (foodCounter > 1000)
                    {
                        decreaseNutrition(1.0f);
                        foodCounter = 0;
                        if (nutrition < 0.4 * maxNutrition && getsFeeded == false)
                        {
                            getsFeeded = true;
                            map.JobsWaiting.Enqueue(new Job(Vars_Func.ImpJob.Feed, position));
                        }
                    }
                    //timer to spawn creatures
                    if (spawnCounter > Math.Max(10000, (250000 / nestHexagons.Count)) && nutrition > 0)
                    {
                        spawnCreature(map);
                        spawnCounter = 0;
                    }
                }
            }
            ////update an entrance
            //else
            //{
            //    //timer to spawn heroes
            //    if (spawnCounter > 5000)
            //    {
            //        spawnCreature(map);
            //        spawnCounter = 0;
            //    }
            //}
        }

        override public void DrawModel(Renderer.Camera camera, Vector3 drawPosition, Color drawColor, bool isEnlightend, float lightPower)
        {
            drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + Vars_Func.getNestParams(typ).X);

            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(Vars_Func.getNestParams(typ).Y) *
            Matrix.CreateRotationX(Vars_Func.getNestParams(typ).Z) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            Vars_Func.getNestModell(typ).Color = drawColor;
            Vars_Func.getNestModell(typ).Draw(camera, modelMatrix, false, isEnlightend, lightPower/2);


            if (typ == Vars_Func.NestTyp.Farm)
            {
                Vars_Func.getFarmInput().Color = drawColor;
                float zValue = MathHelper.Lerp(0.4f, -0.1f, ((float)(Player.Food) / 3000));
                Vector3 farmPostion = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z - zValue);
                Matrix farmMatrix = Matrix.Identity *
                Matrix.CreateScale(Vars_Func.getNestParams(typ).Y) *
                Matrix.CreateRotationX(Vars_Func.getNestParams(typ).Z) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateTranslation(farmPostion);
                Vars_Func.getFarmInput().Draw(camera, farmMatrix, false, isEnlightend, lightPower / 2);
            }

            else if (typ == Vars_Func.NestTyp.Temple)
            {
                Vars_Func.getTempelBath().Color = drawColor;
                Vars_Func.getTempelStone().Color = drawColor;

                float zRoationSpeed = MathHelper.Lerp(0.1f, 1, ((float)(Player.Mana) / 30000));
                rotationSpeed += zRoationSpeed;

                Matrix tempelMatrix = Matrix.Identity *
                Matrix.CreateScale(Vars_Func.getNestParams(typ).Y) *
                Matrix.CreateRotationX(Vars_Func.getNestParams(typ).Z) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(rotationSpeed) *
                Matrix.CreateTranslation(drawPosition);
                Vars_Func.getTempelStone().Draw(camera, tempelMatrix, false, isEnlightend, lightPower / 2);

                Vector3 tempelBathPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z +1f);
                Matrix tempelBathMatrix = Matrix.Identity *
                Matrix.CreateScale(10) *
                Matrix.CreateRotationX(Vars_Func.getNestParams(typ).Z) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateTranslation(tempelBathPosition);
                Vars_Func.getTempelBath().Draw(camera, tempelBathMatrix, false, isEnlightend, lightPower / 2);
            }
        }

        //override public void DrawModel(Renderer.Camera camera, Vector3 drawPosition, Color drawColor)
        //{
        //    Matrix modelMatrix = Matrix.Identity *
        //    Matrix.CreateScale(1) *
        //    Matrix.CreateRotationX(0) *
        //    Matrix.CreateRotationY(0) *
        //    Matrix.CreateRotationZ(0) *
        //    Matrix.CreateTranslation(drawPosition);

        //    Vars_Func.getNestModell(typ).Color = drawColor;
        //    Vars_Func.getNestModell(typ).Draw(camera, modelMatrix);
        //}

        public void spawnCreature(Environment.Map map, int startage = 0)
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
                    new Creature(Vars_Func.CreatureTyp.Knight, tmp, this, Vars_Func.ThingTyp.HeroCreature, map, upgradeCount, startage);
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
                    new Creature(Vars_Func.CreatureTyp.Beetle, tmp, this, Vars_Func.ThingTyp.DungeonCreature, map, upgradeCount);
                    break;

                case Vars_Func.NestTyp.Skeleton:
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
                    new Creature(Vars_Func.CreatureTyp.Skeleton, tmp, this, Vars_Func.ThingTyp.DungeonCreature, map, upgradeCount);
                    break;
            }
        }

        public void addUpgrade(Vars_Func.UpgradeTyp typ, Vector2 position, Environment.Hexagon hex, Environment.Map map)
        {
            upgrades.Add(new Upgrade(typ, position, hex, map));
            switch (typ)
            {
                case Vars_Func.UpgradeTyp.Damage:
                    ++upgradeCount[0];
                    break;
                case Vars_Func.UpgradeTyp.Life:
                    ++upgradeCount[1];
                    break;
                case Vars_Func.UpgradeTyp.Speed:
                    ++upgradeCount[2];
                    break;
            }
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
            }
        }
    }
}
