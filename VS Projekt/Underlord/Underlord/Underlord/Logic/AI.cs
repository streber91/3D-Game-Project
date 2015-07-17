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

        static public void compute(Imp imp, GameTime time, Environment.Map map)
        {
            if (imp.ActionTimeCounter >= 500)
            {
                if (imp.Path == null) imp.Path = new Stack<Vector2>();
                // search Job
                if (imp.CurrentJob.getJobTyp() == Vars_Func.ImpJob.Idle)
                {
                    //jobs there?
                    if (map.JobsWaiting.Peek() != null)
                    {
                        imp.CurrentJob = map.JobsWaiting.Dequeue();
                        map.JobsInProgress.Add(imp.CurrentJob);
                    }
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
                // nothing to do?
                if (imp.Path == null || imp.Path.Count == 0) randomwalk(imp, map);

                // time left for action?
                if (imp.ActionTimeCounter >= 500)
                {
                    map.move(imp);
                    imp.AnimationMove(time);
                }
                imp.ActionTimeCounter = 0;
            }
            imp.ActionTimeCounter += time.ElapsedGameTime.Milliseconds;
        }

        static public void compute(Creature creature, GameTime time, Environment.Map map)
        {
            // time for creatur to act and none HQcreature?
            if (creature.ActionTimeCounter >= 1000 / creature.getSpeed() && creature.getThingTyp() != Vars_Func.ThingTyp.HQCreature)
            {
                Vector2 nearestEnemy = computeNearestEnemy(creature, map);
                if(creature.Path == null) creature.Path = new Stack<Vector2>();
                // neutral creatures only randomwalk
                if (creature.getThingTyp() == Vars_Func.ThingTyp.NeutralCreature) randomwalk(creature, map);
                // walk to nearest Enemy and attack if there is one
                else if (nearestEnemy.X != map.getPlanelength())
                {
                    if (map.getHexagonAt(creature.Position).Neighbors.Contains(nearestEnemy))
                    {
                        if (creature.ActionTimeCounter >= 1000 / creature.getSpeed())
                        {
                            // attack creature
                            if (map.getHexagonAt(nearestEnemy).Obj.getThingTyp() != Vars_Func.ThingTyp.Imp)
                            {
                                Creature target = (Creature)map.getHexagonAt(nearestEnemy).Obj;
                                target.takeDamage(creature.getDmg());
                                if (target.getDamageTaken() >= target.getHP()) map.remove(target);
                                creature.ActionTimeCounter = 0;
                            }
                            // attack imp
                            else
                            {
                                Imp target = (Imp)map.getHexagonAt(nearestEnemy).Obj;
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
                    // serach path
                    else creature.Path = determinePath(creature.Position, map.getHexagonAt(nearestEnemy).Neighbors, map, false);
                }

                // calculate path if creature has none
                else if (creature.Path.Count == 0)
                {
                    if (Entity.Vars_Func.computeDistance(creature.getHome().TargetPosition, creature.Position, map) < 5) randomwalk(creature, map);
                    else creature.Path = determinePath(creature.Position, creature.getHome().TargetPosition, map);
                    // herocreature found no path and so burrow throug walls
                    if (creature.Path == null && creature.getThingTyp() == Vars_Func.ThingTyp.HeroCreature)
                        creature.Path = determinePath(creature.Position, creature.getHome().TargetPosition, map, true, true);
                }

                // time left for action?
                if (creature.ActionTimeCounter >= 1000 / creature.getSpeed())
                {
                    map.move(creature);
                }
                creature.ActionTimeCounter = 0;
            }
            creature.ActionTimeCounter += time.ElapsedGameTime.Milliseconds;
        }

        static private Vector2 computeNearestEnemy(Creature creature, Environment.Map map)
        {
            //return statement
            Vector2 nearesEnemy = new Vector2(map.getPlanelength(), 0);

            //breadth-first search
            Vector2 tmp = new Vector2();
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(creature.Position);
            map.getHexagonAt(creature.Position).Visited = true;
            queue.Enqueue(new Vector2(map.getPlanelength(), creature.getVision()));

            while (queue.Count != 1)
            {
                tmp = queue.Dequeue();

                if (tmp.X == map.getPlanelength())
                {
                    //is vision of creature reached?
                    if (tmp.Y <= 1) break;
                    queue.Enqueue((new Vector2(tmp.X, tmp.Y - 1)));
                    continue;
                }
                //contains position an enemy creature?
                if ( map.getHexagonAt(tmp).Obj != null &&
                    // Enemys can attack player creatures
                    (((map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.Imp || map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.DungeonCreature ||
                    map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.HQCreature) &&
                    (creature.getThingTyp() != Vars_Func.ThingTyp.HQCreature && creature.getThingTyp() != Vars_Func.ThingTyp.DungeonCreature)) ||
                    // everyone can attack neural and heroes expect themselfe
                    ((map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.HeroCreature || map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.NeutralCreature) &&
                    map.getHexagonAt(tmp).Obj.getThingTyp() != creature.getThingTyp())))
                {
                    nearesEnemy = tmp;
                    break;
                }

                foreach (Vector2 hex in map.getHexagonAt(tmp).Neighbors)
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
            return determinePath(start, des, map, ignoreCreatures, ignoreWalls);
        }

        static private Stack<Vector2> determinePath(Vector2 start, Vector2[] destination, Environment.Map map, bool ignoreCreatures = true, bool ignoreWalls = false)
        {
            List<Vector2> des = new List<Vector2>();
            foreach(Vector2 vec in destination)
            {
                des.Add(vec);
            }
            return determinePath(start, des, map, ignoreCreatures, ignoreWalls);
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
            
            while (queue.Count != 0)
            {
                tmp = queue.Dequeue();
                if (destination.Contains(tmp)) break;
                foreach (Vector2 hex in map.getHexagonAt(tmp).Neighbors)
                {
                    // is the hex a not wall objekt? 
                    if (!map.getHexagonAt(hex).Visited && (map.getHexagonAt(hex).Obj == null ||
                        ((map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.Wall || ignoreWalls) && 
                        (map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.DungeonCreature || ignoreCreatures) &&
                        (map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.HeroCreature || ignoreCreatures) &&
                        (map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.NeutralCreature || ignoreCreatures) &&
                        map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.Nest && map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.Upgrade &&
                        (map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.HQCreature || map.getHexagonAt(start).Obj.getThingTyp() == Vars_Func.ThingTyp.HeroCreature)
                        )))
                    {
                        queue.Enqueue(hex);
                        map.getHexagonAt(hex).Visited = true;
                        map.getHexagonAt(hex).Parent = tmp;
                    }
                }
            }

            //push path on stack
            if (!destination.Contains(tmp)) path = null;
            else
            {
                while (tmp != start)
                {
                    path.Push(tmp);
                    tmp = map.getHexagonAt(tmp).Parent;
                }
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
            int random = (int)rand.Next(6);
            if (map.getHexagonAt(map.getHexagonAt(creature.Position).Neighbors[random]).Obj == null)
            {
                creature.Path.Push(map.getHexagonAt(creature.Position).Neighbors[random]);
            }
        }

        static private void randomwalk(Imp imp, Environment.Map map)
        {
            //random determination of next step
            imp.Path.Push(map.getHexagonAt(imp.Position).Neighbors[(int)rand.Next(6)]);
        }
    }
}
