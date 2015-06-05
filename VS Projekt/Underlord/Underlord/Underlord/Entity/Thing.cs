﻿using System;
using System.Collections.Generic;
using System.Linq;
using Underlord.Renderer;
using Microsoft.Xna.Framework;

namespace Underlord.Entity
{
    abstract class Thing
    {
        abstract public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor);
    }
}
