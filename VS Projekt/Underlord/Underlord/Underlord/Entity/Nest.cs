using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Underlord.Entity
{
    class Nest : Thing
    {
        Vars_Func.NestTyp typus;
        Upgrade[] upgrades;
        List<Vector2> upgradePos, nestHexagons, possibleNextNestHexagons;
        float size, nutrition, maxNutrition, growcounter, timeCounter, spawnCounter;
        Boolean undead;
        Vector2 targetPos, position;

        #region Properties
        public Boolean Undead
        {
            get { return undead; }
            set { undead = value; }
        }
        public Vector2 TargetPos
        {
            get { return targetPos; }
            set { targetPos = value; }
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
        public Nest(Vars_Func.NestTyp typus, Vector2 position, Environment.Hexagon hex, Environment.Map map, Vector2 targetPosition)
        {
            possibleNextNestHexagons = new List<Vector2>();
            nestHexagons = new List<Vector2>();
            nestHexagons.Add(position);
            switch (typus)
            {
                case (int)Vars_Func.NestTyp.Beetle:
                    hex.Typ = Vars_Func.HexTyp.BeetleNest;
                    hex.Building = true;
                    hex.Nest = true;
                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = hex.getNeighbors()[i];
                        nestHexagons.Add(neighbor);
                        map.getHexagonAt(neighbor).Typ = Vars_Func.HexTyp.BeetleNest;
                        map.getHexagonAt(neighbor).Building = true;
                        map.getHexagonAt(neighbor).Nest = true;
                        for (int j = 0; j < 6; ++j)
                        {
                            Vector2 nextNeighbor = hex.getNeighbors()[j];
                            if (!map.getHexagonAt(nextNeighbor).Nest && map.getHexagonAt(nextNeighbor).RoomNumber == map.getHexagonAt(neighbor).RoomNumber && !possibleNextNestHexagons.Contains(nextNeighbor))
                            {
                                possibleNextNestHexagons.Add(nextNeighbor);
                            }
                        }
                    }
                    break;
            }
            this.typus = typus;
            this.position = position;
            targetPos = hex.getNeighbors()[3];
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
            if (timeCounter > 1000)
            {
                //update a nest
                if (this.typus != Vars_Func.NestTyp.Entrance)
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
                            Vector2 nextNeighbor = map.getHexagonAt(pos).getNeighbors()[i];
                            if (!map.getHexagonAt(nextNeighbor).Nest && map.getHexagonAt(nextNeighbor).RoomNumber == hex.RoomNumber && !possibleNextNestHexagons.Contains(nextNeighbor))
                            {
                                possibleNextNestHexagons.Add(nextNeighbor);
                            }
                        }
                        possibleNextNestHexagons.RemoveAt(tmp);
                    }
                    timeCounter = 0;
                }
                if (spawnCounter > 4000)
                {
                    spawnCreature(map);
                    spawnCounter = 0;
                }
            }
            //update an entrance
            else
            {

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

            Entity.Vars_Func.getNestModell(typus).Color = drawColor;
            Entity.Vars_Func.getNestModell(typus).Draw(camera, modelMatrix);
        }

        public void spawnCreature(Environment.Map map)
        {
            switch(typus)
            {
                case Vars_Func.NestTyp.Beetle:
                    map.Creatures.Add(new Creature(Vars_Func.CreatureTyp.Beetle, new List<Ability>(), map.getHexagonAt(this.position).getNeighbors()[3], this, Vars_Func.ThingTyp.DungeonCreature, map));
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
