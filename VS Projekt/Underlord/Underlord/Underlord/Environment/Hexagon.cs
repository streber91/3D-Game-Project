using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Entity;
using Microsoft.Xna.Framework;

namespace Underlord.Environment
{
    class Hexagon : Thing
    {
            float hp = 100f;
            Boolean unbreakable = false, passable = true;
            Vector2 position;
            Vector2[] neighbors;
            List<Boolean> imps;
            Creature creature;
            Vars_Func.TypHex typus;

        public Hexagon(Vars_Func.TypHex typ, int x, int y)
        {
            this.typus = typ;
            this.position.X=x;
            this.position.Y = y;
        }
        public Vars_Func.TypHex getTyp()
        {
            return this.typus;
        }
        public void setTyp(Vars_Func.TypHex typ)
        {
            this.typus = typ;
        }
        public Creature getCreature()
        {
            return this.creature;
        }
        public void setCreature(Creature crea)
        {
            this.creature = crea;
        }
        public void getImp()
        {

        }
        public void addImp()
        {
            this.imps.Add(true);
        }
        public void setHP(float d)
        {
            if (this.unbreakable == true)
            {
            }
            else
            {
                this.hp -= d;
            }
        }
    }
}
