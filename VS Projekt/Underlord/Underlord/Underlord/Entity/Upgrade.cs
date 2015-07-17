using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Underlord.Renderer;
using Microsoft.Xna.Framework;
using Underlord.Logic;

namespace Underlord.Entity
{
    class Upgrade : Thing
    {
        // TODO everything inclusiv filter funktions
        Vars_Func.ThingTyp thingTyp;
        String name = "";
        int cost = 150;
        float size = 1;

        public Upgrade(String name)
        {
            thingTyp = Vars_Func.ThingTyp.Upgrade;
            this.name = name;
        }
        public String getName() { return this.name; }

        override public void update(GameTime time, Environment.Map map)
        {

        } 

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        { 

        }
    }
}
