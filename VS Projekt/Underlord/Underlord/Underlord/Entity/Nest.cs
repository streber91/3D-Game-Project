using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Underlord.Renderer;

namespace Underlord.Entity
{
    class Nest : Thing
    {
            Vars_Func.Typ typus;
            List<Upgrade> upgrades;
            List<Vector2> upgradePos, nestHexagons;
            float size, nutrition, maxNutrition, growcounter;
            Boolean undead = false;
            Vector2 targetPos, position;
        // update list in construtor for loading issues
        public Nest(Vars_Func.Typ typus, Vector2 position)
        {
            this.nestHexagons = new List<Vector2>();
            nestHexagons.Add(position);
            this.typus = typus;
            this.position = position;
            this.upgrades = new List<Upgrade>();
            this.upgradePos = new List<Vector2>();
            size = 1;
            maxNutrition = 450f;
            nutrition = 250f;
        }

        public void addUpgrade(Upgrade upgrade, Vector2 upgradePosition)
        {
            this.upgrades.Add(upgrade);
            this.upgradePos.Add(upgradePosition);
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
            this.nutrition -= d;

            if (nutrition <= 0)
            {
                nutrition = 0;
                setDead(true);
            }
        }
        public void update(GameTime time)
        {

        }
        public Boolean isUndead()
        {
            return this.undead;
        }
        public void setDead(Boolean dead)
        {
            this.undead=dead;
        }
        public void setTarget(Vector2 pos)
        {
            this.targetPos = pos;
        }
        public Vector2 getTarget()
        {
            return this.targetPos;
        }
        public Vector2 getPosition()
        {
            return this.position;
        }
        public void setPosition(Vector2 pos)
        {
            this.position = pos;
        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {

        }
    }
}
