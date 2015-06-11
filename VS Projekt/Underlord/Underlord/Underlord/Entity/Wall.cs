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
        Vector2 indexPosition;
        Vars_Func.TypWall type;
        int hp;

        Model objectModel;
        private Matrix[] boneTransforms;

        public Wall(Vector2 indexPosition, Vars_Func.TypWall type, int hp, Map map, Model model)
        {
            this.indexPosition = indexPosition;
            this.type = type;
            this.hp = hp;
            map.getHexagonAt(indexPosition.X, indexPosition.Y).Obj = this;
            objectModel = model;
            boneTransforms = new Matrix[this.objectModel.Bones.Count];
        }

        override public void DrawModel(Camera camera, Vector3 drawPosition, Color drawColor)
        {
            this.objectModel.Root.Transform = Matrix.Identity *

            Matrix.CreateScale(1) *
            Matrix.CreateRotationX(0/*MathHelper.PiOver2*/) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);
            this.objectModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in this.objectModel.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    basicEffect.EnableDefaultLighting();
                    basicEffect.World = boneTransforms[mesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;
                    basicEffect.AmbientLightColor = new Vector3(drawColor.R, drawColor.G, drawColor.B);
                }
                mesh.Draw();
            }
        }
    }
}
