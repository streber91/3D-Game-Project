using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Underlord.Renderer;

namespace Underlord.Entity
{
    class Creature : Thing
    {
            Vars_Func.Typ type;
            List<Ability> abilities;
            int hp, dmg, vision;
            float size = 1f, attackCooldown = 0.100f, timeSinceLastAttack = 0;
            Vector2 position;
            List<Vector2> path;

        public Creature(Vars_Func.Typ type, List<Ability> ability, Vector2 pos)
        {
            this.type = type;
            this.abilities = ability;
            this.position = pos;
        }

        public void increaseHP(int d)
        {
            if(d<0)
            {
            }
            else
            {
                this.hp+=d;
            }
        }
        public void decreaseHP(int d)
        {
            if(d>0)
            {
            }
            else
            {
                this.hp-=d;
            }
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

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            
        }
    }
}
