using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Renderer;
using Microsoft.Xna.Framework;
using Underlord.Animation;

namespace Underlord.Logic
{
    class Imp : Thing
    {
        Vars_Func.ThingTyp thingTyp;
        int hp, damage;
        Vector2 position;
        Stack<Vector2> path;
        Logic.Job currentJob;
        float actionTimeCounter;
      
        #region Properties
        public Stack<Vector2> Path
        {
            get { return path; }
            set { path = value; }
        }
        public Logic.Job CurrentJob
        {
            get { return currentJob; }
            set { currentJob = value; }
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
        public int DamageTaken
        {
            get { return damage; }
        }
        public int HP
        {
            get { return hp; }
        }
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
        }
        #endregion

        override public void update(GameTime time, Environment.Map map)
        {
            Logic.AI.compute(this, time, map);
        }

        public void takeDamage(int damage)
        {
            if (damage > 0) hp -= damage;
        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(0.1f) *
            Matrix.CreateRotationX(MathHelper.PiOver2) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            Logic.Vars_Func.getImpModell().Color = drawColor;
            Logic.Vars_Func.getImpModell().Draw(camera, modelMatrix);
        }

        public void AnimationJob(GameTime time, Logic.Job job)
        {
            Logic.Vars_Func.getImpModell().PlayJobAnimation(time, job);
        }

        public void AnimationMove(GameTime time)
        {
            Logic.Vars_Func.getImpModell().PlayMoveAnimation(time);
        }
    }
}
