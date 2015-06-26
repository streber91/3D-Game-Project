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

        static public void compute(Creature creature, GameTime time, Environment.Map map)
        {
            // time for creatur to act?
            if (creature.ActionTimeCounter >= 1000 / creature.getSpeed())
            {
                List<Vector2> nearestEnemy = computeNearestEnemy(creature, map);

                //if(gegner in angrifsreichweite) angreifen else TODO
                if(nearestEnemy != null) creature.Path = determinePath(creature.Position, nearestEnemy[0], map);
                else if (creature.Path.Count == 0)
                {
                    if (Entity.Vars_Func.computeDistance(creature.getHome().getTarget(), creature.Position, map) < 5) randomwalk(creature, map);
                    else creature.Path = determinePath(creature.Position, creature.getHome().getTarget(), map);
                }
                // time left for action?
                if (creature.ActionTimeCounter >= 1000 / creature.getSpeed())
                {
                    creature.Position = creature.Path.Pop();
                    creature.ActionTimeCounter -= 1000 / creature.getSpeed();
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
                if ((map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.DungeonCreature ||
                     map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.HeroCreature ||
                     map.getHexagonAt(tmp).Obj.getThingTyp() == Vars_Func.ThingTyp.NeutralCreature) &&
                     map.getHexagonAt(tmp).Obj.getThingTyp() != creature.getThingTyp())
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

        static private Stack<Vector2> determinePath(Vector2 start, Vector2 destination , Environment.Map map)
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
                if (tmp == destination) break;
                foreach (Vector2 hex in map.getHexagonAt(tmp).getNeighbors())
                {
                    // is the hex a not wall objekt? 
                    if (!map.getHexagonAt(hex).Visited && (map.getHexagonAt(hex).Obj == null || map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.Wall))
                    {
                        queue.Enqueue(hex);
                        map.getHexagonAt(hex).Visited = true;
                        map.getHexagonAt(hex).Parent = tmp;
                    }
                }
            }

            //clear Hexmap for next search
            while (tmp != start)
            {
                path.Push(tmp);
                tmp = map.getHexagonAt(tmp).Parent;
            }
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

        //static private void determinePath(Creature creature, Environment.Map map)
        //{
        //    Vector2 tmp = new Vector2();
        //    Queue<Vector2> queue = new Queue<Vector2>();
        //    queue.Enqueue(creature.Position);
        //    map.getHexagonAt(creature.Position).Visited = true;

        //    while (queue.Peek() != null)
        //    {
        //        tmp = queue.Dequeue();
        //        if (tmp == creature.getHome().getTarget()) break;
        //        foreach (Vector2 hex in map.getHexagonAt(tmp).getNeighbors())
        //        {
        //            if (!map.getHexagonAt(hex).Visited && (map.getHexagonAt(hex).Obj == null || map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.Wall))
        //            {
        //                queue.Enqueue(hex);
        //                map.getHexagonAt(hex).Visited = true;
        //                map.getHexagonAt(hex).Parent = tmp;
        //            }
        //        }
        //    }

        //    while (tmp != creature.Position)
        //    {
        //        creature.Path.Push(tmp);
        //        tmp = map.getHexagonAt(tmp).Parent;
        //    }
        //    for (int i = 0; i < map.getPlanelength(); ++i)
        //    {
        //        for (int j = 0; j < map.getPlanelength(); ++j)
        //        {
        //            map.getHexagonAt(i, j).Visited = false;
        //            map.getHexagonAt(i, j).Parent = new Vector2(i,j);
        //        }
        //    }
        //}

        static private void randomwalk(Creature creature, Environment.Map map)
        {
            //random determination of next step
            creature.Path.Push(map.getHexagonAt(creature.Position).getNeighbors()[(int)rand.Next(6)]);
        }
    }
}
