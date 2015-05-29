using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Entity
{
    class Creature : Thing
    {
            Vars_Func.Typ typus;
            List<Ability> abilities;
            float hp = 250f, dmg = 15f, size = 1f, attackCooldown = 0.100f, timeSinceLastAttack = 0;
            int vision = 4;
            Vector2 position;
            List<Vector2> path;

        public Creature(Vars_Func.Typ typus, List<Ability> ability, Vector2 pos)
        {
            this.typus = typus;
            this.abilities = ability;
            this.position = pos;
        }

        public void increaseHP(float d)
        {
            if(d<0)
            {
            }
            else
            {
                this.hp+=d;
            }
        }
        public void decreaseHP(float d)
        {
            if(d>0)
            {
            }
            else
            {
                this.hp-=d;
            }
        }
        public void draw()
        {
        }

        public void update(GameTime time)
        {
            // update path    update life       update attackCD
        }

        public Boolean canMove(/*Direction dir*/)
        {
           return false;
        }

        public void changePosition(Vector2 pos)
        {
            this.position=pos;
        }


        public float getHP()
        {
            return this.hp;
        }
        public int getVision()
        {
            return this.vision;
        }
        public Vector2 getNextPathTile()
        {
            return this.path.ElementAt(0);
        }
        public float getDmg()
        {
            return this.dmg;
        }
        public float getAttackCooldown()
        {
            return this.attackCooldown;
        }
        public void setAttackCooldown(float d)
        {
            this.attackCooldown = d;
        }
        public float getTimeSinceLastAttack()
        {
            return this.timeSinceLastAttack;
        }
    }
}
