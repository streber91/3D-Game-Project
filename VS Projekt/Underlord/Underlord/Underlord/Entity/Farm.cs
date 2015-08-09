using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Entity
{
    class Farm : Thing
    {
        int food;
        bool getsHarvested;
        Vector2 position;
        float foodCounter;

        #region Properties
        public int Food
        {
            get { return food; }
            set { food = value; }
        }
        public Boolean GetsHarvested
        {
            get { return getsHarvested; }
            set { getsHarvested = value; }
        }
        public Vector2 Position
        {
            get { return position; }
        }
        #endregion

        #region Constructor
        public Farm(Vector2 position, Environment.Map map)
        {
            this.position = position;
            thingTyp = Logic.Vars_Func.ThingTyp.Farm;
            food = 0;
            getsHarvested = false;
            foodCounter = 0;

            map.getHexagonAt(position).Obj = this;
            map.Farms.Add(this);
        }
        #endregion

        override public void update(GameTime gameTime, Environment.Map map)
        {
            foodCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (foodCounter > 100)
            {
                food++;
                foodCounter = 0;
                if (food >= 100 && getsHarvested == false)
                {
                    getsHarvested = true;
                    map.JobsWaiting.Enqueue(new Logic.Job(Logic.Vars_Func.ImpJob.Harvest, position));
                }
            } 
        }

        override public void DrawModel(Renderer.Camera camera, Vector3 drawPosition, Color drawColor)
        {

        }
    }
}
