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
        int damageTaken, dmg, vision, maxAge, hp;
        float size, speed, actionTimeCounter, age, ageModifire;
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
                    damageTaken = 0;
                    hp = 300;
                    dmg = 20;
                    age = 0;
                    maxAge = 100;
                    ageModifire = 1;
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
                    damageTaken = 0;
                    hp = 300;
                    dmg = 20;
                    age = 0;
                    ageModifire = 1;
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
                    damageTaken = 0;
                    hp = 300;
                    dmg = 20;
                    age = 0;
                    ageModifire = 1;
			        map.getHexagonAt(pos).Obj = this;
                    map.Creatures.Add(this);
                    break;
            }
            
        }
        #endregion

        public void takeDamage(int d)
        {
            if(d<=0){}
            else { this.damageTaken += d; }
        }
        
        override public void update(GameTime time, Environment.Map map)
        {
            Logic.AI.compute(this, time, map);
            if (thingTyp == Vars_Func.ThingTyp.DungeonCreature && age > maxAge) map.remove(this);
            age += time.ElapsedGameTime.Milliseconds / 1000;
            ageing();
        }
        
        public Nest getHome() { return home; }
        public int getDamageTaken() { return damageTaken; }
        public int getHP() { return (int)(hp * ageModifire); }
        public int getVision() { return vision; }
        public int getDmg() { return (int)(dmg * ageModifire); }
        public float getSpeed() { return speed; }
        
        private void ageing()
        {
            switch (thingTyp)
            {
                case Vars_Func.ThingTyp.HeroCreature:
                    if (age >= (ageModifire - 1) * 200) ageModifire += 0.05f;
                    break;
                case Vars_Func.ThingTyp.DungeonCreature:
                    if (age >= maxAge * 0.3f && ageModifire == 1) ageModifire = 1.3f;
                    else if (age >= maxAge * 0.8f && ageModifire == 1.3f) ageModifire = 1.1f;
                    break;
                case Vars_Func.ThingTyp.HQCreature:
                    break;
            }
        }

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
