using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Entity;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    static class AI
    {
        static Random rand = new Random();
        //TODO
        static public void compute(Imp imp, GameTime time, Environment.Map map)
        {
            
            {
                // search Job
                if (imp.CurrentJob.getJobTyp() == Vars_Func.ImpJob.Idle)
                {
                    imp.CurrentJob = map.JobsWaiting.Dequeue();
                    map.JobsInProgress.Add(imp.CurrentJob);
                }
                // search path to workplace
                else if(imp.Path.Count == 0) imp.Path = determinePath(imp.Position, imp.CurrentJob.getDestination(), map);
                // working
                else if (imp.Path.Count == 1)
                {
                    imp.CurrentJob.Worktime -= time.ElapsedGameTime.Milliseconds;
                    imp.AnimationJob(time, imp.CurrentJob);

                    if (imp.CurrentJob.Worktime <= 0)
                    {
                        map.JobsInProgress.Remove(imp.CurrentJob);
                        map.JobsDone.Add(imp.CurrentJob);
                        imp.CurrentJob = null;
                    }
                }
                else if (imp.ActionTimeCounter >= 500)
                {
                    map.move(imp);
                    imp.AnimationMove(time);
                }
                imp.ActionTimeCounter += time.ElapsedGameTime.Milliseconds;
            }
        }

        static public void compute(Creature creature, GameTime time, Environment.Map map)
        {
            // time for creatur to act?
            if (creature.ActionTimeCounter >= 1000 / creature.getSpeed())
            {
                List<Vector2> nearestEnemy = computeNearestEnemy(creature, map);

                // walk to nearest Enemy and attack if there is one
                if (nearestEnemy != null)
                {
                    if (map.getHexagonAt(creature.Position).getNeighbors().Contains(nearestEnemy[0]))
                    {
                        if(creature.ActionTimeCounter >= 1000 / creature.getSpeed())
                        {
                            if (map.getHexagonAt(nearestEnemy[0]).Obj.getThingTyp() != Vars_Func.ThingTyp.Imp)
                            {
                                Creature target = (Creature)map.getHexagonAt(nearestEnemy[0]).Obj;
                                target.decreaseHP(creature.getDmg());
                                if (target.getHP() <= 0) map.remove(target);
                                creature.ActionTimeCounter = 0;
                            }
                            else
                            {
                                Imp target = (Imp)map.getHexagonAt(nearestEnemy[0]).Obj;
                                target.decreaseHP(creature.getDmg());
                                if (target.getHP() <= 0)
                                {
                                    map.JobsWaiting.Enqueue(target.CurrentJob);
                                    map.JobsInProgress.Remove(target.CurrentJob);
                                    map.remove(target);
                                }
                                creature.ActionTimeCounter = 0;
                            }
                        }
                    }
                    else creature.Path = determinePath(creature.Position, nearestEnemy[0], map, false);
                }

                // calculate path if creature has non
                else if (creature.Path.Count == 0)
                {
                    if (Entity.Vars_Func.computeDistance(creature.getHome().TargetPos, creature.Position, map) < 5) randomwalk(creature, map);
                    else creature.Path = determinePath(creature.Position, creature.getHome().TargetPos, map);
                    // herocreature found no path and so burrow throug walls
                    if (creature.Path == null && creature.getThingTyp() == Vars_Func.ThingTyp.HeroCreature)
                        creature.Path = determinePath(creature.Position, creature.getHome().TargetPos, map, true, true);
                }

                // time left for action?
                if (creature.ActionTimeCounter >= 1000 / creature.getSpeed())
                {
                    map.move(creature);
                }
            }
            creature.ActionTimeCounter += time.ElapsedGameTime.Milliseconds;
        }

        static private List<Vector2> computeNearestEnemy(Creature creature, Environment.Map map)
        {
            //return statement
            List<Vector2> nearesEnemy = new List<Vector2>();

            //breadth-first search
            Vector2 tmp = new Vector2();
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(creature.Position);
            map.getHexagonAt(creature.Position).Visited = true;
            queue.Enqueue(new Vector2(map.getPlanelength() + creature.getVision(), 0));

            while (queue.Count != 1)
            {
                tmp = queue.Dequeue();
                //contains position an enemy creature?
                if (((map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.DungeonCreature ||
                     map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.HeroCreature ||
                     map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.NeutralCreature) &&
                     map.getHexagonAt(tmp).Obj.getThingTyp() != creature.getThingTyp()) ||
                    (map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.Imp &&
                    creature.getThingTyp() != Vars_Func.ThingTyp.DungeonCreature))
                {
                    nearesEnemy[0] = tmp;
                    break;
                }

                if (tmp.X == map.getPlanelength())
                {
                    //is vision of creature reached?
                    if (tmp.X <= map.getPlanelength() + 1) break;
                    queue.Enqueue((new Vector2(tmp.X - 1, 0)));
                    continue;
                }

                foreach (Vector2 hex in map.getHexagonAt(tmp).getNeighbors())
                {
                    // is the hex a not wall objekt?
                    if (!map.getHexagonAt(hex).Visited && (map.getHexagonAt(hex).Obj == null || map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.Wall))
                    {
                        queue.Enqueue(hex);
                        map.getHexagonAt(hex).Visited = true;
                    }
                }
            }
            //clear Hexmap for next search
            for (int i = 0; i < map.getPlanelength(); ++i)
            {
                for (int j = 0; j < map.getPlanelength(); ++j)
                {
                    map.getHexagonAt(i, j).Visited = false;
                }
            }

            return nearesEnemy;
        }

        static private Stack<Vector2> determinePath(Vector2 start, Vector2 destination, Environment.Map map, bool ignoreCreatures = true, bool ignoreWalls = false)
        {
            List<Vector2> des = new List<Vector2>();
            des.Add(destination);
            return determinePath(start, des, map, ignoreWalls, ignoreCreatures);
        }

        static private Stack<Vector2> determinePath(Vector2 start, List<Vector2> destination, Environment.Map map, bool ignoreCreatures = true, bool ignoreWalls = false)
        {
            //return statement
            Stack<Vector2> path = new Stack<Vector2>();

            //breadth-first search
            Vector2 tmp = new Vector2();
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(start);
            map.getHexagonAt(start).Visited = true;
            
            while (queue.Peek() != null)
            {
                tmp = queue.Dequeue();
                if (destination.Contains(tmp)) break;
                foreach (Vector2 hex in map.getHexagonAt(tmp).getNeighbors())
                {
                    // is the hex a not wall objekt? 
                    if (!map.getHexagonAt(hex).Visited && (map.getHexagonAt(hex).Obj == null ||
                        ((map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.Wall || ignoreWalls) && 
                        (map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.DungeonCreature || ignoreCreatures) &&
                        (map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.HeroCreature || ignoreCreatures) &&
                        (map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.NeutralCreature || ignoreCreatures)
                        )))
                    {
                        queue.Enqueue(hex);
                        map.getHexagonAt(hex).Visited = true;
                        map.getHexagonAt(hex).Parent = tmp;
                    }
                }
            }

            //push path on stack
            if (!destination.Contains(tmp)) return null;
            while (tmp != start)
            {
                path.Push(tmp);
                tmp = map.getHexagonAt(tmp).Parent;
            }
            //clear Hexmap for next search
            for (int i = 0; i < map.getPlanelength(); ++i)
            {
                for (int j = 0; j < map.getPlanelength(); ++j)
                {
                    map.getHexagonAt(i, j).Visited = false;
                    map.getHexagonAt(i, j).Parent = new Vector2(i, j);
                }
            }
            return path;
        }

        static private void randomwalk(Creature creature, Environment.Map map)
        {
            //random determination of next step
            creature.Path.Push(map.getHexagonAt(creature.Position).getNeighbors()[(int)rand.Next(6)]);
        }
    }
}
