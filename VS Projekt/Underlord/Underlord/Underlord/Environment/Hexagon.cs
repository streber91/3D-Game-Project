using System;
using System.Collections.Generic;
using System.Linq;
using Underlord.Entity;
using Underlord.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Underlord.Environment
{
    class Hexagon
    {
        Vector3 position;
        Vector2 indexNumber;
        Vector2[] neighbors = new Vector2[6]; //[up,right-up,right-down,down,left-down,left-up]
        List<Boolean> imps;
        Thing obj;
        Color drawColor;
        Entity.Vars_Func.HexTyp typ;
        //Model hexagonModel;
        private Matrix[] boneTransforms;

        #region Properties
        public Color Color
        {
            get { return drawColor; }
            set { drawColor = value; }
        }
        public Thing Obj
        {
            get { return obj; }
            set { obj = value; }
        }
        #endregion

        public Hexagon(Vector3 position, Vector2 indexNumber, Vector2[] neighbors, Entity.Vars_Func.HexTyp typ)
        {
            this.position = position;
            this.indexNumber = indexNumber;
            this.neighbors = neighbors;
            drawColor = Color.White;
            this.typ = typ;
            boneTransforms = new Matrix[Entity.Vars_Func.getHexagonModell(typ).Bones.Count];
        }

        public Vector3 get3DPosition() { return position; }
        public Vector2 getIndexNumber() { return indexNumber; }
        public Vector2[] getNeighbors() { return neighbors; }
        public void getImp() { } //TODO

        public void setObjekt(Thing obj) { this.obj = obj; }

        public void addImp()
        {
            this.imps.Add(true);
        }

        public void DrawModel(Camera camera, Vector3 drawPosition)
        {
            Entity.Vars_Func.getHexagonModell(typ).Root.Transform = Matrix.Identity *

            Matrix.CreateScale(/*0.0127f*/1) *
            Matrix.CreateRotationX(0/*2*MathHelper.PiOver2*/) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0/*MathHelper.PiOver2*/) *
            Matrix.CreateTranslation(drawPosition + new Vector3(0.0f, 0.0f, 0));
            Entity.Vars_Func.getHexagonModell(typ).CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in Entity.Vars_Func.getHexagonModell(typ).Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    basicEffect.EnableDefaultLighting();
                    basicEffect.World = boneTransforms[mesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;

                    basicEffect.AmbientLightColor = new Vector3(this.drawColor.R, this.drawColor.G, this.drawColor.B);
                }
                mesh.Draw();
            }
             if(obj != null) obj.DrawModel(camera, drawPosition /*+ Vector3.UnitZ*/, drawColor);
        }
    }
}
