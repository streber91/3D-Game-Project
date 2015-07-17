﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Underlord.Renderer;

namespace Underlord.Logic
{
    class Creature : Thing
    {
        Vars_Func.CreatureTyp typ;
        int damageTaken, damage, vision, maxAge, hp;
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
        public Nest Home
        {
            get { return home; }
        }
        public int DamageTaken
        {
            get { return damageTaken; }
        }
        public float Speed
        {
            get { return speed; }
        }
        public int Vision
        {
            get { return vision; }
        }
        public int HP
        {
            get { return (int)(hp * ageModifire); }
        }
        public int Damage
        {
            get { return (int)(damage * ageModifire); }
        }
        #endregion

        #region Constructor
        public Creature(Vars_Func.CreatureTyp typ, Vector2 position, Nest home, Vars_Func.ThingTyp allignment, Environment.Map map)
        {
            switch (typ)
            {
                case Vars_Func.CreatureTyp.Beetle:
                    this.typ = typ;
                    this.position = position;
                    this.home = home;
                    thingTyp = allignment;
                    size = 1;
                    speed = 1;
                    actionTimeCounter = 0;
                    vision = 4;
                    damageTaken = 0;
                    hp = 300;
                    damage = 20;
                    age = 0;
                    maxAge = 100;
                    ageModifire = 1;
			        map.getHexagonAt(position).Obj = this;
                    map.Creatures.Add(this);
                    break;
                case Vars_Func.CreatureTyp.Knight:
                    this.typ = typ;
                    this.position = position;
                    this.home = home;
                    thingTyp = allignment;
                    size = 1;
                    speed = 1;
                    actionTimeCounter = 0;
                    vision = 4;
                    damageTaken = 0;
                    hp = 300;
                    damage = 20;
                    age = 0;
                    ageModifire = 1;
			        map.getHexagonAt(position).Obj = this;
                    map.Heroes.Add(this);
                    break;
                case Vars_Func.CreatureTyp.HQCreatur:
                    this.typ = typ;
                    this.position = position;
                    this.home = home;
                    thingTyp = allignment;
                    size = 1;
                    speed = 1;
                    actionTimeCounter = 0;
                    vision = 4;
                    damageTaken = 0;
                    hp = 300;
                    damage = 20;
                    age = 0;
                    ageModifire = 1;
			        map.getHexagonAt(position).Obj = this;
                    map.Creatures.Add(this);
                    break;
            }
            
        }
        #endregion

        public void takeDamage(int damage)
        {
            if (damage > 0) this.damageTaken += damage;
        }
        
        override public void update(GameTime time, Environment.Map map)
        {
            Logic.AI.compute(this, time, map);
            if (thingTyp == Vars_Func.ThingTyp.DungeonCreature && age > maxAge) map.remove(this);
            age += time.ElapsedGameTime.Milliseconds / 1000;
            ageing();
        }
        
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

            Logic.Vars_Func.getCreatureModell(typ).Color = drawColor;
            Logic.Vars_Func.getCreatureModell(typ).Draw(camera, modelMatrix);
        }
    }
}
