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
        Vars_Func.CreatureTyp typ;
        List<Ability> abilities;
        int hp, dmg, vision, maxAge;
        float size, speed, actionTimeCounter, age;
        Vector2 position;
        Stack<Vector2> path;
        Nest home;
        //TODO implement balancing and diferent stats

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

        #region Constructor
        public Creature(Vars_Func.CreatureTyp typ, List<Ability> ability, Vector2 pos, Nest home, Vars_Func.ThingTyp allignment, Environment.Map map)
        {
            switch (typ)
            {
                case Vars_Func.CreatureTyp.Beetle:
                    this.typ = typ;
                    this.abilities = ability;
                    this.position = pos;
                    this.home = home;
                    thingTyp = allignment;
                    size = 1;
                    speed = 1;
                    actionTimeCounter = 0;
                    vision = 4;
                    hp = 300;
                    dmg = 20;
                    age = 0;
                    maxAge = 100;
			        map.getHexagonAt(pos).Obj = this;
                    map.Creatures.Add(this);
                    break;
                case Vars_Func.CreatureTyp.Knight:
                    this.typ = typ;
                    this.abilities = ability;
                    this.position = pos;
                    this.home = home;
                    thingTyp = allignment;
                    size = 1;
                    speed = 1;
                    actionTimeCounter = 0;
                    vision = 4;
                    hp = 300;
                    dmg = 20;
                    age = 0;
			        map.getHexagonAt(pos).Obj = this;
                    map.Heroes.Add(this);
                    break;
                case Vars_Func.CreatureTyp.HQCreatur:
                    this.typ = typ;
                    this.abilities = ability;
                    this.position = pos;
                    this.home = home;
                    thingTyp = allignment;
                    size = 1;
                    speed = 1;
                    actionTimeCounter = 0;
                    vision = 4;
                    hp = 300;
                    dmg = 20;
                    age = 0;
			        map.getHexagonAt(pos).Obj = this;
                    map.Creatures.Add(this);
                    break;
            }
            
        }
        #endregion

        public void decreaseHP(int d)
        {
            if(d<0){}
            else { this.hp -= d; }
        }
        
        override public void update(GameTime time, Environment.Map map)
        {
            Logic.AI.compute(this, time, map);
            if (thingTyp == Vars_Func.ThingTyp.DungeonCreature && age > maxAge) map.remove(this);
            age += time.ElapsedGameTime.Milliseconds / 1000;
        }
        
        public Nest getHome() { return home; }
        public int getHP() { return this.hp; }
        public int getVision() { return this.vision; }
        public int getDmg() { return this.dmg; }
        public float getSpeed() { return this.speed; }
        
        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(1) *
            Matrix.CreateRotationX(0) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            Entity.Vars_Func.getCreatureModell(typ).Color = drawColor;
            Entity.Vars_Func.getCreatureModell(typ).Draw(camera, modelMatrix);
        }
    }
}
