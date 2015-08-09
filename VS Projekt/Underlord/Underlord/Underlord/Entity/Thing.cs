using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Underlord.Logic;

namespace Underlord.Entity
{
    abstract class Thing
    {
        protected Vars_Func.ThingTyp thingTyp;       
        abstract public void update(GameTime gameTime, Environment.Map map);
        abstract public void DrawModel(Renderer.Camera camera, Vector3 drawPosition, Color drawColor);

        public Vars_Func.ThingTyp getThingTyp() { return thingTyp; }
    }
}
