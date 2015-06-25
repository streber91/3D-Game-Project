using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Renderer;
using Microsoft.Xna.Framework;

namespace Underlord.Entity
{
    class Upgrade : Thing
    {
        Vars_Func.ThingTyp thingTyp;
        String name = "";
        int cost = 150;
        float size = 1;
        Ability effect;

        public Upgrade(String name, Ability eff)
        {
            thingTyp = Vars_Func.ThingTyp.Upgrade;
            this.name = name;
            this.effect = eff;
        }
        public Ability getEffect()
        {
            return this.effect;
        }
        public String getName()
        {
            return this.name;
        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        { 

        }
    }
}
