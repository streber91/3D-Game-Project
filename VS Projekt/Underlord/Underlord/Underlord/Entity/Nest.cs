using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Underlord.Renderer;

namespace Underlord.Entity
{
    class Nest : Thing
    {
        Vars_Func.NestTyp typus;
        Upgrade[] upgrades;
        List<Vector2> upgradePos, nestHexagons;
        float size, nutrition, maxNutrition, growcounter;
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
        public Nest(Vars_Func.NestTyp typus, Vector2 position, Environment.Hexagon hex, Environment.Map map)
        {
            nestHexagons = new List<Vector2>();
            nestHexagons.Add(position);
            this.typus = typus;
            this.position = position;
            targetPos = hex.getNeighbors()[3];
            upgradePos = new List<Vector2>();
            undead = false;
            size = 1;
            maxNutrition = 450f;
            nutrition = 250f;
            hex.Obj = this;
        }
        #endregion

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
        public void update(GameTime time)
        {

        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {

        }
    }
}
