using System;
using System.Collections.Generic;
using System.Linq;
using Underlord.Renderer;
using Underlord.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Underlord.Entity
{
    class Wall : Thing
    {
        Vars_Func.ThingTyp thingTyp;
        Vector2 indexPosition;
        Vars_Func.WallTyp typ;
        int hp;
       // private Matrix[] boneTransforms;

        public Texture2D Texture{

            set { Entity.Vars_Func.getWallModell(typ).Texture = value; }
        }


        #region Constructor
        public Wall(Vector2 indexPosition, Vars_Func.WallTyp typ, int hp, Map map)
        {
            thingTyp = Vars_Func.ThingTyp.Wall;
            this.indexPosition = indexPosition;
            this.typ = typ;
            this.hp = hp;
            map.getHexagonAt(indexPosition.X, indexPosition.Y).Obj = this;
            //boneTransforms = new Matrix[Entity.Vars_Func.getWallModell(typ).Model.Bones.Count];
        }
        #endregion

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(1) *
            Matrix.CreateRotationX(0/*MathHelper.PiOver2*/) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            Entity.Vars_Func.getWallModell(typ).Color = drawColor;
            Entity.Vars_Func.getWallModell(typ).Draw(camera, modelMatrix);

            //Entity.Vars_Func.getWallModell(typ).Model.Root.Transform = Matrix.Identity *
            //Matrix.CreateScale(1) *
            //Matrix.CreateRotationX(0/*MathHelper.PiOver2*/) *
            //Matrix.CreateRotationY(0) *
            //Matrix.CreateRotationZ(0) *
            //Matrix.CreateTranslation(drawPosition);
            //Entity.Vars_Func.getWallModell(typ).Model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            //foreach (ModelMesh mesh in Entity.Vars_Func.getWallModell(typ).Model.Meshes)
            //{
            //    foreach (BasicEffect basicEffect in mesh.Effects)
            //    {
            //        basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //        basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            //        basicEffect.EnableDefaultLighting();
            //        basicEffect.World = boneTransforms[mesh.ParentBone.Index];
            //        basicEffect.View = camera.View;
            //        basicEffect.Projection = camera.Projection;
            //        basicEffect.AmbientLightColor = new Vector3(drawColor.R, drawColor.G, drawColor.B);
            //    }
            //    mesh.Draw();
            //}
        }
    }
}
