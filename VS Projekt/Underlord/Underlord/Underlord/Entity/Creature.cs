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
        Vars_Func.ThingTyp thingTyp;
        Vars_Func.CreatureTyp type;
        List<Ability> abilities;
        int hp, dmg, vision;
        float size, speed, actionTimeCounter;
        Vector2 position;
        Stack<Vector2> path;
        Nest home;

        public Creature(Vars_Func.CreatureTyp type, List<Ability> ability, Vector2 pos, Nest home, Vars_Func.ThingTyp allignment)
        {
            thingTyp = allignment;
            this.home = home;
            this.type = type;
            this.abilities = ability;
            this.position = pos;
            size = 1;
            speed = 1;
            actionTimeCounter = 0;
        }

        #region Properties
        public Stack<Vector2> Path
        {
            get { return path; }
            set { path = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float ActionTimeCounter
        {
            get { return actionTimeCounter; }
            set { actionTimeCounter = value; }
        }
        #endregion

        public void increaseHP(int d)
        {
            if(d<0)
            {
            }
            else { this.hp+=d; }
        }
        public void decreaseHP(int d)
        {
            if(d>0)
            {
            }
            else { this.hp -= d; }
        }

        public void update(GameTime time)
        {
            // update path    update life       update attackCD
        }

        public Boolean canMove(/*Direction dir*/){ return false; }
        public Nest getHome() { return home; }
        public float getHP() { return this.hp; }
        public int getVision() { return this.vision; }
        //public Vector2 getNextPathTile() { return this.path.ElementAt(0); }
        public float getDmg() { return this.dmg; }
        public float getSpeed() { return this.speed; }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            
        }
    }
}
