using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Underlord.Renderer;
using Underlord.Logic;
using Underlord.Animation;
using Underlord.Environment;

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

        Hexagon startHex, currentHex, oldHex, targetHex, oldTargetHex;
        Vector3 targetPosition, tempPosition, fightPosition, startPosition;
        float positionLerpCounter, fightLerpCounter, startLerpCounter;
        float degree;
        float tempZ;

        AnimationModel model;
        Vars_Func.CreatureState currentState = Vars_Func.CreatureState.Walking;
        Vars_Func.CreatureState oldState = Vars_Func.CreatureState.Nothing;
        bool updatePlayer, reachGround; 
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
        public Vars_Func.CreatureState State { set { updatePlayer = true; currentState = value; } }

        public Hexagon CurrentHexagon { set { positionLerpCounter = 0; oldHex = currentHex; currentHex = value; } }

        public Vector3 TempDrawPositon
        {
            get
            {
                if (currentState == Vars_Func.CreatureState.Fighting)
                {
                    return fightPosition;
                }
                else
                {
                    return tempPosition;
                }
            }
        }

        public Hexagon TargetHexagon { set { targetHex = value; } }

        public Vector3 TargetPosition { set { targetPosition = value; } }
        #endregion

        #region Constructor
        public Creature(Vars_Func.CreatureTyp typ, Vector2 position, Nest home, Vars_Func.ThingTyp allignment, Environment.Map map, int[] upgrades, int startage = 0)
        {
            switch (typ)
            {
                case Vars_Func.CreatureTyp.Beetle:
                    this.typ = typ;
                    this.position = position;
                    this.home = home;
                    thingTyp = allignment;
                    size = 1;
                    speed = 1 + upgrades[2] * 0.2f;
                    actionTimeCounter = 0;
                    vision = 4;
                    damageTaken = 0;
                    hp = 200 + upgrades[1] * 100;
                    damage = 10 + upgrades[0] * 4;
                    age = startage;
                    maxAge = 100;
                    ageModifire = 1;
			        map.getHexagonAt(position).Obj = this;
                    map.Creatures.Add(this);
                    break;
                case Vars_Func.CreatureTyp.Skeleton:
                    this.typ = typ;
                    this.position = position;
                    this.home = home;
                    thingTyp = allignment;
                    size = 1;
                    speed = 1 + upgrades[2] * 0.2f;
                    actionTimeCounter = 0;
                    vision = 4;
                    damageTaken = 0;
                    hp = 100 + upgrades[1] * 50;
                    damage = 15 + upgrades[0] * 6;
                    age = startage;
                    maxAge = 120;
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
                    hp = 150;
                    damage = 10;
                    age = startage;
                    ageModifire = 1;
			        map.getHexagonAt(position).Obj = this;
                    map.Heroes.Add(this);
                    currentState = Vars_Func.CreatureState.Starting;
                    startHex = map.getHexagonAt(home.Position);
                    startLerpCounter = 0;
                    reachGround = false;
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
                    damage = 500;
                    age = startage;
                    ageModifire = 1;
			        map.getHexagonAt(position).Obj = this;
                    map.Creatures.Add(this);
                    map.getHexagonAt(position).IsHQ = true;
                    map.getHexagonAt(position).EnlightendHexagon(map);
                    currentState = Vars_Func.CreatureState.PingPong;

                    // Some Fireball Test
                    //map.getHexagonAt(position).Fireball = Vars_Func.getFireBall();
                    break;
            }
            this.model = new AnimationModel(Vars_Func.getCreatureModell(typ).Model);
            model.AnimationClip = Vars_Func.getCreatureModell(typ).AnimationClip;
            updatePlayer = true;
            currentHex = map.getHexagonAt(position);
            oldHex = map.getHexagonAt(position);
            targetHex = null;
            oldTargetHex = null;
            tempZ = 0;
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

            if (typ != Vars_Func.CreatureTyp.HQCreatur)
            {

                if (tempPosition != currentHex.getDrawPosition())
                {
                    positionLerpCounter += (float)time.ElapsedGameTime.Milliseconds;
                    if ((positionLerpCounter / (1000 * 1 / speed)) > 1)
                    {
                        positionLerpCounter = (1000 * 1 / speed);
                    }
                    tempPosition = Vector3.Lerp(oldHex.getDrawPosition(), currentHex.getDrawPosition(), (positionLerpCounter / (1000 * 1 / speed)));
                    degree = this.Rotate(oldHex.getDrawPosition(), currentHex.getDrawPosition());
                }
                this.UpdateState(time);
                this.model.Update(time);
            }
            else
            {
                //Update the fireball
                Vars_Func.getFireBall().Update(time);

                if (currentState == Vars_Func.CreatureState.OpenMouth && (positionLerpCounter / (1000 * 1 / speed)) < 1)
                {
                    positionLerpCounter += (float)time.ElapsedGameTime.Milliseconds;
                    tempZ = positionLerpCounter / (500 * 1 / speed) * 0.3f;
                }
                else if (currentState == Vars_Func.CreatureState.CloseMouth && (positionLerpCounter / (500 * 1 / speed)) > 0)
                {
                    positionLerpCounter -= (float)time.ElapsedGameTime.Milliseconds;
                    tempZ = positionLerpCounter / (500 * 1 / speed) * 0.3f;
                }
                else if (currentState == Vars_Func.CreatureState.PingPong)
                {
                    positionLerpCounter += (float)time.ElapsedGameTime.Milliseconds;
                    tempZ = ((float)(0.5f * Math.Cos(positionLerpCounter / 500 * (1 / speed) + MathHelper.PiOver2)) + 0.5f) * 0.3f;
                }
            }
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

        private void UpdateState(GameTime time)
        {
            switch (currentState)
            {
                case Vars_Func.CreatureState.Walking:
                    this.WalkingBehaviour(time);
                    break;
                case Vars_Func.CreatureState.Fighting:
                    this.FightingBehaviour(time);
                    break;
                case Vars_Func.CreatureState.Starting:
                    this.StartingBehaviour(time);
                    break;
            }
        }

        private void WalkingBehaviour(GameTime time)
        {
            this.UpdateClip(0);
            if (fightLerpCounter > 0)
            {
                fightLerpCounter = 0;
            }
        }

        private void FightingBehaviour(GameTime time)
        {
            if (targetHex != null && targetHex.Neighbors.Contains(position) && tempPosition == currentHex.getDrawPosition())
            {
                if (targetHex.Obj != null && targetHex.Obj is Creature)
                {
                    targetPosition = ((Creature)targetHex.Obj).TempDrawPositon;
                }

                fightLerpCounter += (float)time.ElapsedGameTime.Milliseconds;
                if ((fightLerpCounter / (1000 * 1 / speed)) > 1)
                {
                    fightLerpCounter = (1000 * 1 / speed);
                    this.UpdateClip(1);
                }
                fightPosition = Vector3.Lerp(currentHex.getDrawPosition(), targetPosition, (fightLerpCounter / (1000 * 1 / speed)) * 0.4f);
                degree = this.Rotate(currentHex.getDrawPosition(), targetPosition);

                if (oldTargetHex != targetHex)
                {
                    fightLerpCounter = 0;
                    oldTargetHex = targetHex;
                }
            }
        }

        private void StartingBehaviour(GameTime time)
        {
            if (typ == Vars_Func.CreatureTyp.Knight)
            {
                if (oldHex == currentHex)
                {
                    startLerpCounter += (float)time.ElapsedGameTime.Milliseconds;
                    if ((startLerpCounter / 500) > 1)
                    {
                        startLerpCounter = 500;
                    }

                    if (startPosition == startHex.getDrawPosition() && !reachGround)
                    {
                        reachGround = true;
                        startLerpCounter = 0;
                        updatePlayer = true;
                        this.UpdateClip(0);
                    }

                    if (reachGround)
                    {
                        startPosition = Vector3.Lerp(startHex.getDrawPosition(), currentHex.getDrawPosition(), startLerpCounter / 500);
                        degree = this.Rotate(startHex.getDrawPosition(), currentHex.getDrawPosition());
                    }
                    else
                    {
                        this.UpdateClip(2);
                        Vector3 nestPostion = new Vector3(startHex.getDrawPosition().X, startHex.getDrawPosition().Y, startHex.getDrawPosition().Z + 3f);
                        startPosition = Vector3.Lerp(nestPostion, startHex.getDrawPosition(), startLerpCounter / 500);
                        degree = 0;
                    }
                }
            }
        }

        private void UpdateClip(int index)
        {
            if (updatePlayer)
            {
                AnimationPlayer player = this.model.PlayClip(this.model.AnimationClip[index]);
                player.Looping = true;
                updatePlayer = false;
            }
        }

        private float Rotate(Vector3 source, Vector3 target)
        {
            float valueY = target.Y - source.Y;
            float valueX = target.X - source.X;

            if (valueX == 0)
            {
                if (valueY < 0)
                {
                    return 0;
                }
                else if (valueY > 0)
                {
                    return 2 * MathHelper.PiOver2;
                }
            }
            if (valueY == 0)
            {
                if (valueX < 0)
                {
                    return MathHelper.PiOver2;
                }
                else if (valueX > 0)
                {
                    return 3 * MathHelper.PiOver2;
                }
            }
            if (valueX < 0)
            {
                if (valueY < 0)
                {
                    return -MathHelper.PiOver4;
                }
                else if (valueY > 0)
                {
                    return -3 * MathHelper.PiOver4;
                }
            }
            else if (valueX > 0)
            {
                if (valueY < 0)
                {
                    return MathHelper.PiOver4;
                }
                else if (valueY > 0)
                {
                    return 3 * MathHelper.PiOver4;
                }
            }
            return 0;
        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor, bool isEnlightend, float lightPower)
        {
            if (typ == Vars_Func.CreatureTyp.HQCreatur)
            {
                tempPosition = drawPosition;
                drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + Vars_Func.getCreatureParams(typ).X);
                Matrix modelMatrix = Matrix.Identity *
                Matrix.CreateScale(Vars_Func.getCreatureParams(typ).Y) *
                Matrix.CreateRotationX(Vars_Func.getCreatureParams(typ).Z) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(camera.Rotation) *
                Matrix.CreateTranslation(drawPosition);
                this.model.Color = drawColor;
                this.model.Draw(camera, modelMatrix, false, isEnlightend, lightPower);

                drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + Vars_Func.getCreatureParams(typ).X + tempZ);
                Matrix mouthMatrix = Matrix.Identity *
                Matrix.CreateScale(Vars_Func.getCreatureParams(typ).Y) *
                Matrix.CreateRotationX(Vars_Func.getCreatureParams(typ).Z) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(camera.Rotation) *
                Matrix.CreateTranslation(drawPosition);
                Vars_Func.getHQMouthModel().Color = drawColor;
                Vars_Func.getHQMouthModel().Draw(camera, mouthMatrix, false, isEnlightend, lightPower);
            }
            else
            {
                if (currentState == Vars_Func.CreatureState.Fighting)
                {
                    drawPosition = fightPosition;
                }
                else if (currentState == Vars_Func.CreatureState.Starting)
                {
                    drawPosition = startPosition;
                }
                else
                {
                    drawPosition = tempPosition;
                }

                drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + Vars_Func.getCreatureParams(typ).X);
                Matrix modelMatrix = Matrix.Identity *
                Matrix.CreateScale(Vars_Func.getCreatureParams(typ).Y) *
                Matrix.CreateRotationX(Vars_Func.getCreatureParams(typ).Z) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(degree) *
                Matrix.CreateTranslation(drawPosition);

                //Vars_Func.getCreatureModell(typ).Color = drawColor;
                //Vars_Func.getCreatureModell(typ).Draw(camera, modelMatrix);
                this.model.Color = drawColor;
                this.model.Draw(camera, modelMatrix, false, isEnlightend, lightPower);
                Vars_Func.getCreatureShadow(typ).Draw(camera, modelMatrix, false, isEnlightend, lightPower);
            }
        }

        //override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        //{
        //    drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + Vars_Func.getCreatureParams(typ).X);

        //    Matrix modelMatrix = Matrix.Identity *
        //    Matrix.CreateScale(Vars_Func.getCreatureParams(typ).Y) *
        //    Matrix.CreateRotationX(Vars_Func.getCreatureParams(typ).Z) *
        //    Matrix.CreateRotationY(0) *
        //    Matrix.CreateRotationZ(0) *
        //    Matrix.CreateTranslation(drawPosition);

        //    //Vars_Func.getCreatureModell(typ).Color = drawColor;
        //    //Vars_Func.getCreatureModell(typ).Draw(camera, modelMatrix);
        //    this.model.Color = drawColor;
        //    this.model.Draw(camera, modelMatrix);
        //}
    }
}
