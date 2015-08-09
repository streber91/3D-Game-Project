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

        public void updateJob(Environment.Map map, Entity.Imp imp)
        {
            imp.CurrentJob.Worktime -= imp.ActionTimeCounter;
            imp.ActionTimeCounter = 0;

            switch (jobTyp)
            {
                case Vars_Func.ImpJob.Harvest:
                    break;
                    //TODO
                case Vars_Func.ImpJob.Feed:
                    if (Player.enoughFood(10))
                    {
                        Player.Food -= 10;
                        ((Entity.Nest)map.getHexagonAt(destination).Obj).increaseNutrition(10);
                    }
                    break;

                case Vars_Func.ImpJob.Mine:
                    Entity.Wall wall = ((Entity.Wall)map.getHexagonAt(destination).Obj); 
                    wall.HP -= 10;
                    if (wall.HP <= 0)
                    {
                        map.JobsDone.Add(this);
                        map.JobsInProgress.Remove(this);
                        imp.CurrentJob = new Job(Vars_Func.ImpJob.Idle);
                    }
                    break;

                case Vars_Func.ImpJob.MineDiamonds:
                    Player.Gold += 10;
                    break;

                case Vars_Func.ImpJob.MineGold:
                    Entity.Wall goldWall = ((Entity.Wall)map.getHexagonAt(destination).Obj);
                    goldWall.HP -= 10;
                    if (goldWall.Gold > 0)
                    {
                        Player.Gold += Math.Min(10, goldWall.Gold);
                        goldWall.Gold -= Math.Min(10, goldWall.Gold);
                    }
                    if (goldWall.HP <= 0)
                    {
                        Player.Gold += goldWall.Gold;
                        map.JobsDone.Add(this);
                        map.JobsInProgress.Remove(this);
                        imp.CurrentJob = new Job(Vars_Func.ImpJob.Idle);
                    }
                    break;
            }
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
                    break;

                case Vars_Func.ImpJob.MineDiamonds:           
                    break;

                case Vars_Func.ImpJob.MineGold:
                    map.MineJobs.Remove(destination);
                    map.getHexagonAt(destination).Obj = null;
                    break;
            }
            map.JobsDone.Remove(this);
        }

    }
}
