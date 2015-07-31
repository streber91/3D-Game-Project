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
        Vars_Func.UpgradeTyp typ;
        Vector2 position;
        int cost;

        #region Properties

        #endregion

        #region Constructor
        public Upgrade(Vars_Func.UpgradeTyp typ, Vector2 position, Environment.Hexagon hex, Environment.Map map)
        {
            thingTyp = Vars_Func.ThingTyp.Upgrade;
            this.typ = typ;
            this.position = position;
            hex.Obj = this;
            hex.Building = true;
            for (int i = 0; i < 6; ++i)
            {
                Vector2 neighbor = hex.Neighbors[i];
                map.getHexagonAt(neighbor).Building = true;
            }
        }
        #endregion

        override public void update(GameTime gameTime, Environment.Map map)
        {

        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(1) *
            Matrix.CreateRotationX(0) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            Vars_Func.getUpgradeModell(typ).Color = drawColor;
            Vars_Func.getUpgradeModell(typ).Draw(camera, modelMatrix);
        }
    }
}
