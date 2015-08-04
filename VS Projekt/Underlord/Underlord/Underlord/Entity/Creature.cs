using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Underlord.Renderer;
using Underlord.Logic;
using Underlord.Animation;

namespace Underlord.Entity
{
    class Creature : Thing
    {
        Vars_Func.CreatureTyp typ;
        int damageTaken, damage, vision, maxAge, hp;
        float size, speed, actionTimeCounter, age, ageModifire;
        Vector2 position;
        Stack<Vector2> path;
        Nest home;

        AnimationModel model; 
        //TODO implement balancing and diferent stats

        Boolean wasSet = false;

        #region Properties
        public Vars_Func.CreatureTyp Typ
        {
            get { return typ; }
        }
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
        public float Age
        {
            get { return age; }
        }
        #endregion

        #region Constructor
        public Creature(Vars_Func.CreatureTyp typ, Vector2 position, Nest home, Vars_Func.ThingTyp allignment, Environment.Map map, int[] upgrades)
        {
            switch (typ)
            {
                case Vars_Func.CreatureTyp.Beetle:
                    this.typ = typ;
                    this.position = position;
                    this.home = home;
                    thingTyp = allignment;
                    size = 1;
                    speed = 1 + upgrades[2] * 0.1f;
                    actionTimeCounter = 0;
                    vision = 4;
                    damageTaken = 0;
                    hp = 500 + upgrades[1] * 100;
                    damage = 10 + upgrades[0] * 2;
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
                    hp = 5000;
                    damage = 25;
                    age = 0;
                    ageModifire = 1;
			        map.getHexagonAt(position).Obj = this;
                    map.Creatures.Add(this);
                    break;
            }
            this.model = new AnimationModel(Vars_Func.getCreatureModell(typ).Model);
        }
        #endregion

        public void takeDamage(int damage)
        {
            if (damage > 0) this.damageTaken += damage;
        }
        
        override public void update(GameTime time, Environment.Map map)
        {
            AI.compute(this, time, map);
            if (thingTyp == Logic.Vars_Func.ThingTyp.DungeonCreature && age > maxAge) map.DyingCreatures.Add(this);
            age += (float)time.ElapsedGameTime.Milliseconds / 1000;
            ageing();

            this.Animation(time);
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

        private void Animation(GameTime time) {

            if (!wasSet)
            {
                AnimationPlayer player = this.model.PlayClip(this.model.Clips[0]);
                player.Looping = true;
                wasSet = true;
            }
            this.model.Update(time);
        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + Vars_Func.getCreatureParams(typ).X);

            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(Vars_Func.getCreatureParams(typ).Y) *
            Matrix.CreateRotationX(Vars_Func.getCreatureParams(typ).Z) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            //Vars_Func.getCreatureModell(typ).Color = drawColor;
            //Vars_Func.getCreatureModell(typ).Draw(camera, modelMatrix);
            this.model.Color = drawColor;
            this.model.Draw(camera, modelMatrix);
        }
    }
}
