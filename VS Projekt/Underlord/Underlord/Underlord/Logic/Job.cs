using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    class Job
    {
        Entity.Vars_Func.ImpJob jobTyp;
        List<Vector2> destination;
        float worktime;

        public Job(Entity.Vars_Func.ImpJob jobTyp, List<Vector2> destination, float worktime)
        {
            this.jobTyp = jobTyp;
            this.destination = destination;
            this.worktime = worktime;
        }

        #region Properties

        public float Worktime
        {
            get { return worktime; }
            set { worktime = value; }
        }

        #endregion

        public Entity.Vars_Func.ImpJob getJobTyp() { return jobTyp; }
        public List<Vector2> getDestination() { return destination; }
    }
}
