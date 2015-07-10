﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Underlord.Renderer;

namespace Underlord.Entity
{
    class Creature : Thing
    {
        Vars_Func.CreatureTyp type;
        List<Ability> abilities;
        int hp, dmg, vision;
        float size, speed, actionTimeCounter;
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
        public Creature(Vars_Func.CreatureTyp type, List<Ability> ability, Vector2 pos, Nest home, Vars_Func.ThingTyp allignment, Environment.Map map)
        {
            this.type = type;
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
			map.getHexagonAt(pos).Obj = this;
            map.Creatures.Add(this);
        }
        #endregion

        // TODO filter funktions and implemnt funktions
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
        
        override public void update(GameTime time, Environment.Map map)
        {
            Logic.AI.compute(this, time, map);
            // update path    update life       update attackCD
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

            Entity.Vars_Func.getCreatureModell(type).Color = drawColor;
            Entity.Vars_Func.getCreatureModell(type).Draw(camera, modelMatrix);
        }
    }
}
