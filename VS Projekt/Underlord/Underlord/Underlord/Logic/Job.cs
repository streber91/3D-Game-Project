using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    class Job
    {
        Logic.Vars_Func.ImpJob jobTyp;
        Vector2 destination;
        float worktime;

        #region Properties
        public float Worktime
        {
            get { return worktime; }
            set { worktime = value; }
        }
        public Logic.Vars_Func.ImpJob JobTyp
        {
            get { return jobTyp; }
        }
        public Vector2 Destination
        {
            get { return destination; }
        }
        #endregion

        #region Constructor
        public Job(Logic.Vars_Func.ImpJob jobTyp, Vector2 destination = new Vector2(), float worktime = 0)
        {
            this.jobTyp = jobTyp;
            this.destination = destination;
            this.worktime = worktime;
        }

        #endregion

        public void updateJob(Environment.Map map)
        {
 
        }

        public void endJob(Environment.Map map)
        {
            switch(jobTyp)
            {
                case Vars_Func.ImpJob.Harvest:
                    break;

                case Vars_Func.ImpJob.Feed:
                    break;

                case Vars_Func.ImpJob.Mine:
                    map.MineJobs.Remove(destination);
                    map.getHexagonAt(destination).Obj = null;
                    Player.Gold += 5;
                    break;

                case Vars_Func.ImpJob.MineDiamonds:
                    map.JobsWaiting.Enqueue(new Job(Vars_Func.ImpJob.MineDiamonds, this.destination, 5000));
                    Player.Gold += 100;
                    break;

                case Vars_Func.ImpJob.MineGold:
                    map.MineJobs.Remove(destination);
                    map.getHexagonAt(destination).Obj = null;
                    Player.Gold += 100;
                    break;
            }
            map.JobsDone.Remove(this);
        }

    }
}
