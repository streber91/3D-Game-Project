using System;
using System.Collections.Generic;
using System.Linq;
using Underlord.Renderer;
using Microsoft.Xna.Framework;

namespace Underlord.Entity
{
    abstract class Thing
    {
        protected Vars_Func.ThingTyp thingTyp;
        abstract public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor);

        public Vars_Func.ThingTyp getThingTyp() { return thingTyp; }
    }
}
