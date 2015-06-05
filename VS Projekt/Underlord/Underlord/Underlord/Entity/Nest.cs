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
            List<Vector2> upgradePos;
            float size = 1f, nutrition = 250f, maxNutrition = 450f;
            Boolean undead = false;
            Vector2 targetPos, position;

        // update list in construtor for loading issues
        public Nest(Vars_Func.Typ typus, Vector2 pos, List<Upgrade> ups, List<Vector2> upsPos)
        {
            this.typus = typus;
            this.position = pos;
            this.upgrades = ups;
            this.upgradePos = upsPos;
        }

        public void upgrade(Upgrade ups, Vector2 upsPos)
        {
            this.upgrades.Add(ups);
            this.upgradePos.Add(upsPos);
        }
        public void increaseNutrition(float d)
        {
            if (this.nutrition + d <= maxNutrition)
            {
                this.nutrition += d;
            }
            else
            {
            }
        }
        public void decreaseNutrition(float d)
        {
            this.nutrition -= d;

            if (this.nutrition <= 0)
            {
                setDead(true);
            }
            else
            {
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
