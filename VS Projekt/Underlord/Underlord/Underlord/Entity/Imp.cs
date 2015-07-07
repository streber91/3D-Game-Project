using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Renderer;
using Microsoft.Xna.Framework;
using Underlord.Animation;

namespace Underlord.Entity
{
    class Imp : Thing
    {
        Vars_Func.ThingTyp thingTyp;
        int HP;
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

        #endregion

        public int getHP() { return HP; }
        public Vars_Func.ThingTyp getThingTyp() { return thingTyp; }

        #region Constructor
        public Imp(Vector2 position)
        {
            actionTimeCounter = 0;
            HP = 100;
            thingTyp = Vars_Func.ThingTyp.DungeonCreature;
            this.position = position;
            path = new Stack<Vector2>();
        }
        #endregion

        override public void update(GameTime time, Environment.Map map)
        {
            Logic.AI.compute(this, time, map);
        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(0.1f) *
            Matrix.CreateRotationX(MathHelper.PiOver2) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            Entity.Vars_Func.getImpModell().Color = drawColor;
            Entity.Vars_Func.getImpModell().Draw(camera, modelMatrix);
        }

        public void AnimationJob(GameTime time, Logic.Job job)
        {
            Entity.Vars_Func.getImpModell().PlayJobAnimation(time, job);
        }

        public void AnimationMove(GameTime time)
        {
            Entity.Vars_Func.getImpModell().PlayMoveAnimation(time);
        }
    }
}
