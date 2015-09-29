using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Renderer;
using Microsoft.Xna.Framework;
using Underlord.Animation;
using Underlord.Logic;
using Underlord.Environment;

namespace Underlord.Entity
{
    class Imp : Thing
    {
        Vars_Func.ThingTyp thingTyp;
        int hp, damage;
        Vector2 position;
        Stack<Vector2> path;
        Logic.Job currentJob;
        float actionTimeCounter;

        Hexagon currentHex, oldHex, targetHex;
        Vector3 tempPosition;

        float positionLerpCounter, diggLerpCounter;
        float degree;
        float[] animationSpeeds;

        AnimationModel model;
        Vars_Func.ImpState currentState = Vars_Func.ImpState.Walking;
        Vars_Func.ImpState oldState = Vars_Func.ImpState.Nothing;
        bool updatePlayer; 

        #region Properties
        public Stack<Vector2> Path { get { return path; } set { path = value; } }
      
        public Logic.Job CurrentJob { get { return currentJob; } set { currentJob = value; } }

        public Vector2 Position { get { return position; } set { position = value; } }

        public float ActionTimeCounter  { get { return actionTimeCounter; } set { actionTimeCounter = value; } }

        public int DamageTaken { get { return damage; } }

        public int HP { get { return hp; } }

        public Vars_Func.ImpState State { set { updatePlayer = true; currentState = value; } }

        public Hexagon CurrentHexagon { set { positionLerpCounter = 0; oldHex = currentHex; currentHex = value; } }

        public Vector3 TempDrawPositon { get { return tempPosition; } }

        public Hexagon TargetHexagon { set { targetHex = value; } }
        #endregion

        #region Constructor
        public Imp(Vector2 position, Environment.Map map)
        {
            actionTimeCounter = 0;
            hp = 100;
            damage = 0;
            thingTyp = Vars_Func.ThingTyp.Imp;
            this.position = position;
            path = new Stack<Vector2>();
            map.getHexagonAt(position).Imps.Add(this);
            map.ImpList.Add(this);
            currentJob = new Job(Vars_Func.ImpJob.Idle);
            this.model = new AnimationModel(Vars_Func.getImpModell().Model, Vars_Func.getImpModell().AnimationClip);
            updatePlayer = true;
            currentHex = map.getHexagonAt(position);
            oldHex = map.getHexagonAt(position);
            targetHex = null;
            animationSpeeds = new float[4];
            animationSpeeds[0] = 2;
            animationSpeeds[1] = 1;
            animationSpeeds[2] = 1;
            animationSpeeds[3] = 1;
        }
        #endregion

        public void takeDamage(int damage)
        {
            if (damage > 0) hp -= damage;
        }

        override public void update(GameTime time, Environment.Map map)
        {
            AI.compute(this, time, map);

            if (tempPosition != currentHex.getDrawPosition())
            {
                positionLerpCounter += (float)time.ElapsedGameTime.Milliseconds;
                if ((positionLerpCounter / 500) > 1)
                {
                    positionLerpCounter = 500;
                }
                tempPosition = Vector3.Lerp(oldHex.getDrawPosition(), currentHex.getDrawPosition(), (positionLerpCounter / 500));
                degree = this.Rotate(oldHex.getDrawPosition(), currentHex.getDrawPosition());
            }
            this.UpdateState(time);
            this.model.Update(time);       
        }

        private void UpdateState(GameTime time)
        {
            switch (currentState)
            {
                case Vars_Func.ImpState.Walking:
                    this.WalkingBehaviour(time);
                    break;
                case Vars_Func.ImpState.Digging:
                    this.DiggingBehaviour(time);
                    break;
                case Vars_Func.ImpState.Praying:
                    this.PrayingBehaviour(time);
                    break;
                case Vars_Func.ImpState.Harvesting:
                    this.HarvestingBehaviour(time);
                    break;
            }
        }

        private void WalkingBehaviour(GameTime time)
        {
            this.UpdateClip(0);
            if (diggLerpCounter > 0)
            {
                diggLerpCounter = 0;
            }
        }

        private void DiggingBehaviour(GameTime time)
        {
            if (targetHex != null && targetHex.Neighbors.Contains(position) && tempPosition == currentHex.getDrawPosition())
            {
                diggLerpCounter += (float)time.ElapsedGameTime.Milliseconds;
                if ((diggLerpCounter / 500) > 1)
                {
                    diggLerpCounter = 500;
                    this.UpdateClip(1);
                }
                tempPosition = Vector3.Lerp(currentHex.getDrawPosition(), targetHex.getDrawPosition(), (diggLerpCounter / 2000));
                degree = this.Rotate(currentHex.getDrawPosition(), targetHex.getDrawPosition());
            }
        }

        private void PrayingBehaviour(GameTime time)
        {
            this.UpdateClip(2);
            if (diggLerpCounter > 0)
            {
                diggLerpCounter = 0;
            }
        }

        private void HarvestingBehaviour(GameTime time)
        {
            this.UpdateClip(3);
            if (diggLerpCounter > 0)
            {
                diggLerpCounter = 0;
            }
        }

        private void UpdateClip(int index)
        {
            if (updatePlayer)
            {
                AnimationPlayer player = this.model.PlayClip(this.model.AnimationClip[index], animationSpeeds[index]);
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
            drawPosition = tempPosition;
            drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + 0.5f);

            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(0.05f) *
            Matrix.CreateRotationX(MathHelper.PiOver2) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(degree) *
            Matrix.CreateTranslation(drawPosition);

            //Vars_Func.getCreatureModell(typ).Color = drawColor;
            //Vars_Func.getCreatureModell(typ).Draw(camera, modelMatrix);
            this.model.Color = drawColor;
            this.model.Draw(camera, modelMatrix, !(drawColor.Equals(Color.White)), isEnlightend, lightPower);
            Vars_Func.getImpShadow().Draw(camera, modelMatrix, !(drawColor.Equals(Color.White)), isEnlightend, lightPower);
        }
    }
}
