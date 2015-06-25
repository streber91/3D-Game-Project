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
            if (creature.ActionTimeCounter >= 1000 / creature.getSpeed())
            {
                //if(gegner in angrifsreichweite) angreifen
                //else if(computeNearestEnemy != null) auf gegner zubewegen else
                if (creature.Path.Count == 0)
                {
                    if (Entity.Vars_Func.computeDistance(creature.getHome().getTarget(), creature.Position, map) < 5) randomwalk(creature, map);
                    else determinePath(creature, map);
                }

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
            return null;
        }

        static private void determinePath(Creature creature, Environment.Map map)
        {
            Vector2 tmp = new Vector2();
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(creature.Position);
            map.getHexagonAt(creature.Position).Visited = true;

            while (queue.Peek() != null)
            {
                tmp = queue.Dequeue();
                if (tmp == creature.getHome().getTarget()) break;
                foreach (Vector2 hex in map.getHexagonAt(tmp).getNeighbors())
                {
                    if (!map.getHexagonAt(hex).Visited && (map.getHexagonAt(hex).Obj == null || map.getHexagonAt(hex).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.Wall))
                    {
                        queue.Enqueue(hex);
                        map.getHexagonAt(hex).Visited = true;
                        map.getHexagonAt(hex).Parent = tmp;
                    }
                }
            }

            while (tmp != creature.Position)
            {
                creature.Path.Push(tmp);
                tmp = map.getHexagonAt(tmp).Parent;
            }
            for (int i = 0; i < map.getPlanelength(); ++i)
            {
                for (int j = 0; j < map.getPlanelength(); ++j)
                {
                    map.getHexagonAt(i, j).Visited = false;
                    map.getHexagonAt(i, j).Parent = new Vector2(i,j);
                }
            }
        }

        static private void randomwalk(Creature creature, Environment.Map map)
        {
            creature.Path.Push(map.getHexagonAt(creature.Position).getNeighbors()[(int)rand.Next(6)]);
        }
    }
}
