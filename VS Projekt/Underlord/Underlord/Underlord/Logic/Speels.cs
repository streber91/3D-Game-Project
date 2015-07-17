using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Logic
{
    static class Speels
    {

        public static void summonImp(Environment.Map map)
        {
            new Imp(map.getHexagonAt(map.HQPosition).Neighbors[3], map);
        }

        public static void fireball(Vector2 position, Environment.Map map)
        {
            // is the target a creature?
            if (map.getHexagonAt(position).Obj != null && (map.getHexagonAt(position).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.DungeonCreature ||
                    map.getHexagonAt(position).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.NeutralCreature || map.getHexagonAt(position).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.HeroCreature))
            {
                //damage creature
                Creature target = (Creature)map.getHexagonAt(position).Obj;
                target.takeDamage(100);
                //is creature dead?
                if (target.DamageTaken >= target.HP) map.remove(target);
            }
            // is the target an imp?
            else if (map.getHexagonAt(position).Obj != null && map.getHexagonAt(position).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.Imp)
            {
                //damage imp
                Imp target = (Imp)map.getHexagonAt(position).Obj;
                target.takeDamage(100);
                // is imp dead?
                if (target.DamageTaken >= target.HP)
                {
                    map.JobsWaiting.Enqueue(target.CurrentJob);
                    map.JobsInProgress.Remove(target.CurrentJob);
                    map.remove(target);
                }
            }
            // effects neighbors
            foreach (Vector2 hex in map.getHexagonAt(position).Neighbors)
            {
                // is the target a creature?
                if (map.getHexagonAt(hex).Obj != null && (map.getHexagonAt(hex).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.DungeonCreature ||
                    map.getHexagonAt(hex).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.NeutralCreature || map.getHexagonAt(hex).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.HeroCreature))
                {
                    //damage creature
                    Creature target = (Creature)map.getHexagonAt(hex).Obj;
                    target.takeDamage(100);
                    //is creature dead?
                    if (target.DamageTaken >= target.HP) map.remove(target);
                }
                // is the target an imp?
                else if (map.getHexagonAt(hex).Obj != null && map.getHexagonAt(hex).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.Imp)
                {
                    //damage imp
                    Imp target = (Imp)map.getHexagonAt(hex).Obj;
                    target.takeDamage(100);
                    // is imp dead?
                    if (target.DamageTaken >= target.HP)
                    {
                        map.JobsWaiting.Enqueue(target.CurrentJob);
                        map.JobsInProgress.Remove(target.CurrentJob);
                        map.remove(target);
                    }
                }
            }
        }
    }
}
