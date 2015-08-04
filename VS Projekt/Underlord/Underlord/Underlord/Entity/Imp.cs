using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Renderer;
using Microsoft.Xna.Framework;
using Underlord.Animation;
using Underlord.Logic;

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

        AnimationModel model;
        Vars_Func.ImpState state = Vars_Func.ImpState.Walking;
        Vars_Func.ImpState oldState = Vars_Func.ImpState.Nothing;

        #region Properties
        public Stack<Vector2> Path { get { return path; } set { path = value; } }
      
        public Logic.Job CurrentJob { get { return currentJob; } set { currentJob = value; } }

        public Vector2 Position { get { return position; } set { position = value; } }

        public float ActionTimeCounter  { get { return actionTimeCounter; } set { actionTimeCounter = value; } }

        public int DamageTaken { get { return damage; } }

        public int HP { get { return hp; } }

        public Vars_Func.ImpState State { set { state = value; } }
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
        }
        #endregion

        override public void update(GameTime time, Environment.Map map)
        {
            AI.compute(this, time, map);

            if (oldState != state)
            {
                AnimationPlayer player = null;
                switch (state)
                {
                    case Vars_Func.ImpState.Walking:
                        player = this.model.PlayClip( this.model.AnimationClip[0]);
                        player.Looping = true;

                        break;
                    case Vars_Func.ImpState.Digging:

                        break;
                }

                if (player != null)
                {
                    player.Looping = true;
                }

                oldState = state;
            }
            
            this.model.Update(time);          
        }

        public void takeDamage(int damage)
        {
            if (damage > 0) hp -= damage;
        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + 0.5f);
            
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(0.05f) *
            Matrix.CreateRotationX(MathHelper.PiOver2) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            this.model.Color = drawColor;
            this.model.Draw(camera, modelMatrix);
        }
    }
}
