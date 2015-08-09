using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Entity
{
    class Temple : Thing
    {
        Vector2 position;

        #region Properties
        public Vector2 Position
        {
            get { return position; }
        }
        #endregion

        #region Constructor
        public Temple(Vector2 position, Environment.Map map)
        {
            thingTyp = Logic.Vars_Func.ThingTyp.Temple;
            this.position = position;

            map.getHexagonAt(position).Obj = this;
            map.Temples.Add(this);
        }
        #endregion

        override public void update(GameTime gameTime, Environment.Map map)
        {

        }

        override public void DrawModel(Renderer.Camera camera, Vector3 drawPosition, Color drawColor)
        {
 
        }
    }
}
