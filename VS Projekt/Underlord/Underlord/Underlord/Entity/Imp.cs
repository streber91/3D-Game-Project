using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Renderer;
using Microsoft.Xna.Framework;

namespace Underlord.Entity
{
    class Imp : Thing
    {
        Vars_Func.ThingTyp thingTyp;
        int HP;
        Vector2 position;
        Stack<Vector2> path;
        Logic.Job currentJob;
        float worktime;

        #region Properties

        public Stack<Vector2> Path
        {
            get { return path; }
            set { path = value; }
        }

        public Logic.Job CurentJob
        {
            get { return currentJob; }
            set { currentJob = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Worktime
        {
            get { return worktime; }
            set { worktime = value; }
        }

        #endregion

        public int getHP() { return HP; }
        public Vars_Func.ThingTyp getThingTyp() { return thingTyp; }

        #region Constructor
        public Imp(Vector2 position)
        {
            worktime = 0;
            HP = 100;
            thingTyp = Vars_Func.ThingTyp.DungeonCreature;
            this.position = position;
        }
        #endregion

        override public void update(GameTime time, Environment.Map map)
        {
            Logic.AI.compute(this, time, map);
        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {

        }
    }
}
